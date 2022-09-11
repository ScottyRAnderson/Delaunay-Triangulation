using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBounds
{
	public float MinX;
	public float MinY;
	public float MaxX;
	public float MaxY;

	public float minX { get { return MinX; } }
	public float minY { get { return MinY; } }
    public float maxX { get { return MaxX; } }
    public float maxY { get { return MaxY; } }

    public Vector2 bottomLeft { get { return new Vector2(minX, minY); } }
    public Vector2 bottomRight { get { return new Vector2(minX, maxY); } }
    public Vector2 topLeft { get { return new Vector2(maxX, minY); } }
    public Vector2 topRight { get { return new Vector2(maxX, maxY); } }

	public PointBounds(float minX, float minY, float maxX, float maxY)
	{
		MinX = minX;
		MinY = minY;
		MaxX = maxX;
		MaxY = maxY;
	}
}