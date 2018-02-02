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

    private AudioSource m_soundJump;
    private AudioSource m_soundLand;

    public bool m_autoJump = false;

    private DureCamera m_dudeCamera;
    private bool m_canJump = true;

    public Vector3 LookTargetSmooth { get; private set; }
    private Vector3 m_lookTarget;
    private Vector3 m_lookTargetVelocity;

    public JumpPointView m_firstJumpPoint;

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

	public Camera HUDCamera;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    private void Awake()
    {
        var playViewPresenterGameObject = GameObject.Find("PlayView");
        var playViewPresenter = playViewPresenterGameObject.GetComponent<PlayViewPresenter>();
        playViewPresenter.RetryPressed.AddListener(() => { UiEvent_Reset(); });

        m_soundJump = transform.Find("Jump").GetComponent<AudioSource>();
        m_soundLand = transform.Find("Land").GetComponent<AudioSource>();
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

    private void Init()
    {
        m_prevPlatform = m_firstJumpPoint;
        m_nextPlatform = m_firstJumpPoint.GetComponent<PlatformJumpPointView>().NextPlatform;

        transform.position = m_firstJumpPoint.Position + Vector3.up;

        SetLookTarget(m_nextPlatform, m_nextPlatform);
        LookTargetSmooth = m_nextPlatform.Position;
        transform.LookAt(LookTargetSmooth);

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

        if (m_isFallingDown)
        {
            var positon = transform.position;
            m_fallingDownVelocity += GravityVector * Time.deltaTime;
            positon += m_fallingDownVelocity * Time.deltaTime;
            transform.position = positon;

            Quaternion rollRotation = Quaternion.AngleAxis(m_rollAngleSpeed * Time.deltaTime, transform.forward);
            transform.rotation = rollRotation * transform.rotation;

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
            m_died = true;
            OnDied.Invoke();
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || (!m_started && m_autoJump))
        {
            if (m_canJump)
            {
                if (!m_started)
                {
                    Jump(m_prevPlatform.NativePosition, 0.001f, 45.0f * Mathf.Deg2Rad);
                    m_started = true;
                }
                else
                    AirJump();

                m_canJump = false;
            }
			else{
				UnityEngine.RaycastHit hit;
				UnityEngine.Ray ray = HUDCamera.ScreenPointToRay(Input.mousePosition);

				if (UnityEngine.Physics.Raycast(ray, out hit)) {
					if (hit.transform.tag == "Clickable"){
						var t_TargetTrigger = hit.transform.gameObject.GetComponent<TargetTrigger>();
						t_TargetTrigger.NotifyHit();
					}
				}
			}
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
        transform.LookAt(LookTargetSmooth);

        if (m_autoJump &&
            m_horizontalDistance >= m_prevPlatform.GetComponent<PlatformJumpPointView>().AirJumpOnDistance &&
            m_canJump)
        {
            AirJump();
            m_canJump = false;
        }
    }

    private void AirJump()
    {
        Jump(m_nextPlatform.NativePosition, 8.0f, 45.0f * Mathf.Deg2Rad);

        m_soundJump.Play();

        m_dudeCamera.PlayAirJumpAnimation();
    }

    private void Jump(Vector3 targetPlatformPosition, float speed, float angle)
    {
        m_jumpSpeed = speed;
        m_jumpAngle = angle;
        
        m_jumpPosition = transform.position;

        m_horizontalDirection = targetPlatformPosition - m_jumpPosition;
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

        if (m_nextPlatform == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        SetLookTarget(m_prevPlatform, m_nextPlatform);

        Jump(m_nextPlatform.NativePosition, m_prevPlatform.GetJumpSpeed(), m_prevPlatform.GetJumpAngle());

        m_dudeCamera.PlayPlatformJumpAnimation();

        m_soundLand.Play();

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
}
