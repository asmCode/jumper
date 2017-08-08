using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator
{
    public Track Generate(Platform platformPrefab)
    {
        var jumpPointPosition = new Vector3(0, 0, 0);

        List<Vector3> jumpPointPositions = new List<Vector3>();

        int pointCount = 20;

        Random.InitState(10);

        for (int i = 0; i < pointCount; i++)
        {
            jumpPointPositions.Add(jumpPointPosition);

            //jumpPointPosition.x += 2 * i * ((i % 2 == 0) ? -1 : 1);
            //jumpPointPosition.z += 20.0f;
            //jumpPointPosition.y += 5.0f;

            jumpPointPosition.x += Random.Range(-5.0f, 5.0f);
            jumpPointPosition.z += Random.Range(20.0f, 40.0f);
            jumpPointPosition.y += Random.Range(-5.0f, 5.0f);
        }

        Track track = new Track();

        for (int i = 0; i < pointCount; i++)
        {
            Vector3? prevPosition = null;
            if (i > 0)
                prevPosition = jumpPointPositions[i - 1];

            Vector3 position = jumpPointPositions[i];

            Vector3? nextPosition = null;
            if (i < pointCount - 1)
                nextPosition = jumpPointPositions[i + 1];

            var jumpPoint = new JumpPoint(position, prevPosition, nextPosition);
            track.AddJumpPoint(jumpPoint);
        }

        CreateVisualTrack(track, platformPrefab);

        return track;
    }

    private Platform GeneratePlatform(Platform platformPrefab, Vector3 position)
    {
        var platform = GameObject.Instantiate(platformPrefab, position, Quaternion.identity);

        return platform;
    }

    private void CreateVisualTrack(Track track, Platform platformPrefab)
    {
        for (int i = 0; i < track.GetJumpPointCount(); i++)
        {
            GeneratePlatform(platformPrefab, track.GetJumpPoint(i).Position);
        }
    }
}
