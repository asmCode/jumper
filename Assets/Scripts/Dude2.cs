using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dude2 : MonoBehaviour
{
    private float m_jumpSpeed = 8.0f;
    private float m_jumpAngle = 8.0f;
    private bool m_started = false;

    private AudioSource m_soundJump;
    private AudioSource m_soundLand;

    public bool m_autoJump = false;

    private DureCamera m_dudeCamera;
    private bool m_camJump = true;

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

    public JumpPointView PrevPlatform { get { return m_prevPlatform; } }
    public JumpPointView NextPlatform { get { return m_nextPlatform; } }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    private void Awake()
    {
        m_soundJump = transform.Find("Jump").GetComponent<AudioSource>();
        m_soundLand = transform.Find("Land").GetComponent<AudioSource>();
        m_dudeCamera = transform.Find("CameraAnimRoot").GetComponent<DureCamera>();
    }

    private void Init()
    {
        m_prevPlatform = m_firstJumpPoint;
        m_nextPlatform = m_firstJumpPoint.GetComponent<PlatformJumpPointView>().m_nextPlatform;

        SetLookTarget(m_prevPlatform, m_nextPlatform);
        LookTargetSmooth = m_prevPlatform.Position;

        var current = m_firstJumpPoint.GetComponent<PlatformJumpPointView>();
        while (current != null)
        {
            if (current.m_nextPlatform != null)
            {
                current.m_nextPlatform.GetComponent<PlatformJumpPointView>().m_prevPlatform = current;
                current = current.m_nextPlatform.GetComponent<PlatformJumpPointView>();
            }
            else
                current = null;
        }
    }

    public void UiEvent_Reset()
    {
        SceneManager.LoadScene("Gameplay2");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (m_camJump)
            {
                if (!m_started)
                {
                    Jump(m_prevPlatform.Position, 8.0f, 45.0f * Mathf.Deg2Rad);
                    m_started = true;
                }
                else
                    AirJump();

                m_camJump = false;
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

        if (transform.position.y < 0)
        {
            SceneManager.LoadScene("Gameplay2");
            return;
        }

        if (m_autoJump &&
            m_horizontalDistance >= m_prevPlatform.GetComponent<PlatformJumpPointView>().m_airJumpOnDistance &&
            m_camJump)
        {
            AirJump();
            m_camJump = false;
        }
    }

    private void AirJump()
    {
        Jump(m_nextPlatform.Position, 8.0f, 45.0f * Mathf.Deg2Rad);

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

    private void OnTriggerEnter(Collider collider)
    {
        var platform = collider.gameObject.GetComponent<Platform>();
        if (!platform)
            return;

        if (platform.Visited)
            return;

        platform.Visited = true;

        var platformJumpPoint = platform.GetComponent<PlatformJumpPointView>();
        m_prevPlatform = platformJumpPoint;
        m_nextPlatform = platformJumpPoint.m_nextPlatform;

        if (m_nextPlatform == null)
        {
            SceneManager.LoadScene("Gameplay2");
        }

        SetLookTarget(m_prevPlatform, m_nextPlatform);

        Jump(m_nextPlatform.Position, m_prevPlatform.GetJumpSpeed(), m_prevPlatform.GetJumpAngle());

        m_dudeCamera.PlayPlatformJumpAnimation();

        m_soundLand.Play();

        m_camJump = true;
    }

    private void SetLookTarget(JumpPointView prevJumpPoint, JumpPointView nextJumpPoint)
    {
        Vector3 direction = nextJumpPoint.Position - prevJumpPoint.Position;
        direction = direction.normalized * 4.0f;

        Vector3 lookTarget = nextJumpPoint.Position + direction;
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
