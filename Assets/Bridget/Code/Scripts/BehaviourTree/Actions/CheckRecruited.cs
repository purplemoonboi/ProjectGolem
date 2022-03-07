using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckRecruited : ActionNode
{
    bool firstSetup = true; //Checking if this is the first run of the behaviour tree that the friendly character has been flagged as 'recruited'

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
            if (!firstSetup)
                return State.Failure;

            blackboard.moveToPosition = context.friendlyController.GetSpawnPoint();
            firstSetup = false;

            return State.Success;
        }

        return State.Running;
    }
}
