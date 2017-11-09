using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class PlatformJumpPointView : JumpPointView
{
    public JumpPointView NextPlatform { get; set; }
    public JumpPointView PrevPlatform { get; set; }
    public float m_jumpSpeed = 8.0f;
    public float m_jumpAngle = 45.0f;
    public Transform m_attachTo;

    public UnityEvent OnJump;

    public override void Update()
    {
        //if (m_nextPlatform != null)
        //{
        //    transform.LookAt(m_nextPlatform.transform);
        //}
        
        if (m_attachTo != null)
        {
        }
    }

    public override Vector3 GetDirection()
	{
        if (NextPlatform == null)
            return Vector3.forward;

		return (NextPlatform.NativePosition - NativePosition).normalized;
	}

	public override float GetJumpSpeed()
	{
		return m_jumpSpeed;
	}

	public override float GetJumpAngle()
	{
        return m_jumpAngle * Mathf.Deg2Rad;
    }

    public void NotifyJump()
    {
        OnJump.Invoke();
    }
}
