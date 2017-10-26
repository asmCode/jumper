using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class PathDrawer : MonoBehaviour
{
    public enum DrawMode
    {
        None,
        Selected,
        All
    }

    public DrawMode m_drawMode = DrawMode.All;

    private Transform m_track;

    void Update()
    {
        Draw();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        Selection.selectionChanged += HandleSelectionChanged;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        Selection.selectionChanged -= HandleSelectionChanged;
#endif
    }

    private void HandleSelectionChanged()
    {
        // Trick to force unity to refresh the view
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        //if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<PlatformJumpPointView>() != null)
        //{

        //    Selection.activeGameObject.SetActive(false);
        //    Selection.activeGameObject.SetActive(true);
        //}

        Draw();
    }

    private void Draw()
    {
        Init();

        if (m_drawMode == DrawMode.None)
            return;

#if UNITY_EDITOR
        if (m_drawMode == DrawMode.Selected && Selection.activeGameObject == null)
            return;
        else if (m_drawMode == DrawMode.Selected && Selection.activeGameObject != null)
            DrawTrack(Selection.activeGameObject);
        else
            DrawTrack(null);
#endif
    }

    private void Init()
    {
        if (m_track != null)
            return;

        m_track = transform;
    }

    private void DrawTrack(GameObject drawOnly)
    {
        if (m_track == null || m_track.childCount < 2)
            return;

        for (int i = 0; i < m_track.childCount - 1; i++)
        {
            var child1 = m_track.GetChild(i);
            var platform1 = child1.GetComponent<PlatformJumpPointView>();

            var child2 = m_track.GetChild(i + 1);
            var platform2 = child2.GetComponent<PlatformJumpPointView>();

            if (drawOnly != null && drawOnly != child1.gameObject && drawOnly != child2.gameObject)
                continue;

            Vector3 jumpPoint;
            float jumpTraDist = JumpResolver.GetOptimalJumpTrajectoryDistance(
                platform1.Position,
                platform1.GetJumpSpeed(),
                platform1.GetJumpAngle(),
                8.0f,
                Mathf.PI / 4,
                platform2.Position,
                out jumpPoint);

            DrawJumpTrajectory(
                platform1.Position,
                platform1.GetDirection2D(),
                platform1.GetJumpAngle(),
                platform1.GetJumpSpeed(),
                Color.green,
                jumpTraDist);

            var targetDist = platform2.Position - jumpPoint;
            targetDist.y = 0.0f;

            DrawJumpTrajectory(
                jumpPoint,
                platform1.GetDirection2D(),
                Mathf.PI / 4,
                8.0f,
                Color.yellow,
                targetDist.magnitude);

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere(jumpPoint, 0.2f);
        }
    }

    private void DrawJumpTrajectory(
        Vector3 position,
        Vector3 direction2D,
        float angle,
        float speed,
        Color color,
        float maxDistance)
    {
        // Vector3 direction2D = direction;
        // direction2D.y = 0.0f;
        // direction2D.Normalize();
        // float angle = Vector3.Angle(direction2D, direction) * Mathf.Deg2Rad;

        int segments = 20;
        float distancePerSegment = maxDistance / segments;

        float prevDistance = 0.0f;
        float distance = 0.0f;

        for (int j = 0; j < segments; j++)
        {
            prevDistance = distance;
            distance += distancePerSegment;

            //float prevHeight = position.y + Physics.GetHeightAtDistance(0, angle, prevDistance, speed);
            //float height = position.y + Physics.GetHeightAtDistance(0, angle, distance, speed);
            float prevHeight = Physics.GetHeightAtDistance(0, angle, prevDistance, speed);
            float height = Physics.GetHeightAtDistance(0, angle, distance, speed);

            Vector3 startLine = position + direction2D * prevDistance;
            Vector3 endLine = position + direction2D * distance;

            startLine.y += prevHeight;
            endLine.y += height;

            Debug.DrawLine(startLine, endLine, color);

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawSphere()
        }
    }
}
