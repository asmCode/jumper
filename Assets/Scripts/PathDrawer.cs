using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public JumpPointView m_firstJumpPoint;
    public Transform m_track;

    void OnDrawGizmos()
    {
        DrawTrack();
        //return;

        List<int> visitedNodes = new List<int>();

        if (m_firstJumpPoint == null || m_firstJumpPoint.m_nextJumpPoint == null)
            return;

        var startPoint = m_firstJumpPoint;
        var endPoint = startPoint.m_nextJumpPoint;

        while (endPoint != null && !visitedNodes.Contains(startPoint.GetHashCode()))
        {
            visitedNodes.Add(startPoint.GetHashCode());

            Gizmos.DrawLine(
                startPoint.Position,
                endPoint.Position);

            DrawJumpTrajectory(
                startPoint.Position,
                startPoint.GetDirection2D(),
                startPoint.GetJumpAngle(),
                startPoint.GetJumpSpeed(),
                Color.green,
                10.0f);

            startPoint = endPoint;
            endPoint = endPoint.m_nextJumpPoint;

            // Vector3 direction = m_platforms[i + 1].transform.position - m_platforms[i].transform.position;
            // direction.y = 0;
            // direction.Normalize();

            // Vector3 axis = Vector3.Cross(Vector3.up, direction);
            // var q = Quaternion.AngleAxis(-45.0f, axis);
            // direction = q * direction;

            // DrawJumpTrajectory(
            //     m_platforms[i].transform.position,
            //     direction,
            //     m_airJumpSpeeed,
            //     Color.green);
        }
    }

    private void DrawTrack()
    {
        if (m_track == null || m_track.childCount < 2)
            return;

        for (int i = 0; i < m_track.childCount - 1; i++)
        {
            var child1 = m_track.GetChild(i);
            var platform1 = child1.GetComponent<PlatformJumpPointView>();

            var child2 = m_track.GetChild(i + 1);
            var platform2 = child2.GetComponent<PlatformJumpPointView>();

            Vector3 jumpPoint;
            float jumpTraDist = JumpResolver.GetOptimalJumpTrajectoryDistance(
                platform1.Position,
                platform1.GetJumpSpeed(),
                platform1.GetJumpAngle(),
                8.0f,
                Mathf.PI / 4,
                platform2.Position,
                out jumpPoint);

            DrawJumpTrajectory(
                platform1.Position,
                platform1.GetDirection2D(),
                platform1.GetJumpAngle(),
                platform1.GetJumpSpeed(),
                Color.green,
                jumpTraDist);

            var targetDist = platform2.Position - jumpPoint;
            targetDist.y = 0.0f;

            DrawJumpTrajectory(
                jumpPoint,
                platform1.GetDirection2D(),
                Mathf.PI / 4,
                8.0f,
                Color.yellow,
                targetDist.magnitude);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(jumpPoint, 0.2f);
        }
    }

    private void DrawJumpTrajectory(
        Vector3 position,
        Vector3 direction2D,
        float angle,
        float speed,
        Color color,
        float maxDistance)
    {
        // Vector3 direction2D = direction;
        // direction2D.y = 0.0f;
        // direction2D.Normalize();
        // float angle = Vector3.Angle(direction2D, direction) * Mathf.Deg2Rad;

        int segments = 20;
        float distancePerSegment = maxDistance / segments;

        float prevDistance = 0.0f;
        float distance = 0.0f;

        for (int j = 0; j < segments; j++)
        {
            prevDistance = distance;
            distance += distancePerSegment;

            //float prevHeight = position.y + Physics.GetHeightAtDistance(0, angle, prevDistance, speed);
            //float height = position.y + Physics.GetHeightAtDistance(0, angle, distance, speed);
            float prevHeight = Physics.GetHeightAtDistance(0, angle, prevDistance, speed);
            float height = Physics.GetHeightAtDistance(0, angle, distance, speed);

            Vector3 startLine = position + direction2D * prevDistance;
            Vector3 endLine = position + direction2D * distance;

            startLine.y += prevHeight;
            endLine.y += height;

            Debug.DrawLine(startLine, endLine, color);

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere()
        }
    }
}
