using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPointView : MonoBehaviour
{
	public Platform m_nextPlatfrom;

	public Vector3 GetDirection()
	{
		return (m_nextPlatfrom.transform.position - transform.position).normalized;
	}

	public Vector3 GetDirection2D()
	{
		var direction = GetDirection();
		direction.y = 0;
		return direction.normalized;
	}
}
