using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPointView : MonoBehaviour
{
	public JumpPointView m_nextJumpPoint;

	public Vector3 Position
	{
		get { return transform.position; }
	}

	public virtual Vector3 GetDirection()
	{
		return (m_nextJumpPoint.Position - Position).normalized;
	}

	public virtual Vector3 GetDirection2D()
	{
		var direction = GetDirection();
		direction.y = 0;
		return direction.normalized;
	}

	public virtual float GetJumpAngle()
	{
		return Mathf.PI / 4.0f;
	}

	public virtual float GetJumpSpeed()
	{
		return 8.0f;
	}
}
