//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(JumpPointView))]
//public class JumpPointEditor : Editor
//{
//    SerializedProperty m_nextJumpPoint;

//    void OnEnable()
//    {
//        m_nextJumpPoint = serializedObject.FindProperty("m_nextJumpPoint");
//    }

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        EditorGUILayout.PropertyField(m_nextJumpPoint);
//        serializedObject.ApplyModifiedProperties();

//        //ObjectBuilderScript myScript = (ObjectBuilderScript)target;
//        if (GUILayout.Button("Create Platform"))
//        {
//            CreatePlatform();
//        }
//    }

//    private void CreatePlatform()
//    {
//        var jumpPoint = target as JumpPointView;

//        var gameObject = new GameObject();

//        gameObject.AddComponent<PlatformJumpPointView>();
//        gameObject.transform.SetParent(jumpPoint.transform.parent);
//    }
//}
