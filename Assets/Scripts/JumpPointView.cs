using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class JumpPointView : MonoBehaviour
{
    public float AirJumpOnDistance { get; set; }

    private Vector3 m_nativePosition;

    public Vector3 Position
	{
		get { return transform.position; }
	}

    public Vector3 NativePosition
    {
        get { return m_nativePosition; }
    }

    private void Start()
    {
        m_nativePosition = transform.position;
    }

    public virtual Vector3 GetDirection()
	{
        return Vector3.forward;
	}

    public virtual void Update() { }

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
