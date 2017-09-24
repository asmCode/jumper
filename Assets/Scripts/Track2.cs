#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Track2 : MonoBehaviour
{
    private void OnEnable()
    {
        EditorApplication.hierarchyWindowChanged += OnHierarchyChanged;

        UpdatePlatforms();
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyWindowChanged -= OnHierarchyChanged;
    }

    private void OnHierarchyChanged()
    {
        UpdatePlatforms();
    }

    private void UpdatePlatforms()
    {
        UpdatePlatformConnections();
        UpdatePlatformOptimalJumpDistances();
    }

    private void Update()
    {
    }

    private void UpdatePlatformConnections()
    {
        if (transform.childCount == 0)
            return;

        for (int i = 0; i < transform.childCount; i++)
        {
            var childPrev = i > 0 ? transform.GetChild(i - 1).GetComponent<PlatformJumpPointView>() : null;
            var child = transform.GetChild(i).GetComponent<PlatformJumpPointView>();
            var childNext = i < transform.childCount - 1 ? transform.GetChild(i + 1).GetComponent<PlatformJumpPointView>() : null;

            child.PrevPlatform = childPrev;
            child.NextPlatform = childNext;
        }
    }

    private void UpdatePlatformOptimalJumpDistances()
    {
        if (transform.childCount < 2)
            return;

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            var child = transform.GetChild(i).GetComponent<PlatformJumpPointView>();
            var childNext = transform.GetChild(i + 1).GetComponent<PlatformJumpPointView>();

            Vector3 jumpPosition;
            child.AirJumpOnDistance = JumpResolver.GetOptimalJumpTrajectoryDistance(
                child.Position,
                child.GetJumpSpeed(),
                child.GetJumpAngle(),
                8.0f,
                Mathf.PI / 4.0f,
                childNext.Position,
                out jumpPosition);
        }
    }
}

#endif