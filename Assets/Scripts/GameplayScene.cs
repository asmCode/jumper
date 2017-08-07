using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayScene : MonoBehaviour
{
    public Platform m_platformPrefab;
    public Dude m_dude;

    private Track m_track;
    private int m_nextPlatformIndex = 0;

    private float m_speed = 21.415f;
    private float m_segmentTime;

    private bool m_started = false;
    private TrackDebugInfo m_trackDebugInfo = new TrackDebugInfo();

    private JumpPoint m_beginJumpPoint;
    private JumpPoint m_endJumpPoint;
    private JumpPoint m_endendJumpPoint;

    private Vector3 m_targetLookVector;

    private void Start()
    {
        TrackGenerator trackGenerator = new TrackGenerator();
        m_track = trackGenerator.Generate(m_platformPrefab);
    }

    private void Update()
    {
        m_trackDebugInfo.DrawTrack(m_track, m_speed);

        if (Input.GetMouseButtonDown(0))
        {
            Jump();
        }

        if (!m_started)
            return;

        m_segmentTime += Time.deltaTime;

        float angle = Physics.GetAngle(m_speed, m_beginJumpPoint.Distance, m_beginJumpPoint.HeightDifference);
        float totalTime = Physics.GetTotalTime(m_beginJumpPoint.Distance, m_speed, angle);
        float segmentDistance = Physics.GetDistanceAtTime(m_segmentTime, totalTime, m_beginJumpPoint.Distance);

        float h = Physics.GetHeightAtDistance(0, angle, segmentDistance, m_speed);

        var position = m_beginJumpPoint.Position + m_beginJumpPoint.Direction * segmentDistance;
        position.y = m_beginJumpPoint.Position.y + h;
        m_dude.transform.position = position;

        Vector3 lookVector = m_endJumpPoint.Position - m_dude.transform.position;
        lookVector.Normalize();

        m_dude.transform.LookAt(Vector3.Lerp(m_endJumpPoint.Position, m_endendJumpPoint.Position, m_segmentTime / totalTime));

        // m_dude.transform.forward = Vector3.RotateTowards(m_dude.transform.forward, lookVector, 5.0f * Time.deltaTime, 10 * Time.deltaTime);
        //Vector3.SmoothDamp()
    }

    public void Jump()
    {
        m_started = true;

        if (m_nextPlatformIndex == m_track.GetJumpPointCount() - 1)
        {
            SceneManager.LoadScene("Gameplay");
            return;
        }

        m_beginJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex);
        m_endJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex + 1);
        if (m_nextPlatformIndex < m_track.GetJumpPointCount() - 2)
            m_endendJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex + 2);
        else
            m_endendJumpPoint = m_endJumpPoint;

        m_nextPlatformIndex++;

        m_segmentTime = 0.0f;
    }
}
