using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

public class RandomPosition : ActionNode
{
    public Vector2 min = Vector2.one * -5;
    public Vector2 max = Vector2.one * 5;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 randomPosition = context.transform.position;

        randomPosition.x += (Random.Range(min.x, max.x));
        randomPosition.z += (Random.Range(min.y, max.y));

        NavMeshHit navMeshHit;

        NavMesh.SamplePosition(randomPosition, out navMeshHit, 10.0f, 1);

        blackboard.moveToPosition = navMeshHit.position;

        return State.Success;
    }
}