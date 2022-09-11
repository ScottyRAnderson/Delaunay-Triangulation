using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper
{
    public static Vector2 MidPointOfLine(Point p1, Point p2){
        return new Vector2((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
    }

    public static float GradientOfLine(Point p1, Point p2){
        return (p2.y - p1.y) / (p2.x - p1.x);
    }

    public static float NegativeReciprocal(float value){
        return -(1 / value);
    }

    ///<summary> Returns the intersection point of two lines given the form Ax + By = C </summary>
    public static Vector2 LineIntersection(float A1, float B1, float C1, float A2, float B2, float C2)
    {
        float delta = A1 * B2 - A2 * B1;
        if (delta == 0){
            throw new ArgumentException("Lines are parallel");
        }

        float x = (B2 * C1 - B1 * C2) / delta;
        float y = (A1 * C2 - A2 * C1) / delta;
        return new Vector2(x, y);
    }
}