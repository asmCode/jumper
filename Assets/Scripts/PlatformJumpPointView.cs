using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformJumpPointView : JumpPointView
{
	public JumpPointView m_nextPlatform;
    public JumpPointView m_prevPlatform;
    public float m_jumpSpeed = 8.0f;
	public float m_jumpAngle = Mathf.PI / 4;

    public override void Update()
    {
        base.Update();

        if (m_nextPlatform != null)
        {
            transform.LookAt(m_nextPlatform.transform);
        }
    }

    public override Vector3 GetDirection()
	{
        if (m_nextPlatform == null)
            return Vector3.forward;

		return (m_nextPlatform.Position - Position).normalized;
	}

	public override float GetJumpSpeed()
	{
		return m_jumpSpeed;
	}

	public override float GetJumpAngle()
	{
		return m_jumpAngle;
	}
}
