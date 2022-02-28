using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class FriendlyFlee : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.friendlyController.GetHealth() > (context.friendlyController.GetMaxHealth() / 4.0f))
            return State.Failure;

        blackboard.moveToPosition = new Vector3(Random.Range(-5.0f, 5.0f), 1.0f, Random.Range(-5.0f, 5.0f));

        return State.Success;
    }
}
