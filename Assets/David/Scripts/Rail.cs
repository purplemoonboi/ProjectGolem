using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@author David Costa
public class Rail : MonoBehaviour
{
    [SerializeField]
    private Vector3[] nodes;

    //[SerializeField]
    private int nodeCount;

    private void Start()
    {
        nodeCount = transform.childCount;
        nodes = new Vector3[nodeCount];

        for (int i = 0; i < nodeCount; i++)
        {
            nodes[i] = transform.GetChild(i).position;
        }
    }

    private void Update()
    {
        if (nodeCount > 1)
        {
            for (int i = 0; i < nodeCount - 1; i++)
            {
                Debug.DrawLine(nodes[i], nodes[i + 1], Color.green);
            }
        }
    }

    public Vector3 ProjectPositionOnRail(Vector3 position)
    {
        int closestNodeIndex = GetClosestNode(position);
        if (closestNodeIndex == 0)
        {
            //Project on the first segment
            return ProjectOnSegment(nodes[0], nodes[1], position);

        }
        else if (closestNodeIndex == nodeCount - 1)
        {
            //Project on the last segment
            return ProjectOnSegment(nodes[nodeCount - 1], nodes[nodeCount - 2], position);
        }
        else
        {
            //Project on the two connected segments
            //return shortest vector
            Vector3 leftSeg = ProjectOnSegment(nodes[closestNodeIndex - 1], nodes[closestNodeIndex], position);
            Vector3 rightSeg = ProjectOnSegment(nodes[closestNodeIndex + 1], nodes[closestNodeIndex], position);

            Debug.DrawLine(position, leftSeg, Color.red);
            Debug.DrawLine(position, rightSeg, Color.blue);

            if ((position - leftSeg).sqrMagnitude <= (position - rightSeg).sqrMagnitude)
            {
                return leftSeg;
            }
            else
            {
                return rightSeg;
            }
        }

    }

    private int GetClosestNode(Vector3 position)
    {
        int closestNodeIndex = -1;
        float shortestDistance = 0.0f;

        for (int i = 0; i < nodeCount; i++)
        {
            float sqrDistance = (nodes[i] - position).sqrMagnitude;
            if (shortestDistance == 0.0f || sqrDistance < shortestDistance)
            {
                shortestDistance = sqrDistance;
                closestNodeIndex = i;
            }
        }

        return closestNodeIndex;
    }

    private Vector3 ProjectOnSegment(Vector3 startPoint, Vector3 endPoint, Vector3 position)
    {
        Vector3 v1ToPos = position - startPoint;
        Vector3 segDirection = (endPoint - startPoint).normalized;

        //it should gives us the distance from the first point to where we should be on the line
        float distanceFromV1 = Vector3.Dot(segDirection, v1ToPos);
        if (distanceFromV1 < 0.0f)
        {
            return startPoint;
        }
        else if (distanceFromV1 * distanceFromV1 > (endPoint - startPoint).sqrMagnitude)
        {
            return endPoint;
        }
        else
        {
            //this should gives the exact point startPoint to the point we want to be
            Vector3 fromV1 = segDirection * distanceFromV1;
            return startPoint + fromV1;
        }
    }
}
