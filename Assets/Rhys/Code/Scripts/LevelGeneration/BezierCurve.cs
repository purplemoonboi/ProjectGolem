using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    [SerializeField]
    public Vector3[] points;

    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(0.0f, 0.0f, 0.0f),       
            new Vector3(2.0f, 0.0f, 0.0f),       
            new Vector3(4.0f, 0.0f, 0.0f)
        };

    }

    public Vector3 GetPoint(float interpolant)
    {
        return transform.TransformPoint(Curve.GetPoint(points[0], points[1], points[2], interpolant));
    }

    public Vector3 GetVelocity(float interpolant)
    {
        return transform.TransformPoint(Curve.GetFirstDerivative(points[0], points[1], points[2], interpolant)) - transform.position;
    }

    public Vector3 GetDirection(float interpolant)
    {
        return GetVelocity(interpolant).normalized;
    }


}
