using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PointFieldManager))]
public class PointFieldManager_Inspector : Editor
{
    private PointFieldManager ManagerBase;

    private SerializedProperty PointPrefab;
    private SerializedProperty FieldSizeX;
    private SerializedProperty FieldSizeY;
    private SerializedProperty NumPoints;

    private void OnEnable()
    {
        ManagerBase = target as PointFieldManager;

        PointPrefab = serializedObject.FindProperty("PointPrefab");
        FieldSizeX = serializedObject.FindProperty("FieldSizeX");
        FieldSizeY = serializedObject.FindProperty("FieldSizeY");
        NumPoints = serializedObject.FindProperty("NumPoints");
    }

    public override void OnInspectorGUI()
    {
        Undo.RecordObject(ManagerBase, "Point Field Manager Changed");
        serializedObject.Update();

        EditorGUILayout.PropertyField(PointPrefab);
        EditorGUILayout.PropertyField(FieldSizeX);
        EditorGUILayout.PropertyField(FieldSizeY);
        EditorGUILayout.PropertyField(NumPoints);

        if(GUILayout.Button("Build Point Field")){
            ManagerBase.BuildPointField();
        }

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed){
            EditorUtility.SetDirty(ManagerBase);
        }
    }
}