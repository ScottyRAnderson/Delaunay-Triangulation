using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    [SerializeField]
    private float X;
    [SerializeField]
    private float Y;

    public float x { get { return X; } }
    public float y { get { return Y; } }
    public Vector2 pos { get { return new Vector2(X, Y); } }

    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Point(Point point)
    {
        X = point.X;
        Y = point.Y;
    }

    public void SetPosition(Vector2 pos)
    {
        X = pos.x;
        Y = pos.y;
    }

    public bool EqualsPoint(Point point){
        return X == point.X && Y == point.Y;
    }
}