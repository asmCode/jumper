﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public JumpPointView m_firstJumpPoint;

    void OnDrawGizmos()
    {
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
                Color.green);

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

    private void DrawJumpTrajectory(Vector3 position, Vector3 direction2D, float angle, float speed, Color color)
    {
        // Vector3 direction2D = direction;
        // direction2D.y = 0.0f;
        // direction2D.Normalize();
        // float angle = Vector3.Angle(direction2D, direction) * Mathf.Deg2Rad;

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

            Debug.DrawLine(startLine, endLine, color);
        }
    }
}
