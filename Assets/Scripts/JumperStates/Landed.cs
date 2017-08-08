using UnityEngine;
using UnityEngine.SceneManagement;

public class Landed : JumperState
{
    public Landed(Dude dude) : base(dude)
    {
    }

    public override void Enter()
    {
        m_dude.JumpAfterLanding();
    }

    public override void Leave()
    {

    }

    public override void Update(float time)
    {
        m_dude.UpdateJump();
    }

    public override void Jump()
    {
        if (m_dude.m_nextPlatformIndex == m_dude.m_track.GetJumpPointCount() - 1)
        {
            SceneManager.LoadScene("Gameplay");
            return;
        }

        m_dude.JumpToNextSegment();
        m_dude.SetState(new Jumping(m_dude));
    }
}
