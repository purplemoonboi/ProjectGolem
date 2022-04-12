using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum BezierControlPointMode
{
    Free,
    Aligned,
    Mirrored
}

public class Spline : MonoBehaviour
{
    [SerializeField]
    private Vector3[] points;

    [SerializeField]
    private BezierControlPointMode[] modes;

    [SerializeField]
    private bool loop;

    [SerializeField]
    private float offset = 0.1f;

    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1f,0f,0f),
            new Vector3(2f,0f,0f),
            new Vector3(3f,0f,0f),
            new Vector3(4f,0f,0f)
        };

        modes = new BezierControlPointMode[]
        {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };
    }

    public bool Loop
    {
        get
        {
            return loop;
        }
        set
        {
            loop = value;
            if(value == true)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }
        }
    }

    public int CurveCount
    {
        get
        {
            return (points.Length - 1) / 3;
        }
    }

    public int ControlPointCount()
    {
        return points.Length;
    }

    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }

    public void SetControlPoint(int index, Vector3 value)
    {
        if(index % 3 == 0)
        {
            Vector3 delta = value - points[index];
            if (loop)
            {
                if (index == 0)
                {
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                    points[points.Length - 1] = value;
                }
                else if (index == points.Length - 1)
                {
                    points[0] = value;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }
            }
            else
            {
                if (index > 0)
                {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Length)
                {
                    points[index + 1] += delta;
                }
            }
        }

        points[index] = value;
        EnforceMode(index);
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;

        if(loop)
        {
            if(modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if(modeIndex == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }

        modes[(index + 1) / 3] = mode;
        EnforceMode(index);
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if(mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex = 0;
        int enforcedIndex = 0;

        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if(fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }
            enforcedIndex = middleIndex + 1;
            if(enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if(fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if(enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];

        if(mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }

        points[enforcedIndex] = middle + enforcedTangent;
    }

    public Vector3 GetPointOnSpline(float interpolant)
    {
        int index = 0;
        if(interpolant >= 1.0f)
        {
            interpolant = 1.0f;
            index = points.Length - 4;
        }
        else
        {
            interpolant = Mathf.Clamp01(interpolant) * CurveCount;
            index = (int)interpolant;
            interpolant -= index;
            index *= 3;
        }

        return transform.TransformPoint(Curve.GetPoint(points[index], points[index + 1], points[index + 2], points[index + 3], interpolant));
    }

    public Vector3 GetVelocity(float interpolant)
    {
        int index = 0;
        
        if(interpolant >= 1.0f)
        {
            interpolant = 1.0f;
            index = points.Length - 4;
        }
        else
        {
            interpolant = Mathf.Clamp01(interpolant) * CurveCount;
            index = (int)interpolant;
            interpolant -= index;
            index *= 3;
        }

        return transform.TransformPoint(Curve.GetFirstDerivative(points[index], points[index + 1], points[index + 2], points[index + 3], interpolant))
            - transform.position;
    }

    public Vector3 GetDirection(float interpolant)
    {
        return GetVelocity(interpolant).normalized;
    }

    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode(points.Length - 4);

        if (loop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
    }

    public void SetSplinePosition()
    {
        Debug.Log("Invoked");
        for (int i = 0; i < points.Length; ++i)
        {
            points[i] = new Vector3(points[i].x, offset, points[i].z);
        }
    }

    public float Offset() => offset;
    public void SetOffset(float o) => offset = o;

}
