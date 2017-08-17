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

    private int platformIndex;
    public Platform[] platforms;

    public float m_horizontalSpeed;
    public float m_horizontalDistance;
    public Vector3 m_horizontalDirection;
    public void

    public Vector3 m_jumpPosition;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (camJump)
            {

                m_started = true;
                RecalculateLookTarget();
                Jump( 8.0f, 45.0f * Mathf.Deg2Rad);

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

        transform.position = position;

        m_lookTargetSmooth = transform.position + transform.forward * 10;
        m_lookTargetSmooth = Vector3.SmoothDamp(m_lookTargetSmooth, m_lookTarget, ref m_lookTargetVelocity, 0.95f);

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

        var jumpPoimt = platform.GetComponent<JumpPointView>();

        platformIndex++;

        RecalculateLookTarget();

        Jump(jumpPoimt.Position, jumpPoimt.GetJumpSpeed(), jumpPoimt.GetJumpAngle());

        camJump = true;
    }

    private void RecalculateLookTarget()
    {
        m_lookTarget = platforms[platformIndex].transform.position;

        // float distance = (m_lookTarget - transform.position).magnitude;
    }
}
