using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RandomPosition : ActionNode
{
    public Vector2 min = Vector2.one *-10;
    public Vector2 max = Vector2.one * 10;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        blackboard.moveToPosition.x = context.transform.position.x + (Random.Range(min.x, max.x) * 2.0f);
        blackboard.moveToPosition.z = context.transform.position.z + (Random.Range(min.y, max.y) * 2.0f);

        return State.Success;
    }
}