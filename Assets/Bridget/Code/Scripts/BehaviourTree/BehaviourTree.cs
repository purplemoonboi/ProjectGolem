//@author Bridget A. Casey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree
{
    public enum TreeState
    {
        RUN = 0,
        SUCCESS,
        FAIL
    }

    private Node root;
    private List<Node> nodes = new List<Node>();
}