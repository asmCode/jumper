using UnityEngine;

public class Jumping : JumperState
{
    public Jumping(Dude dude) : base(dude)
    {
    }

    public override void Enter()
    {

    }

    public override void Leave()
    {

    }

    public override void Update(float time)
    {
        m_dude.UpdateJump();

        if (m_dude.SegmentTime >= m_dude.SegmentTotalTime)
            m_dude.SetState(new Landed(m_dude));
    }

    public override void Jump()
    {
        m_dude.JumpToNextSegment();
    }
}
