using UnityEngine;

public static class Curve
{

    // @brief Returns the interpolated point between three points.
    public static Vector3 GetPoint(Vector3 point0, Vector3 point1, Vector3 point2, float interpolant)
    {
        interpolant = Mathf.Clamp01(interpolant);
        float oneMinusInterpolant = 1.0f - interpolant;

        return 
            oneMinusInterpolant * oneMinusInterpolant * point0 +
            2.0f * oneMinusInterpolant * interpolant * point1 +
            interpolant * interpolant * point2; 
    }

    // @brief Returns the first derivative of a point with respect to the interpolant.
    public static Vector3 GetFirstDerivative(Vector3 point0, Vector3 point1, Vector3 point2, float interpolant)
    {
        return
            2.0f * (1.0f - interpolant) * (point1 - point0) +
            2.0f * interpolant * (point2 - point1);
    }

    // @brief Returns the point on a cubic curve.
    public static Vector3 GetPoint(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float interpolant)
    {
        interpolant = Mathf.Clamp01(interpolant);
        float oneMinusInterpolant = 1.0f - interpolant;

        return oneMinusInterpolant * oneMinusInterpolant * oneMinusInterpolant * point0 +
            3.0f * oneMinusInterpolant * oneMinusInterpolant * interpolant * point1 +
            3.0f * oneMinusInterpolant * interpolant * interpolant * point2 +
            interpolant * interpolant * interpolant * point3;
    }

    // Returns the first derivative on a cubic curve.
    public static Vector3 GetFirstDerivative(Vector3 point0, Vector3 point1, Vector3 point2, Vector3 point3, float interpolant)
    {
        interpolant = Mathf.Clamp01(interpolant);
        float oneMinusInterpolant = 1f - interpolant;
        return
            3f * oneMinusInterpolant * oneMinusInterpolant * (point1 - point0) +
            6f * oneMinusInterpolant * interpolant * (point2 - point1) +
            3f * interpolant * interpolant * (point3 - point2);
    }


}
