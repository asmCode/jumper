using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dude : MonoBehaviour
{
    private JumperState m_state;
    public Track m_track;

    private JumpPoint m_beginJumpPoint;
    private JumpPoint m_endJumpPoint;
    private JumpPoint m_endendJumpPoint;

    public int m_nextPlatformIndex = 0;

    private float m_speed = 21.415f;
    private float m_jumpSpeed = 21.415f;
    private float m_landingSpeed = 10.0f;
    private float m_landingAngle = 60.0f * Mathf.Deg2Rad;
    private Vector3 m_lookTarget;

    private Vector3 m_lookTargetVelocity;

    public float Speed
    {
        get { return m_speed; }
    }

    public float SegmentTotalTime
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
    }

    public void JumpToNextSegment()
    {
        m_speed = m_jumpSpeed;

        m_beginJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex);
        m_endJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex + 1);

        m_beginJumpPoint = new JumpPoint(transform.position, null, m_endJumpPoint.Position);

        if (m_nextPlatformIndex < m_track.GetJumpPointCount() - 2)
            m_endendJumpPoint = m_track.GetJumpPoint(m_nextPlatformIndex + 2);
        else
            m_endendJumpPoint = m_endJumpPoint;

        m_nextPlatformIndex++;

        SegmentTime = 0.0f;

        SegmentAngle = Physics.GetAngle(m_speed, m_beginJumpPoint.Distance, m_beginJumpPoint.HeightDifference);
        SegmentTotalTime = Physics.GetTotalTime(m_beginJumpPoint.Distance, m_speed, SegmentAngle);

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

        m_lookTarget = transform.position + transform.forward * 100.0f;
    }

    public void UpdateJump()
    {
        SegmentTime += Time.deltaTime;

        float segmentDistance = Physics.GetDistanceAtTime(SegmentTime, SegmentTotalTime, m_beginJumpPoint.Distance);

        float h = Physics.GetHeightAtDistance(0, SegmentAngle, segmentDistance, m_speed);

        var position = m_beginJumpPoint.Position + m_beginJumpPoint.Direction * segmentDistance;
        position.y = m_beginJumpPoint.Position.y + h;
        transform.position = position;

        Vector3 lookVector = m_endJumpPoint.Position - transform.position;
        lookVector.Normalize();

        // transform.LookAt(Vector3.Lerp(m_endJumpPoint.Position, m_endendJumpPoint.Position, SegmentTime / SegmentTotalTime));

        Vector3 currentLookTarget = transform.position + transform.forward * m_beginJumpPoint.Distance;
        Vector3 newLookTarget = Vector3.SmoothDamp(currentLookTarget, m_lookTarget, ref m_lookTargetVelocity, 0.95f);

        transform.LookAt(newLookTarget);
    }
}
