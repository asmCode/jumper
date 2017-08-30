using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude2 : MonoBehaviour
{
    private Vector3 m_velocity;
    private float m_jumpSpeed = 8.0f;
    private float m_jumpAngle = 8.0f;
    private bool m_started = false;

    public bool camJump = true;

    public Vector3 m_lookTargetSmooth;
    private Vector3 m_lookTarget;
    private Vector3 m_lookTargetVelocity;

    public Platform[] platforms;

    public float m_horizontalSpeed;
    public float m_horizontalDistance;
    public Vector3 m_horizontalDirection;

    public Vector3 m_jumpPosition;

    public JumpPointView m_prevPlatform;
    public JumpPointView m_nextPlatform;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    private void Init()
    {
        var firstPlatformJumpPoint = platforms[0].GetComponent<PlatformJumpPointView>();

        m_prevPlatform = firstPlatformJumpPoint;
        m_nextPlatform = firstPlatformJumpPoint.m_nextPlatform.GetComponent<JumpPointView>();

        SetLookTarget(m_prevPlatform, m_nextPlatform);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (camJump)
            {
                if (!m_started)
                {
                    Jump(m_prevPlatform.Position, 8.0f, 45.0f * Mathf.Deg2Rad);
                    m_started = true;
                }
                else
                    Jump(m_nextPlatform.Position, 8.0f, 45.0f * Mathf.Deg2Rad);

                camJump = false;
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
        transform.LookAt(m_lookTargetSmooth);
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

        SetLookTarget(m_prevPlatform, m_nextPlatform);

        Jump(m_nextPlatform.Position, m_prevPlatform.GetJumpSpeed(), m_prevPlatform.GetJumpAngle());

        camJump = true;
    }

    private void SetLookTarget(JumpPointView prevJumpPoint, JumpPointView nextJumpPoint)
    {
        Vector3 direction = nextJumpPoint.Position - prevJumpPoint.Position;
        direction = direction.normalized * 1.0f;

        Vector3 lookTarget = nextJumpPoint.Position + direction;
        SetLookTarget(lookTarget);
    }

    private void SetLookTarget(Vector3 lookTarget)
    {
        m_lookTarget = lookTarget;
    }

    private void UpdateSmoothLookTarget()
    {
        m_lookTargetSmooth = Vector3.SmoothDamp(m_lookTargetSmooth, m_lookTarget, ref m_lookTargetVelocity, 0.6f);
    }
}
