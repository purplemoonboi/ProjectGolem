using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class TurretSearchForEnemy : ActionNode
{

    private EnemyController[] enemies;

    protected override void OnStart()
    {
        enemies = FindObjectsOfType<EnemyController>();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate() 
    {
        State nodeState = State.Running;

        
        Debug.Log("Number of enemies in scene " + enemies.Length);
        float distance = 0.0f;

        GameObject gameObject = null;

        foreach(EnemyController enemy in enemies)
        {
            float currentDistanceToEnemy = (enemy.transform.position - context.transform.position).magnitude;

            if(distance < 0.01f || currentDistanceToEnemy < distance)
            {
                distance = currentDistanceToEnemy;
                gameObject = enemy.gameObject;
            }

            Debug.Log("Searching for targets...");
        }

        if(gameObject != null)
        {
            blackboard.targetObj = gameObject;
            return State.Success;
        }
        else
        {
            return State.Failure;
        }


        return State.Running;
    }
}
