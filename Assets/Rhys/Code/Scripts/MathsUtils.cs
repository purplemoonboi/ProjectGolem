using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathsUtils 
{
    public static float RemapRange(float t, float a, float b, float c, float d) => ((t - a) / (b - a)) * (d - c) + c;
}

