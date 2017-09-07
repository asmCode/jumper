using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track2 : MonoBehaviour
{
    public PlatformJumpPointView platform1;
    public PlatformJumpPointView platform2;

    private void Update()
    {
        float trajectoryDistance;
        float closestDistance = JumpResolver.GetClosestDistance(
            platform1.Position,
            platform1.GetJumpSpeed(),
            platform1.GetJumpAngle(),
            platform2.Position,
            out trajectoryDistance);

        // Debug.LogFormat("closestDistance = {0:0.00}, trajectoryDistance = {1:0.00}", closestDistance, trajectoryDistance);
    }
}
