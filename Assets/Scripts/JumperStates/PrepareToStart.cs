using UnityEngine;

public class PrepareToStart : JumperState
{
    public PrepareToStart(Dude dude) : base(dude)
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

    }

    public override void Jump()
    {
        m_dude.SetState(new Jumping(m_dude));
        m_dude.JumpToNextSegment();
        m_dude.StartRun();
    }
}
