using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track
{
    private List<Platform> m_platforms = new List<Platform>();

    public void AddPlatform(Platform platform)
    {
        m_platforms.Add(platform);
    }

    public Platform GetPlatform(int index)
    {
        if (index >= m_platforms.Count)
            return null;

        return m_platforms[index];
    }
}
