using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude2 : MonoBehaviour
{
    private Vector3 m_velocity;
    private float m_jumpSpeed = 12.0f;
    private bool m_started = false;

    public Vector3 m_lookTargetSmooth;
    private Vector3 m_lookTarget;
    private Vector3 m_lookTargetVelocity;

    private int platformIndex;
    public Platform[] platforms;

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

        m_velocity += Vector3.down * Physics.G * Time.deltaTime;
        m_velocity += Vector3.back * Time.deltaTime;
        if (m_velocity.z < 0)
            m_velocity.z = 0;

        var position = transform.position;
        position += m_velocity * Time.deltaTime;
        transform.position = position;

        m_lookTargetSmooth = transform.position + transform.forward * 10;
        m_lookTargetSmooth = Vector3.SmoothDamp(m_lookTargetSmooth, m_lookTarget, ref m_lookTargetVelocity, 0.95f);

        transform.LookAt(m_lookTargetSmooth);
    }

    private void Jump()
    {
        float speed = m_jumpSpeed;

        m_velocity = new Vector3(0, 1, 1);
        m_velocity.Normalize();
        m_velocity *= speed;
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
