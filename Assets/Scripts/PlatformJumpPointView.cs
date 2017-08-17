using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlatformJumpPointView : JumpPointView
{
	public JumpPointView m_nextPlatform;
	public float m_airJumpOnDistance;

	public void Update()
	{
		if (m_nextJumpPoint != null)
		{
			float height = Physics.GetHeightAtDistance(0, m_nextJumpPoint.GetJumpAngle(), m_airJumpOnDistance, 8);

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
}
