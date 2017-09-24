using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformJumpPointView : JumpPointView
{
	public JumpPointView NextPlatform { get; set; }
    public JumpPointView PrevPlatform { get; set; }
    public float m_jumpSpeed = 8.0f;
    public float m_jumpAngle = 45.0f;

    public void Start()
    {
        m_jumpAngle *= Mathf.Rad2Deg;
    }

    public override void Update()
    {
        //if (m_nextPlatform != null)
        //{
        //    transform.LookAt(m_nextPlatform.transform);
        //}
    }

    public override Vector3 GetDirection()
	{
        if (NextPlatform == null)
            return Vector3.forward;

		return (NextPlatform.Position - Position).normalized;
	}

	public override float GetJumpSpeed()
	{
		return m_jumpSpeed;
	}

	public override float GetJumpAngle()
	{
        return m_jumpAngle * Mathf.Deg2Rad;
    }
}
