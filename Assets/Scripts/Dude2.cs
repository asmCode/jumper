using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Dude2 : MonoBehaviour
{

    private static Vector3 GravityVector = new Vector3(0, -9.8f, 0);

    public UnityEvent OnScoredPlatform;
    public UnityEvent OnDied;

    private float m_jumpSpeed = 8.0f;
    private float m_jumpAngle = 8.0f;
    private bool m_started = false;

    private bool m_isFallingDown;
    private float m_rollAngle;
    private float m_rollAngleSpeed;
    private Vector3 m_fallingDownVelocity;

    public TouchInput m_touchInput;

    //private AudioSource m_soundJump;
    //private AudioSource m_soundLand;

    public bool m_autoJump = false;

    private DureCamera m_dudeCamera;
    private bool m_canJump = true;

    public Vector3 LookTargetSmooth { get; private set; }
    private Vector3 m_lookTarget;
    private Vector3 m_lookTargetVelocity;

    public JumpPointView m_firstJumpPoint;

    private bool gamePaused;

    private float m_horizontalSpeed;
    private float m_horizontalDistance;
    private Vector3 m_horizontalDirection;

    private Vector3 m_jumpPosition;

    private JumpPointView m_prevPlatform;
    private JumpPointView m_nextPlatform;

    private bool m_died;

    public int PlatformsScored { get; private set; }

    public JumpPointView PrevPlatform { get { return m_prevPlatform; } }
    public JumpPointView NextPlatform { get { return m_nextPlatform; } }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    private void Awake()
    {
        //m_soundJump = transform.Find("Jump").GetComponent<AudioSource>();
        //m_soundLand = transform.Find("Land").GetComponent<AudioSource>();
        m_dudeCamera = transform.Find("CameraAnimRoot").GetComponent<DureCamera>();

        if (m_firstJumpPoint == null)
        {
            var track = GameObject.Find("Track");
            if (track != null && track.transform.childCount > 1)
            {
                m_firstJumpPoint = track.transform.GetChild(0).GetComponent<PlatformJumpPointView>();
            }
        }
    }

    private Vector3 lookDirection;

    private void Init()
    {
        lookDirection = Vector3.right;

        Time.timeScale = 1.0f;
        gamePaused = false;
        m_prevPlatform = m_firstJumpPoint;
        m_nextPlatform = m_firstJumpPoint.GetComponent<PlatformJumpPointView>().NextPlatform;

        transform.position = m_firstJumpPoint.Position + Vector3.up;

        SetLookTarget(m_nextPlatform, m_nextPlatform);
        LookTargetSmooth = m_nextPlatform.Position;
        //transform.LookAt(LookTargetSmooth);
        transform.forward = lookDirection;  


        var current = m_firstJumpPoint.GetComponent<PlatformJumpPointView>();
        while (current != null)
        {
            if (current.NextPlatform != null)
            {
                current.NextPlatform.GetComponent<PlatformJumpPointView>().PrevPlatform = current;
                current = current.NextPlatform.GetComponent<PlatformJumpPointView>();
            }
            else
                current = null;
        }
    }

    public void UiEvent_Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (m_died)
            return;

        lookDirection = Quaternion.AngleAxis(Mathf.Rad2Deg * 2.0f * m_touchInput.Shift, Vector3.up) * lookDirection;

        if (m_isFallingDown)
        {
            var positon = transform.position;
            m_fallingDownVelocity += GravityVector * Time.deltaTime;
            positon += m_fallingDownVelocity * Time.deltaTime;
            transform.position = positon;

            Quaternion rollRotation = Quaternion.AngleAxis(m_rollAngleSpeed * Time.deltaTime, transform.forward);
            // transform.rotation = rollRotation * transform.rotation;

            if (transform.position.y < 0 && !m_died)
            {
                m_died = true;
                OnDied.Invoke();
                return;
            }

            return;
        }

        if (transform.position.y < 0 && !m_died)
        {
            GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Complete, "level 1", PlatformsScored);

            m_died = true;
            OnDied.Invoke();
            return;
        }

        if ((Input.GetKeyDown(KeyCode.Space) || (!m_started && m_autoJump)) && !gamePaused)
        {
            if (m_canJump)
            {
                if (!m_started)
                {
                    Jump(0.001f, 45.0f * Mathf.Deg2Rad);
                    m_started = true;
                    GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start, "level 1", 0);
                }

                m_canJump = false;
            }
            else
            {
                UnityEngine.RaycastHit hit;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("AimPrototype");
        }

        if (!m_started)
            return;

        m_horizontalDistance += m_horizontalSpeed * Time.deltaTime;
        float height = Physics.GetHeightAtDistance(0, m_jumpAngle, m_horizontalDistance, m_jumpSpeed);

        var position = m_jumpPosition;
        position += m_horizontalDirection * m_horizontalDistance;
        position.y += height;

        UpdateSmoothLookTarget();

        transform.position = position;
        //transform.LookAt(LookTargetSmooth);
        transform.forward = lookDirection;
    }

    private void Jump(float speed, float angle)
    {
        m_jumpSpeed = speed;
        m_jumpAngle = angle;

        m_jumpPosition = transform.position;

        m_horizontalDirection = lookDirection;
        m_horizontalDirection.y = 0.0f;
        m_horizontalDirection.Normalize();

        m_horizontalDistance = 0.0f;
        m_horizontalSpeed = m_jumpSpeed * Mathf.Cos(angle);
    }

    private void OnBodyCollision(BodyCollider bodyCollider)
    {
        m_isFallingDown = true;

        m_rollAngle = 0.0f;
        m_rollAngleSpeed = Random.Range(50.0f, 150.0f) * Mathf.Sign(Random.Range(-1.0f, 1.0f));
        m_fallingDownVelocity = -m_horizontalDirection.normalized * 2.0f;
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Ignore collisions if falling down.
        if (m_isFallingDown)
            return;

        var bodyCollider = collider.gameObject.GetComponent<BodyCollider>();
        if (bodyCollider != null && !bodyCollider.m_platform.Visited)
        {
            OnBodyCollision(bodyCollider);
            return;
        }

        var platform = collider.gameObject.GetComponent<Platform>();
        if (!platform)
            return;

        if (platform.Visited)
            return;

        platform.Visited = true;

        PlatformsScored++;

        var platformJumpPoint = platform.GetComponent<PlatformJumpPointView>();
        m_prevPlatform = platformJumpPoint;
        m_nextPlatform = platformJumpPoint.NextPlatform;

        platformJumpPoint.NotifyJump();

        // We've reached the last platform.
        if (m_nextPlatform == null)
        {
            Time.timeScale = 0.0f;
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        SetLookTarget(m_prevPlatform, m_nextPlatform);

        var direction = m_nextPlatform.NativePosition - transform.position;
        var direction2d = new Vector2(direction.x, direction.z);
        var speed = Physics.GetRequiredSpeed(m_prevPlatform.GetJumpAngle(), direction2d.magnitude, direction.y);

        Jump(speed, m_prevPlatform.GetJumpAngle());

        m_dudeCamera.PlayPlatformJumpAnimation();

        switch (collider.tag)
        {
            case "platformNormal":
                break;
            case "platformEject":
                break;
            case "platformSlide":
                break;
        }

        m_canJump = true;
    }

    private void SetLookTarget(JumpPointView prevJumpPoint, JumpPointView nextJumpPoint)
    {
        Vector3 direction = nextJumpPoint.NativePosition - prevJumpPoint.NativePosition;
        direction = direction.normalized * 4.0f;

        Vector3 lookTarget = nextJumpPoint.NativePosition + direction;
        SetLookTarget(lookTarget);
    }

    private void SetLookTarget(Vector3 lookTarget)
    {
        m_lookTarget = lookTarget;
    }

    private void UpdateSmoothLookTarget()
    {
        LookTargetSmooth = Vector3.SmoothDamp(LookTargetSmooth, m_lookTarget, ref m_lookTargetVelocity, 0.4f);
    }

    public void gamePause()
    {
        gamePaused = !gamePaused;
    }

    public void gameLeave()
    {
    }
}
