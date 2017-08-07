using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackDebugInfo
{
    public void DrawTrack(Track track, float speed)
    {
        for (int i = 0; i < track.GetJumpPointCount() - 1; i++)
        {
            var segmentBegin = track.GetJumpPoint(i);
            var segmentEnd = track.GetJumpPoint(i + 1);

            Debug.DrawLine(segmentBegin.Position, segmentEnd.Position, Color.blue);

            DrawJumpTrajectory(segmentBegin, segmentEnd, speed);
        }
    }

    private void DrawJumpTrajectory(JumpPoint jumpPointA, JumpPoint jumpPointB, float speed)
    {
        float distance = jumpPointA.Distance;
        float height = jumpPointA.HeightDifference;

        float angle = Physics.GetAngle(speed, distance, height);
        float totalTime = Physics.GetTotalTime(distance, speed, angle);

        int steps = 20;
        float timeStep = totalTime / steps;

        for (int j = 0; j < steps; j++)
        {
            float t = j * timeStep;
            float t2 = (j + 1.0f) * timeStep;

            float startSegmentDistance = Physics.GetDistanceAtTime(t, totalTime, distance);
            float endSegmentDistance = Physics.GetDistanceAtTime(t2, totalTime, distance);

            float startH = jumpPointA.Position.y + Physics.GetHeightAtDistance(0, angle, startSegmentDistance, speed);
            float endH = jumpPointA.Position.y + Physics.GetHeightAtDistance(0, angle, endSegmentDistance, speed);

            Vector3 startLine = jumpPointA.Position + jumpPointA.Direction * startSegmentDistance;
            Vector3 endLine = jumpPointA.Position + jumpPointA.Direction * endSegmentDistance;

            startLine.y = startH;
            endLine.y = endH;

            Debug.DrawLine(startLine, endLine, Color.red);
        }
    }
}
