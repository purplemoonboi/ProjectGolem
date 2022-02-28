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

    public Vector3 GetPoint(float t)
    {
        // return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t));
        return new Vector3();
    }
}
