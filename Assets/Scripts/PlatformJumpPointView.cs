using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformJumpPointView : JumpPointView
{
	public JumpPointView m_nextPlatform;
	public float m_airJumpOnDistance;
	public float m_jumpSpeed = 8.0f;
	public float m_jumpAngle = Mathf.PI / 4;

	public void Update()
	{
		if (m_nextJumpPoint != null)
		{
			float height = Physics.GetHeightAtDistance(0, GetJumpAngle(), m_airJumpOnDistance, GetJumpSpeed());

			var position = Position;
			position += GetDirection2D() * m_airJumpOnDistance;
			position.y += height;
			m_nextJumpPoint.transform.position = position;
		}
	}

	public override Vector3 GetDirection()
	{
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
