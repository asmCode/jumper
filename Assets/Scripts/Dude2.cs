using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude2 : MonoBehaviour
{
    private Vector3 m_velocity;
    private float m_jumpSpeed = 8.0f;
    private bool m_started = false;

    public Vector3 m_lookTargetSmooth;
    private Vector3 m_lookTarget;
    private Vector3 m_lookTargetVelocity;

    private int platformIndex;
    public Platform[] platforms;

    public float m_horizontalSpeed;
    public float m_horizontalDistance;
    public Vector3 m_horizontalDirection;

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
            m_started = true;
            RecalculateLookTarget();
            Jump();
        }

        if (!m_started)
            return;

        //m_velocity += Vector3.down * Physics.G * Time.deltaTime;
        ////m_velocity += Vector3.back * Time.deltaTime;
        ////if (m_velocity.z < 0)
        ////    m_velocity.z = 0;

        //var position = transform.position;
        //position += m_velocity * Time.deltaTime;
        //transform.position = position;

        m_horizontalDistance += m_horizontalSpeed * Time.deltaTime;
        float height = Physics.GetHeightAtDistance(0, 45.0f * Mathf.Deg2Rad, m_horizontalDistance, m_jumpSpeed);

        var position = m_jumpPosition;
        position += m_horizontalDirection * m_horizontalDistance;
        position.y += height;

        transform.position = position;

        m_lookTargetSmooth = transform.position + transform.forward * 10;
        m_lookTargetSmooth = Vector3.SmoothDamp(m_lookTargetSmooth, m_lookTarget, ref m_lookTargetVelocity, 0.95f);

        transform.LookAt(m_lookTargetSmooth);
    }

    private void Jump()
    {
        float speed = m_jumpSpeed;

        m_jumpPosition = transform.position;

        Vector3 platformPosition = platforms[platformIndex].transform.position;

        m_horizontalDirection = platformPosition - m_jumpPosition;
        m_horizontalDirection.y = 0.0f;
        m_horizontalDirection.Normalize();

        //Vector3 axis = Vector3.Cross(Vector3.up, m_horizontalDirection);
        //var q = Quaternion.AngleAxis(-45.0f, axis);
        //var jumpDirection = q * m_horizontalDirection;

        m_horizontalDistance = 0.0f;
        m_horizontalSpeed = m_jumpSpeed * Mathf.Cos(45.0f * Mathf.Deg2Rad);
    }

    private void OnTriggerEnter(Collider collider)
    {
        var platform = collider.gameObject.GetComponent<Platform>();
        if (!platform)
            return;

        platformIndex++;

        RecalculateLookTarget();

        Jump();
    }

    private void RecalculateLookTarget()
    {
        m_lookTarget = platforms[platformIndex].transform.position;

        // float distance = (m_lookTarget - transform.position).magnitude;
    }
}
