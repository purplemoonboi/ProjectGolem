using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : NpcController
{
    [SerializeField]
    private EnemyScriptableObject enemyData;

    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float fireForce = 1000.0f;
    [SerializeField]
    private float fireRate = 1.0f;
    [SerializeField]
    private bool shouldFire = false;

    private float fireTimer = 0.0f;

    void Start()
    {
        SetupCharacter(enemyData.MAX_HEALTH, enemyData.power);

        spawnPoint = transform.position;
    }

    void Update()
    {
        UpdateUIComponents();
        CheckDeath();

        fireTimer += 1.0f * Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            shouldFire = true;
            fireTimer = 0.0f;
        }
    }

    public void SpawnProjectile(Vector3 targetPosition)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetPosition - transform.position, turnSpeed * Time.deltaTime, 0.0f));

        if (shouldFire)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);

            Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();

            Vector3 force = fireForce * projectile.transform.forward;

            rigidbody.AddForce(force, ForceMode.Force);

            shouldFire = false;
        }
    }
}