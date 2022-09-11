using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Edge
{
    private Point VertexA;
	private Point VertexB;
	private Vector2 MidPoint;
	private float Gradient;

	public Point vertexA { get { return VertexA; } }
    public Point vertexB { get { return VertexB; } }
	public Vector2 midPoint { get { return MidPoint; } }
	public float gradient { get { return Gradient; } }

    public Edge(Point vertexA, Point vertexB)
	{
		VertexA = vertexA;
		VertexB = vertexB;
		MidPoint = ComputeMidPoint();
		Gradient = ComputeGradient();
    }

	public Vector2 ComputeMidPoint(){
		return MathHelper.MidPointOfLine(VertexA, VertexB);
	}

	public float ComputeGradient(){
		return MathHelper.GradientOfLine(VertexA, VertexB);
	}

	public bool EqualsEdge(Edge edge){
		return VertexA.pos == edge.VertexA.pos && VertexB.pos == edge.VertexB.pos;
    }
}