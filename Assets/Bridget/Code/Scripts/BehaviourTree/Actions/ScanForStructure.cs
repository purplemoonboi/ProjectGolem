using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ScanForStructure : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.enemyController)    //Adding this check as this node will be reused for friendly builders
        {
            if (context.enemyController.GetHealth() <= (context.enemyController.GetMaxHealth() / 4.0f))
                return State.Failure;

            if (ScanTargetsAsEnemy())
                return State.Success;
        }

        else if (context.friendlyController)
        {
            if (context.friendlyController.GetFriendlyType() != FriendlyController.FriendlyType.BUILDER)
                return State.Failure;

            if (context.friendlyController.GetInCombat())   //If a builder is being attacked, flee
                return State.Failure;

            if (context.friendlyController.GetHealth() <= (context.friendlyController.GetMaxHealth() / 4.0f))
                return State.Failure;

            if (ScanTargetsAsFriendly())
                return State.Success;
            else
                return State.Failure;
        }

        return State.Running;
    }

    public bool ScanTargetsAsEnemy()
    {
        var structures = FindObjectsOfType<TurretStats>();

        float distance = 0.0f;

        foreach (var target in structures)
        {
            if (target.IsActivated())
            {
                Vector3 direction = target.transform.position - context.transform.position;

                if ((direction.magnitude < distance || distance <= 0.0f))
                {
                    distance = direction.magnitude;
                    blackboard.moveToPosition = target.transform.position;
                    blackboard.targetObj = target.gameObject;
                }
            }
            //Debug.Log("Target Pos: " + target.transform.position + "Distance:" + distance);
        }

        //Debug.Log("Next Position: " + blackboard.moveToPosition);

        if (distance == 0.0f)
            return false;

        return true;
    }

    public bool ScanTargetsAsFriendly()
    {
        var structures = FindObjectsOfType<TurretStats>();

        float distance = 0.0f;

        foreach (var target in structures)
        {
            if (target.GetHealth() < target.GetMaxHealth())
            {
                Vector3 direction = target.transform.position - context.transform.position;

                if (direction.magnitude < distance || distance == 0.0f)
                {
                    distance = direction.magnitude;
                    blackboard.moveToPosition = target.transform.position;
                    blackboard.targetObj = target.gameObject;
                }
            }
            //Debug.Log("Target Pos: " + target.transform.position + "Distance:" + distance);
        }

        //Debug.Log("Next Position: " + blackboard.moveToPosition);

        if (distance == 0.0f)
            return false;

        return true;
    }
}