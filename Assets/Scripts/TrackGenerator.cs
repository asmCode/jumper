using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator
{
    public Track Generate(Platform platformPrefab)
    {
        Track track = new Track();

        var position = new Vector3(0, 0, 0);

        for (int i = 0; i < 10; i++)
        {
            var platform = GeneratePlatform(platformPrefab, position);
            track.AddPlatform(platform);

            position.z += 10.0f;
        }

        return track;
    }

    private Platform GeneratePlatform(Platform platformPrefab, Vector3 position)
    {
        var platform = GameObject.Instantiate(platformPrefab, position, Quaternion.identity);

        return platform;
    }
}
