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
        if (context.friendlyController.GetFriendlyType() == FriendlyController.FriendlyType.FIGHTER)
        {
            if (context.friendlyController.GetHealth() > (context.friendlyController.GetMaxHealth() / 4.0f))
                return State.Failure;
        }
        
        else
        {
            if (!context.friendlyController.GetInCombat())
                return State.Failure;
        }

        blackboard.targetObj = null;
        blackboard.moveToPosition = context.friendlyController.GetSpawnPoint();

        return State.Success;
    }
}