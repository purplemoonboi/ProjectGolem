using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : NpcController
{
    [SerializeField]
    private EnemyScriptableObject enemyData;

    void Start()
    {
        SetupCharacter(enemyData.MAX_HEALTH, enemyData.power);
    }

    void Update()
    {
        UpdateUIComponents();
    }
}