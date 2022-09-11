using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Triangulator : MonoBehaviour
{
    ///Implementation based on the Bowyer-Watson delaunay triangulation algorithm.
    ///https://www.newcastle.edu.au/__data/assets/pdf_file/0018/22482/07_An-implementation-of-Watsons-algorithm-for-computing-two-dimensional-Delaunay-triangulations.pdf

    [SerializeField]
    private List<Point> Points = new List<Point>();
    [SerializeField]
    private bool AlwaysRefresh = true;

    [SerializeField]
    private bool DebugVerticies = true;
    [SerializeField]
    private bool DebugEdges = true;
    [SerializeField]
    private bool DebugCircumCircles = true;
    [SerializeField]
    private bool DebugCircumCentres = true;
    [SerializeField]
    private bool DebugSupraTriangle = true;
    [SerializeField]
    private bool DebugBounds = true;

    [SerializeField]
    private float DebugScale = 0.2f;
    [SerializeField]
    private Color PointDebugColour = Color.red;
    [SerializeField]
    private Color EdgeDebugColour = Color.yellow;
    [SerializeField]
    private Color CircumDebugColour = Color.green;

    private List<Triangle> Triangulation;

    public List<Point> points { get { return Points; } }

    public TriangulateEvent OnTriangulated;
    public delegate void TriangulateEvent(List<Triangle> triangulation);

    ///Editor only
    #if UNITY_EDITOR
    [HideInInspector]
    public bool ShowPointData;
    [HideInInspector]
    public bool ShowDebugData;
    #endif

    #if UNITY_EDITOR
    private void OnValidate(){
        EditorApplication.update += NotifyOfUpdatedValues;
    }

    private void NotifyOfUpdatedValues()
    {
        EditorApplication.update -= NotifyOfUpdatedValues;
        if(this == null || Application.isPlaying){
            return;
        }

        if(AlwaysRefresh){
            Triangulate();
        }

        TriangulatedMeshDisplay meshDisplay = GetComponent<TriangulatedMeshDisplay>();
        if(meshDisplay != null){
            meshDisplay.NotifyOfUpdatedValues();
        }
        EditorDisplay editorDisplay = GetComponent<EditorDisplay>();
        if (editorDisplay != null){
            editorDisplay.NotifyOfUpdatedValues();
        }
    }
    #endif

    public void AddPoint(Vector2 position){
        Points.Add(new Point(position.x, position.y));
    }

    public void SetPoints(List<Point> points){
        Points = points;
    }

    public void ClearPoints(){
        Points.Clear();
    }

    private void Update()
    {
        if (AlwaysRefresh){
            Triangulate();
        }
    }

    public List<Triangle> Triangulate()
    {
        if(Points.Count == 0){
            return null;
        }

        Triangulation = DelaunayHelper.Delaun(Points);
        OnTriangulated?.Invoke(Triangulation);
        return Triangulation;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos(){
        DebugTriangulationGizmos();
    }

    private void DebugTriangulationGizmos()
    {
        if(Points.Count == 0){
            return;
        }

        if (DebugVerticies)
        {
            for (int i = 0; i < Points.Count; i++)
            {
                Handles.color = PointDebugColour;
                Handles.DrawSolidDisc(Points[i].pos, Vector3.forward, DebugScale);
            }
        }

        if(Triangulation == null){
            return;
        }

        for (int i = 0; i < Triangulation.Count; i++)
        {
            Triangle triangle = Triangulation[i];
            if (triangle.vertices == null){
                continue;
            }

            if(DebugEdges)
            {
                Gizmos.color = EdgeDebugColour;
                for (int j = 0; j < triangle.vertices.Length - 1; j++){
                    Gizmos.DrawLine(triangle.vertices[j].pos, triangle.vertices[j + 1].pos);
                }
                Gizmos.DrawLine(triangle.vertices[triangle.vertices.Length - 1].pos, triangle.vertices[0].pos);
            }

            if (DebugCircumCircles)
            {
                Handles.color = CircumDebugColour;
                Handles.DrawWireDisc(triangle.circumCentre, Vector3.forward, triangle.circumRadius);
            }

            if (DebugCircumCentres)
            {
                Handles.color = CircumDebugColour;
                Handles.DrawSolidDisc(triangle.circumCentre, Vector3.forward, DebugScale);
            }
        }

        if (DebugSupraTriangle)
        {
            Gizmos.color = EdgeDebugColour;
            PointBounds bounds = DelaunayHelper.GetPointBounds(Points);
            Triangle supraTriangle = DelaunayHelper.GenerateSupraTriangle(bounds);
            for (int i = 0; i < supraTriangle.vertices.Length - 1; i++){
                Gizmos.DrawLine(supraTriangle.vertices[i].pos, supraTriangle.vertices[i + 1].pos);
            }
            Gizmos.DrawLine(supraTriangle.vertices[supraTriangle.vertices.Length - 1].pos, supraTriangle.vertices[0].pos);
        }

        if (DebugBounds)
        {
            Gizmos.color = Color.cyan;
            PointBounds bounds = DelaunayHelper.GetPointBounds(Points);
            Gizmos.DrawLine(bounds.bottomLeft, bounds.bottomRight);
            Gizmos.DrawLine(bounds.bottomRight, bounds.topRight);
            Gizmos.DrawLine(bounds.topRight, bounds.topLeft);
            Gizmos.DrawLine(bounds.topLeft, bounds.bottomLeft);
        }
    }
    #endif
}