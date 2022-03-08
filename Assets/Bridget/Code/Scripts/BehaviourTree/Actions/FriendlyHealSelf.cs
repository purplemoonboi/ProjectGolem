using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class FriendlyHealSelf : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.friendlyController.GetHealth() >= context.friendlyController.GetMaxHealth())
        {
            return State.Success;
        }

        context.friendlyController.SetHealth(context.friendlyController.GetHealth() + 1.0f);

        return State.Running;
    }
}
