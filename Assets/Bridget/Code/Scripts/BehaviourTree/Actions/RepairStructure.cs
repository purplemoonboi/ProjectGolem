using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class RepairStructure : ActionNode
{
    public float distanceToTarget;

    public TurretStats structure;

    public ParticleSystem builderSparks;

    private float timer = 0.0f;

    protected override void OnStart()
    {
        if (blackboard.targetObj != null)
            structure = blackboard.targetObj.GetComponent<TurretStats>();

        builderSparks = context.friendlyController.gameObject.GetComponent<ParticleSystem>();

        builderSparks.Play();
    }

    protected override void OnStop()
    {
        builderSparks.Stop();
    }

    protected override State OnUpdate()
    {
        if (structure == null)
            return State.Failure;

        if (context.friendlyController.GetInCombat())
            return State.Failure;

        Vector3 direction = structure.gameObject.transform.position - context.transform.position;
        distanceToTarget = direction.magnitude;

        if (structure.GetHealth() >= structure.GetMaxHealth())
        {
            blackboard.targetObj = null;
            return State.Success;
        }

        if (distanceToTarget < 10.0f)
        {
            PerformRepair();
        }
        else
            return State.Failure;

        return State.Running;
    }

    public void PerformRepair()
    {
        timer += 1.0f * Time.deltaTime;

        if (timer > 0.8f)
        {
            structure.SetHealth(structure.GetHealth() + context.friendlyController.GetPower());
            timer = 0.0f;
        }
    }
}