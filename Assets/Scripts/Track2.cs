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
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyWindowChanged -= OnHierarchyChanged;
    }

    private void OnHierarchyChanged()
    {
        UpdatePlatformConnections();
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

            child.m_prevPlatform = childPrev;
            child.m_nextPlatform = childNext;
        }
    }
}
