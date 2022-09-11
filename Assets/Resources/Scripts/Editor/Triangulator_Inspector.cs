using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Triangulator))]
public class Triangulator_Inspector : Editor
{
    private Triangulator TriangulatorBase;

    private SerializedProperty Points;
    private SerializedProperty AlwaysRefresh;

    private SerializedProperty DebugVerticies;
    private SerializedProperty DebugEdges;
    private SerializedProperty DebugCircumCircles;
    private SerializedProperty DebugCircumCentres;
    private SerializedProperty DebugSupraTriangle;
    private SerializedProperty DebugBounds;
    private SerializedProperty DebugScale;
    private SerializedProperty PointDebugColour;
    private SerializedProperty EdgeDebugColour;
    private SerializedProperty CircumDebugColour;

    private void OnEnable()
    {
        TriangulatorBase = target as Triangulator;

        Points = serializedObject.FindProperty("Points");
        AlwaysRefresh = serializedObject.FindProperty("AlwaysRefresh");

        DebugVerticies = serializedObject.FindProperty("DebugVerticies");
        DebugEdges = serializedObject.FindProperty("DebugEdges");
        DebugCircumCircles = serializedObject.FindProperty("DebugCircumCircles");
        DebugCircumCentres = serializedObject.FindProperty("DebugCircumCentres");
        DebugSupraTriangle = serializedObject.FindProperty("DebugSupraTriangle");
        DebugBounds = serializedObject.FindProperty("DebugBounds");
        DebugScale = serializedObject.FindProperty("DebugScale");
        PointDebugColour = serializedObject.FindProperty("PointDebugColour");
        EdgeDebugColour = serializedObject.FindProperty("EdgeDebugColour");
        CircumDebugColour = serializedObject.FindProperty("CircumDebugColour");

        Tools.hidden = true;
    }

    private void OnDisable(){
        Tools.hidden = false;
    }

    public override void OnInspectorGUI()
    {
        Undo.RecordObject(TriangulatorBase, "Triangulator Changed");
        serializedObject.Update();

        DrawPointData();
        GUILayout.Space(5f);
        DrawDebugData();

        serializedObject.ApplyModifiedProperties();
        if(GUI.changed){
            EditorUtility.SetDirty(TriangulatorBase);
        }
    }

    private void DrawPointData()
    {
        using (new GUILayout.VerticalScope(EditorHelper.GetColouredStyle(EditorHelper.GroupBoxCol)))
        {
            EditorGUI.indentLevel++;
            TriangulatorBase.ShowPointData = EditorHelper.Foldout(TriangulatorBase.ShowPointData, "Point Data");
            if (TriangulatorBase.ShowPointData)
            {
                EditorGUI.indentLevel++;
                if(AlwaysRefresh.boolValue){
                    GUI.enabled = false;
                }
                if (EditorHelper.GUILayoutButtonIndented("Triangulate", 24f)){
                    TriangulatorBase.Triangulate();
                }
                GUI.enabled = true;

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Space(24f);
                    if (GUILayout.Button("Clear Points"))
                    {
                        if (EditorUtility.DisplayDialog("Clear Points", "Confirm clear points", "Clear", "Cancel")){
                            TriangulatorBase.ClearPoints();
                        }
                    }

                    if (GUILayout.Button("Add Point")){
                        TriangulatorBase.AddPoint(Vector2.zero);
                    }
                }
                EditorGUILayout.PropertyField(Points);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
    }

    private void DrawDebugData()
    {
        using (new GUILayout.VerticalScope(EditorHelper.GetColouredStyle(EditorHelper.GroupBoxCol)))
        {
            EditorGUI.indentLevel++;
            TriangulatorBase.ShowDebugData = EditorHelper.Foldout(TriangulatorBase.ShowDebugData, "Debug Data");
            if (TriangulatorBase.ShowDebugData)
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.Slider(DebugScale, 0.1f, 5f);
                EditorGUILayout.PropertyField(PointDebugColour);
                EditorGUILayout.PropertyField(EdgeDebugColour);
                EditorGUILayout.PropertyField(CircumDebugColour);

                GUILayout.Space(5f);

                using (new GUILayout.HorizontalScope())
                {
                    using (new GUILayout.VerticalScope())
                    {
                        EditorGUILayout.PropertyField(DebugVerticies);
                        EditorGUILayout.PropertyField(DebugEdges);
                        EditorGUILayout.PropertyField(DebugCircumCircles);
                    }
                    using (new GUILayout.VerticalScope())
                    {
                        EditorGUILayout.PropertyField(DebugCircumCentres);
                        EditorGUILayout.PropertyField(DebugSupraTriangle);
                        EditorGUILayout.PropertyField(DebugBounds);
                    }
                }
                EditorGUILayout.PropertyField(AlwaysRefresh);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }
    }
}