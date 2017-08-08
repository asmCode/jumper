using UnityEngine;

public class JumpPoint
{
    public JumpPoint(Vector3 position, Vector3? prevPosition, Vector3? nextPosition)
    {
        Position = position;

        var target = nextPosition.GetValueOrDefault(Vector3.forward) - position;
        Direction = target.normalized;
        Distance = target.magnitude;
        HeightDifference = target.y;
    }

    public JumpPoint(Vector3 position, Vector3 direction, float distance, float heightDifference)
    {
        Position = position;
        Direction = direction;
        Distance = distance;
        HeightDifference = heightDifference;
    }

    public Vector3 Position
    {
        get;
        set;
    }

    public Vector3 Direction
    {
        get;
        set;
    }

    public float Distance
    {
        get;
        set;
    }

    public float HeightDifference
    {
        get;
        set;
    }
}
