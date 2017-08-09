using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour
{
    private JumperState m_state;
    public Track m_track;

    private JumpPoint m_beginJumpPoint;
    private JumpPoint m_endJumpPoint;
    private Vector3 nextPlatformPosition;
    private JumpPoint m_endendJumpPoint;

    public int m_nextPlatformIndex = 0;

    private float m_speed = 21.415f;
    public float m_jumpSpeed = 21.415f;
    private float m_landingSpeed = 10.0f;
    private float m_landingAngle = 60.0f * Mathf.Deg2Rad;
    private Vector3 m_lookTarget;

    private float m_runTime = 0.0f;
    private bool m_isRunning = false;

    private Vector3 m_lookTargetVelocity;

    public float Speed
    {
        get { return m_speed; }
    }

    public static float LastTime
    {
        get;
        private set;
    }

    public float SegmentTotalTime
    {
        get;
        private set;
    }

    public float SpeedChange
    {
        get;
        private set;
    }

    public float SegmentTime
    {
        get;
        private set;
    }

    public float SegmentAngle
    {
        get;
        private set;
    }

    public float JumpAccuracy
    {
        get;
        private set;
    }

    public float RunTime
    {
        get { return m_runTime; }
    }

    public float BestTime
    {
        get { return PlayerPrefs.GetFloat("best_time", 0.0f); }
    }

    public void StartRun()
    {
        m_runTime = 0.0f;
        m_isRunning = true;
    }

    public void StopRun()
    {
        float bestTime = PlayerPrefs.GetFloat("best_time", 0.0f);
        if (m_runTime < bestTime || bestTime == 0)
            PlayerPrefs.SetFloat("best_time", m_runTime);
        m_isRunning = false;

        LastTime = m_runTime;
    }

    public void SetTrack(Track track)
    {
        m_track = track;
    }

    public void SetState(JumperState state)
    {
        if (m_state != null)
            m_state.Leave();

        m_state = state;

        m_state.Enter();
    }

    public void Jump()
    {
        m_state.Jump();
    }

    private void Start()
    {
        SetState(new PrepareToStart(this));
    }

    private void Update()
    {
        m_state.Update(Time.deltaTime);

        if (m_isRunning)
            m_runTime += Time.deltaTime;
    }

    public void JumpToNextSegment()
    {
        if (m_endJumpPoint != null)
        {
            JumpAccuracy = Vector3.Distance(nextPlatformPosition, transform.position);

            float old_speed = m_jumpSpeed;

            if (JumpAccuracy < 0.3f)
            {
                m_jumpSpeed += 2.0f;
            }
            else if (JumpAccuracy < 0.5f)
            {
                m_jumpSpeed += 1.0f;
            }
            else if (JumpAccuracy < 0.7f)
            {
                m_jumpSpeed -= 0.0f;
            }
            else if (JumpAccuracy < 1.0f)
            {
                m_jumpSpeed -= 1.0f;
            }
            else if (JumpAccuracy >= 1.5f)
            {
                m_jumpSpeed -= 3.0f;
            }

            SpeedChange = m_jumpSpeed - old_speed;
        }

        m_speed = m_jumpSpeed;

        m_beginJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex);
        m_endJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex + 1);

        m_beginJumpPoint = new JumpPoint(transform.position, null, m_endJumpPoint.Position);

        if (m_nextPlatformIndex < m_track.GetJumpPointCount() - 2)
        {
            m_endendJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex + 2);
            nextPlatformPosition = m_endJumpPoint.Position;
        }
        else
            m_endendJumpPoint = m_endJumpPoint;

        m_nextPlatformIndex++;

        SegmentTime = 0.0f;

        SegmentAngle = Physics.GetAngle(m_speed, m_beginJumpPoint.Distance, m_beginJumpPoint.HeightDifference);
        if (float.IsNaN(SegmentAngle))
            SegmentAngle = 45 * Mathf.Deg2Rad;
        SegmentTotalTime = Physics.GetTotalTime(m_beginJumpPoint.Distance, m_speed, SegmentAngle);

        if (float.IsNaN(SegmentTotalTime))
        {
            int d = 0;
        }

        m_lookTarget = m_endJumpPoint.Position;
    }

    public void JumpAfterLanding()
    {
        m_speed = m_landingSpeed;
        SegmentAngle = m_landingAngle;

        m_beginJumpPoint = new JumpPoint(transform.position, m_beginJumpPoint.Direction, 3.0f, 0);
        m_endJumpPoint = new JumpPoint(transform.position + m_beginJumpPoint.Direction * 3.0f, m_beginJumpPoint.Direction, 3.0f, 0);

        SegmentTime = 0.0f;

        SegmentTotalTime = Physics.GetTotalTime(m_beginJumpPoint.Distance, m_speed, SegmentAngle);

        m_lookTarget = transform.position + transform.forward * 1000.0f;
    }

    public void UpdateJump()
    {
        SegmentTime += Time.deltaTime;

        float segmentDistance = Physics.GetDistanceAtTime(SegmentTime, SegmentTotalTime, m_beginJumpPoint.Distance);

        float h = Physics.GetHeightAtDistance(0, SegmentAngle, segmentDistance, m_speed);

        var position = m_beginJumpPoint.Position + m_beginJumpPoint.Direction * segmentDistance;
        position.y = m_beginJumpPoint.Position.y + h;

        if (float.IsNaN(position.x))
        {
            int d = 0;
        }
        transform.position = position;

        Vector3 lookVector = m_endJumpPoint.Position - transform.position;
        lookVector.Normalize();

        // transform.LookAt(Vector3.Lerp(m_endJumpPoint.Position, m_endendJumpPoint.Position, SegmentTime / SegmentTotalTime));

        Vector3 currentLookTarget = transform.position + transform.forward * m_beginJumpPoint.Distance;
        Vector3 newLookTarget = Vector3.SmoothDamp(currentLookTarget, m_lookTarget, ref m_lookTargetVelocity, 0.95f);

        transform.LookAt(newLookTarget);
    }
}
