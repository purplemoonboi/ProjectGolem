using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckFriendlyType : ActionNode
{
    public FriendlyController.FriendlyType assignedType;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.friendlyController.GetFriendlyType() == assignedType)
        {
            context.friendlyController.SetFriendlyType(assignedType);   //Updating the UI text object

            return State.Success;
        }

        else
        {
            return State.Failure;
        }
    }
}