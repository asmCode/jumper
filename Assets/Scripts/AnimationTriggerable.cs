using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTriggerable : Triggerable
{
    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (m_animator == null)
            return;

        m_animator.Play("Default", 0, 0.0f);
        m_animator.speed = 0.0f;
    }

    public override void Trigger(float delay)
    {
        if (m_animator == null)
            return;

        Invoke("PlayDefaultAnimation", delay);
    }

    private void PlayDefaultAnimation()
    {
        m_animator.speed = 1.0f;
        m_animator.Play("Default", 0, 0.0f);
    }
}
