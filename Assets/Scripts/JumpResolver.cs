using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpResolver
{
    private const float SampleStep = 0.1f;

    public static float GetClosestDistance(
        Vector3 jumpPosition,
        float speed,
        float angle,
        Vector3 target,
        out float trajectoryDistance)
    {
        trajectoryDistance = float.MaxValue;

        float minDistance = float.MaxValue;
        float traDistance = 0.0f;

        for (traDistance = 0.0f; traDistance < 20.0f; traDistance += SampleStep)
        {
            var point = Physics.GetPositionAtDistance(jumpPosition, target, angle, traDistance, speed);
            var targetDistance = Vector3.Distance(point, target);

            if (targetDistance < minDistance)
            {
                minDistance = targetDistance;
                trajectoryDistance = traDistance;
            }
            else
                break;
        }

        return minDistance;
    }

    public static float GetOptimalJumpTrajectoryDistance(
        Vector3 platformPosition,
        float platformSpeed,
        float platformAngle,
        float jumpSpeed,
        float jumpAngle,
        Vector3 target,
        out Vector3 optimalJumpPosition)
    {
        optimalJumpPosition = Vector3.zero;

        float dummy;
        float minDistance = float.MaxValue;
        float trajectoryDistance = 0.0f;

        for (float traDistance = 0.0f; traDistance < 20.0f; traDistance += SampleStep)
        {
            var jumpPosition = Physics.GetPositionAtDistance(platformPosition, target, platformAngle, traDistance, platformSpeed);

            float targetDistance = GetClosestDistance(jumpPosition, jumpSpeed, jumpAngle, target, out dummy);

            if (targetDistance < minDistance)
            {
                minDistance = targetDistance;
                trajectoryDistance = traDistance;
                optimalJumpPosition = jumpPosition;
            }
            else
                break;
        }

        return trajectoryDistance;
    }

}
