//@author Bridget A. Casey

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodePort : Port
{
    public NodePort(Direction direction, Capacity capacity) : base(Orientation.Vertical, direction, capacity, typeof(bool))
    {

    }
}