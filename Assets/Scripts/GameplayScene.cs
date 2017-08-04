using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScene : MonoBehaviour
{
    public Platform m_platformPrefab;
    public Dude m_dude;

    private Track m_track;
    private int m_nextPlatformIndex = 0;

    private Vector3 m_startPosition;
    private Vector3 m_endPosition;
    private float m_distance;
    private float m_segmentDistance;
    private float m_totalTime;

    private float m_speed = 10.00f;
    private float m_g = 7.0f;
    private float m_segmentTime;

    private void Start()
    {
        TrackGenerator trackGenerator = new TrackGenerator();
        m_track = trackGenerator.Generate(m_platformPrefab);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        // m_segmentDistance += m_speed * Time.deltaTime;
        m_segmentTime += Time.deltaTime;

        float d = m_distance;
        float v = m_speed;
        float y = m_endPosition.y - m_startPosition.y;

        float a = Mathf.Atan((v * v - Mathf.Sqrt(v*v*v*v - m_g * (m_g * d * d + 2 * y * v * v))) / (m_g * d));
        // a = Mathf.Deg2Rad * 20.0f;
        Debug.LogFormat("a = {0}", a);

        float cos_a = Mathf.Cos(a);
        float v_cos_a = v * cos_a;

        m_totalTime = d / (v_cos_a);

        m_segmentDistance = (m_segmentTime / m_totalTime) * m_distance;

        float x = m_segmentDistance;

        float h =
                m_startPosition.y +
                x * Mathf.Tan(a) -
                ((m_g * x * x)) / (2 * v_cos_a * v_cos_a);

        Debug.LogFormat("a = {0}, totalTime = {1}, m_segmentDistance = {2}, m_segmentTime = {3}, m_distance = {4}", a, m_totalTime, m_segmentDistance, m_segmentTime, m_distance);

        var position = new Vector3(0, h, m_startPosition.z + m_segmentDistance);

        m_dude.transform.position = position;
    }

    public void Jump()
    {
        var platform = m_track.GetPlatform(m_nextPlatformIndex);
        var next_platform = m_track.GetPlatform(m_nextPlatformIndex + 1);

        m_nextPlatformIndex++;

        m_startPosition = platform.transform.position;
        m_endPosition = next_platform.transform.position;
        m_distance = Vector3.Distance(m_startPosition, m_endPosition);

        m_segmentDistance = 0.0f;
        m_segmentTime = 0.0f;
    }
}
