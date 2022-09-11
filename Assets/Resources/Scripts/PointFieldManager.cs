using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Triangulator))]
public class PointFieldManager : MonoBehaviour
{
    [SerializeField]
    private WanderingPoint PointPrefab;
	[SerializeField]
	private float FieldSizeX;
    [SerializeField]
    private float FieldSizeY;
    [SerializeField]
    private int NumPoints = 500;

    private List<WanderingPoint> PointField;
    private Transform FieldParent;
    private Triangulator Triangulator;
    private TriangulatedMeshDisplay MeshDisplay;

    private void OnValidate()
	{
		if(Triangulator == null){
			Triangulator = GetComponent<Triangulator>();
		}
        if(MeshDisplay == null){
            MeshDisplay = GetComponent<TriangulatedMeshDisplay>();
        }
        NumPoints = Mathf.Max(0, NumPoints);
    }

    private void Awake()
    {
        Triangulator = GetComponent<Triangulator>();
        MeshDisplay = GetComponent<TriangulatedMeshDisplay>();
        BuildPointField();
    }

    public void BuildPointField()
    {
        if(FieldParent == null)
        {
            GameObject parent = GameObject.Find("PointField");
            if(parent == null)
            {
                FieldParent = new GameObject("PointField").transform;
                FieldParent.SetParent(transform);
            }
            else{
                FieldParent = parent.transform;
            }
        }

        for (int i = FieldParent.childCount - 1; i >= 0; i--)
        {
            if(Application.isPlaying){
                Destroy(FieldParent.GetChild(i).gameObject);
            }
            else{
                DestroyImmediate(FieldParent.GetChild(i).gameObject);
            }
        }

        PointBounds bounds = new PointBounds(-FieldSizeX, -FieldSizeY, FieldSizeX, FieldSizeY);
        PointField = new List<WanderingPoint>();

        //Generate point field
        for (int i = 0; i < NumPoints; i++)
        {
            float x = Random.Range(-FieldSizeX, FieldSizeX);
            float y = Random.Range(-FieldSizeY, FieldSizeY);
            PointField.Add(CreatePointInstance(new Vector2(x, y)));
        }

        //Points for the corners of the bounds, these should never change
        PointField.Add(CreatePointInstance(bounds.bottomLeft, true));
        PointField.Add(CreatePointInstance(bounds.bottomRight, true));
        PointField.Add(CreatePointInstance(bounds.topRight, true));
        PointField.Add(CreatePointInstance(bounds.topLeft, true));

        List<Point> points = new List<Point>();
        for (int i = 0; i < PointField.Count; i++){
            points.Add(PointField[i].point);
        }

        Triangulator.SetPoints(points);
        Triangulator.Triangulate();

        #if UNITY_EDITOR
        MeshDisplay.NotifyOfUpdatedValues();
        #endif
    }

    public WanderingPoint CreatePointInstance(Vector2 pos, bool locked = false)
    {
        WanderingPoint wanderInstance = Instantiate(PointPrefab, FieldParent);
        wanderInstance.SpriteComponent.color = MeshDisplay.displayPreset.pointColour;
        wanderInstance.transform.localScale = Vector3.one * Random.Range(MeshDisplay.displayPreset.minMaxPointScale.x, MeshDisplay.displayPreset.minMaxPointScale.y);
        wanderInstance.InitializePoint(new Point(pos.x, pos.y), MeshDisplay.displayPreset.minMaxWanderSpeed, locked);
        return wanderInstance;
    }

    public void HandleBoundRestriction(WanderingPoint point)
    {
        //If the point 'wanders' outside the bounds, move the point to the opposite edge
        Vector2 pos = point.point.pos;
        bool pointInBounds = pos.x > -FieldSizeX && pos.x < FieldSizeX && pos.y > -FieldSizeY && pos.y < FieldSizeY;
        if(!pointInBounds)
        {
            float x = 0f;
            float y = 0f;
            if(pos.x < -FieldSizeX || pos.x > FieldSizeX)
            {
                x = pos.x < -FieldSizeX ? FieldSizeX : -FieldSizeX;
                y = Random.Range(-FieldSizeY, FieldSizeY);
            }
            else if(pos.y < -FieldSizeY || pos.y > FieldSizeY)
            {
                y = pos.y < -FieldSizeY ? FieldSizeY : -FieldSizeY;
                x = Random.Range(-FieldSizeX, FieldSizeX);
            }

            point.transform.position = new Vector3(x, y, 0f);
            point.SetDirection(pos);
        }
    }

	private void OnDrawGizmos()
	{
		PointBounds bounds = new PointBounds(-FieldSizeX, -FieldSizeY, FieldSizeX, FieldSizeY);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(bounds.bottomLeft, bounds.bottomRight);
        Gizmos.DrawLine(bounds.bottomRight, bounds.topRight);
        Gizmos.DrawLine(bounds.topRight, bounds.topLeft);
        Gizmos.DrawLine(bounds.topLeft, bounds.bottomLeft);
    }
}