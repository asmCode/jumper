using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics
{
    public static float G = 9.8f;

    public static float GetAngle(float speed, float distance, float heightDifference)
    {
        float v2 = speed * speed;
        float d = distance;
        float y = heightDifference;

        float sq = v2 * v2 - G * (G * d * d + 2 * y * v2);
        if (sq < 0.0f)
            return 0.0f;

        return Mathf.Atan((v2 - Mathf.Sqrt(sq)) / (G * d));
    }

    public static float GetHeightAtDistance(float startY, float a, float d, float v)
    {
        float v_cos_a = v * Mathf.Cos(a);

        return
            startY +
            d * Mathf.Tan(a) -
            ((G * d * d)) / (2 * v_cos_a * v_cos_a);
    }

    public static Vector3 GetPositionAtDistance(Vector3 start, Vector3 end, float a, float d, float v)
    {
        float height = GetHeightAtDistance(0, a, d, v);

        var direction2d = end - start;
        direction2d.y = 0.0f;
        direction2d.Normalize();

        var position = start + direction2d * d;
        position.y += height;

        return position;
    }

    public static float GetTotalTime(float distance, float speed, float angle)
    {
        return distance / (speed * Mathf.Cos(angle));
    }

    public static float GetHeightAtTime(float time, float distance, float height, float speed, out float segmentDistance)
    {
        float angle = GetAngle(speed, distance, height);
        float totalTime = GetTotalTime(distance, speed, angle);

        segmentDistance = (time / totalTime) * distance;

        return GetHeightAtDistance(0, angle, segmentDistance, speed);
    }

    public static float GetDistanceAtTime(float time, float totalTime, float totalDistance)
    {
        return (time / totalTime) * totalDistance;
    }

    private static float GetRequiredSpeedInternal(float targetAngle, float distance, float heightDifference, float startSpeed, float speedStep)
    {
        if (speedStep < 0.01f)
            return startSpeed;

        float speed = startSpeed;
        while (true)
        {
            speed += speedStep;

            float angle = GetAngle(speed, distance, heightDifference);
            if (angle > targetAngle)
                return GetRequiredSpeedInternal(targetAngle, distance, heightDifference, speed - speedStep, speedStep / 2.0f);
        }
    }

    public static float GetRequiredSpeed(float angle, float distance, float heightDifference)
    {
        // return GetRequiredSpeedInternal(angle, distance, heightDifference, 0.0f, 10.0f);

        float x = distance;
        float y = heightDifference;
        float tan = Mathf.Tan(angle);
        float cos = Mathf.Cos(angle);

        float top = x * x * G;
        float bottom = (2 * x * tan - 2 * y) * cos * cos;
        return Mathf.Sqrt(top / bottom);
    }
}
