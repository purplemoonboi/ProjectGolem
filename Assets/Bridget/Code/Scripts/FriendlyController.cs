using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FriendlyController : NpcController
{
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

    [SerializeField]
    private bool recruited = false;

    public enum FriendlyType
    {
        CIVILIAN = 0,
        BUILDER,
        FIGHTER
    }

    [SerializeField]
    private FriendlyScriptableObject friendlyData;

    [SerializeField]
    private GameObject typeText;

    [SerializeField]
    private FriendlyType friendlyType;

    void Start()
    {
        SetupCharacter(friendlyData.MAX_HEALTH, friendlyData.power);
        typeText.GetComponent<Text>().text = friendlyType.ToString();

        spawnPoint = transform.position;

        //recruitment mechanic pointless because the level is so small, hardcoding them to always be recruited
        recruited = true;
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

    public void SetFriendlyType(FriendlyType type)
    {
        friendlyType = type;
        typeText.GetComponent<Text>().text = friendlyType.ToString();
    }

    public FriendlyType GetFriendlyType() { return friendlyType; }

    public void SetRecruited(bool r) { recruited = r; }

    public bool GetRecruited() { return recruited; }

    public float GetRepairRate() => friendlyData.repairRate;
}