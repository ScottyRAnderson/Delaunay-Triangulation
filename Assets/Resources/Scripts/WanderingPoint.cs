using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingPoint : MonoBehaviour
{
    public SpriteRenderer SpriteComponent;

    private Point Point;
    private float Speed;
    private bool Locked;

    private float DirectionChangeTimer;
    private Vector2 TargetDirection;
    private PointFieldManager FieldManager;

    public Point point { get { return Point; } }

    public void InitializePoint(Point point, Vector2 minMaxSpeed, bool locked = false)
    {
        Point = point;
        Speed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);
        Locked = locked;

        transform.position = new Vector3(Point.x, Point.y, 0f);
        FieldManager = FindObjectOfType<PointFieldManager>();
    }

    public void SetDirection(Vector2 direction){
        TargetDirection = direction;
    }

    private void Update()
    {
        if(Locked){
            return;
        }

        DirectionChangeTimer -= Time.deltaTime;
        if (DirectionChangeTimer <= 0)
        {
            TargetDirection = new Vector2(Random.Range(-360f, 360f), Random.Range(-360f, 360f));
            DirectionChangeTimer = 4f;
        }

        Quaternion lookRotationVector = Quaternion.LookRotation(new Vector3(TargetDirection.x, TargetDirection.y, 0f) - transform.position);
        lookRotationVector.x = 0;
        lookRotationVector.y = 0;

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotationVector, Time.deltaTime * 1.5f);
        transform.position += transform.up * Time.deltaTime * Speed;
        Point.SetPosition(new Vector2(transform.position.x, transform.position.y));
        FieldManager.HandleBoundRestriction(this);
    }
}