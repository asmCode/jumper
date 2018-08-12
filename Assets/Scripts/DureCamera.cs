using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DureCamera : MonoBehaviour
{
    private Animator m_animator;
    
    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void PlayAirJumpAnimation()
    {
        // m_animator.Play("AirJump", 0, 0);
    }

    public void PlayPlatformJumpAnimation()
    {
        // m_animator.Play("PlatformJump", 0, 0);
    }
}
