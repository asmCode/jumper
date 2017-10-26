using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class Track2 : MonoBehaviour
{
    private void OnEnable()
    {
#if UNITY_EDITOR
		EditorApplication.hierarchyWindowChanged += OnHierarchyChanged;
#endif

        UpdatePlatforms();
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.hierarchyWindowChanged -= OnHierarchyChanged;
#endif
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
                child.NativePosition,
                child.GetJumpSpeed(),
                child.GetJumpAngle(),
                8.0f,
                Mathf.PI / 4.0f,
                childNext.NativePosition,
                out jumpPosition);
        }
    }
}