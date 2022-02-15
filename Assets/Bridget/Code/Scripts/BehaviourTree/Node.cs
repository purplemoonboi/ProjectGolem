//@author Bridget A. Casey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public enum NodeState
    {
        RUN = 0,
        SUCCESS,
        FAIL
    }

    protected NodeState activeState = NodeState.RUN;

    public NodeState Update()
    {
        return activeState;
    }

    public abstract void OnBegin();
    public abstract void OnEnd();
}