using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Triangulator))]
public class TriangulatedMeshDisplay : MonoBehaviour
{
    [SerializeField]
    private Material MeshMaterial;
    [SerializeField]
    private DisplayPreset DisplayPreset;

    private MeshFilter TriangulatedMeshFilter;
    private MeshRenderer TriangulatedMeshRenderer;
    private Triangulator Triangulator;

    public DisplayPreset displayPreset { get { return DisplayPreset; } }

    #if UNITY_EDITOR
    private void OnValidate()
	{
		if(Triangulator == null){
			Triangulator = GetComponent<Triangulator>();
        }
        if(DisplayPreset != null){
            DisplayPreset.OnValuesUpdated += SetMaterialProperties;
        }
        EditorApplication.update += NotifyOfUpdatedValues;
    }

    public void NotifyOfUpdatedValues()
    {
        EditorApplication.update -= NotifyOfUpdatedValues;
        if (this == null || Application.isPlaying){
            return;
        }

        InitializeDisplay();
        UpdateMesh(Triangulator.Triangulate());
    }
    #endif

    private void Awake()
    {
        InitializeDisplay();
        Triangulator = GetComponent<Triangulator>();
        Triangulator.OnTriangulated += UpdateMesh;
    }

    private void SetMaterialProperties()
    {
        if (DisplayPreset == null){
            return;
        }

        MeshMaterial.SetColor("_ColourA", DisplayPreset.bgColourA);
        MeshMaterial.SetColor("_ColourB", DisplayPreset.bgColourB);
        MeshMaterial.SetFloat("_GradientOffset", DisplayPreset.gradientOffset);
        MeshMaterial.SetFloat("_GradientStretch", DisplayPreset.gradientStretch);

        MeshMaterial.SetFloat("_WireThickness", DisplayPreset.edgeThickness);
        MeshMaterial.SetColor("_WireColor", DisplayPreset.edgeColour);
        if (DisplayPreset.drawEdges){
            MeshMaterial.EnableKeyword("DRAW_WIREFRAME");
        }
        else{
            MeshMaterial.DisableKeyword("DRAW_WIREFRAME");
        }
    }

    public void InitializeDisplay()
    {
        SetMaterialProperties();

        if (TriangulatedMeshFilter == null || TriangulatedMeshRenderer == null)
        {
            GameObject parent = GameObject.Find("Triangulated Mesh");
            if (parent == null)
            {
                parent = new GameObject("Triangulated Mesh");
                parent.transform.SetParent(transform);
                parent.AddComponent<MeshFilter>();
                parent.AddComponent<MeshRenderer>();
            }

            TriangulatedMeshFilter = parent.GetComponent<MeshFilter>();
            TriangulatedMeshRenderer = parent.GetComponent<MeshRenderer>();
            TriangulatedMeshRenderer.material = MeshMaterial;
        }
    }

    public void UpdateMesh(List<Triangle> triangulation)
    {
        if(triangulation == null || triangulation.Count == 0){
            return;
        }

        //Compute and display triangulated mesh
		TriangulatedMeshFilter.sharedMesh = DelaunayHelper.CreateMeshFromTriangulation(triangulation);
    }
}