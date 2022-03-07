using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckRecruited : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.friendlyController.GetRecruited())
        {
            blackboard.moveToPosition = context.friendlyController.GetSpawnPoint();

            return State.Success;
        }

        return State.Running;
    }
}
