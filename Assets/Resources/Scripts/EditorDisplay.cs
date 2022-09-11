using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Triangulator))]
public class EditorDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject PointPrefab;
    [SerializeField]
    private DisplayPreset DisplayPreset;

    private Triangulator Triangulator;
    private Transform DisplayParent;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (Triangulator == null){
            Triangulator = GetComponent<Triangulator>();
        }
        if (DisplayPreset != null){
            DisplayPreset.OnValuesUpdated += NotifyOfUpdatedValues;
        }
        EditorApplication.update += NotifyOfUpdatedValues;
    }

    public void NotifyOfUpdatedValues()
    {
        EditorApplication.update -= NotifyOfUpdatedValues;
        if (this == null || Application.isPlaying){
            return;
        }
        UpdateDisplay(Triangulator.Triangulate());
    }
    #endif

    public void UpdateDisplay(List<Triangle> triangulation)
    {
        if (DisplayParent == null)
        {
            GameObject parent = GameObject.Find("PointPreview");
            if (parent == null)
            {
                DisplayParent = new GameObject("PointPreview").transform;
                DisplayParent.SetParent(transform);
            }
            else{
                DisplayParent = parent.transform;
            }
        }

        for (int i = DisplayParent.childCount - 1; i >= 0; i--){
            DestroyImmediate(DisplayParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < triangulation.Count; i++)
        {
            Triangle triangle = triangulation[i];
            for (int j = 0; j < triangle.vertices.Length; j++)
            {
                Point vertex = triangle.vertices[j];

                //Display points
                GameObject pointInstance = Instantiate(PointPrefab, DisplayParent);
                pointInstance.GetComponent<SpriteRenderer>().color = DisplayPreset.pointColour;
                pointInstance.transform.localScale = Vector3.one * DisplayPreset.minMaxPointScale.x;
                pointInstance.transform.position = new Vector3(vertex.pos.x, vertex.pos.y, 0f);

                //Display edges
                LineRenderer lineRenderer = pointInstance.GetComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                lineRenderer.sortingOrder = -1;
                lineRenderer.startWidth = DisplayPreset.edgeThickness;
                lineRenderer.endWidth = DisplayPreset.edgeThickness;
                lineRenderer.startColor = DisplayPreset.edgeColour;
                lineRenderer.endColor = DisplayPreset.edgeColour;

                Point lineTarget;
                if(vertex == triangle.vertA){
                    lineTarget = triangle.vertB;
                }
                else if (vertex == triangle.vertB){
                    lineTarget = triangle.vertC;
                }
                else{
                    lineTarget = triangle.vertA;
                }

                lineRenderer.SetPosition(0, vertex.pos);
                lineRenderer.SetPosition(1, lineTarget.pos);
            }
        }
    }
}