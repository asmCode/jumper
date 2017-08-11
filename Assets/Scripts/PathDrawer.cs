using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public float m_airJumpSpeeed = 12.0f;
    public Platform[] m_platforms;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);

        for (int i = 0; i < m_platforms.Length - 1; i++)
        {
            Gizmos.DrawLine(
                m_platforms[i].transform.position,
                m_platforms[i + 1].transform.position);

            DrawJumpTrajectory(
                m_platforms[i].transform.position,
                new Vector3(0, 1, 1).normalized,
                m_airJumpSpeeed);
        }
    }

    private void DrawJumpTrajectory(Vector3 position, Vector3 direction, float speed)
    {
        Vector3 direction2D = direction;
        direction2D.y = 0.0f;
        direction2D.Normalize();
        float angle = Vector3.Angle(direction2D, direction) * Mathf.Deg2Rad;

        float drawDistance = 20.0f;

        int segments = 20;
        float distancePerSegment = drawDistance / segments;

        float prevDistance = 0.0f;
        float distance = 0.0f;

        for (int j = 0; j < segments; j++)
        {
            prevDistance = distance;
            distance += distancePerSegment;

            float prevHeight = position.y + Physics.GetHeightAtDistance(0, angle, prevDistance, speed);
            float height = position.y + Physics.GetHeightAtDistance(0, angle, distance, speed);

            Vector3 startLine = position + direction2D * prevDistance;
            Vector3 endLine = position + direction2D * distance;

            startLine.y = prevHeight;
            endLine.y = height;

            Debug.DrawLine(startLine, endLine, Color.red);
        }
    }
}
