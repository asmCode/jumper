using UnityEngine;

public class JumperState
{
    protected Dude m_dude;

    public JumperState(Dude dude)
    {
        m_dude = dude;
    }

    public virtual void Enter()
    {

    }

    public virtual void Leave()
    {

    }

    public virtual void Update(float time)
    {

    }

    public virtual void Jump()
    {

    }
}
