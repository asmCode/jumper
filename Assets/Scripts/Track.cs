using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track
{
    private List<JumpPoint> m_jumpPoints = new List<JumpPoint>();

    public void AddJumpPoint(JumpPoint jumpPoint)
    {
        m_jumpPoints.Add(jumpPoint);
    }

    public JumpPoint GetJumpPoint(int index)
    {
        if (index >= m_jumpPoints.Count)
            return null;

        return m_jumpPoints[index];
    }

    public int GetJumpPointCount()
    {
        return m_jumpPoints.Count;
    }
}
