using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Flee : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.enemy.GetHealth() > (context.enemy.GetMaxHealth() / 4.0f))
            return State.Failure;

        blackboard.moveToPosition = new Vector3(0.0f, 1.0f, 0.0f);

        return State.Success;
    }
}
