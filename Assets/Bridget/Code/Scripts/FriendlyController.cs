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
        recruited = false;
        SetupCharacter(friendlyData.MAX_HEALTH, friendlyData.power);
        typeText.GetComponent<Text>().text = friendlyType.ToString();
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
        if (shouldFire)
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();

            Vector3 direction = Vector3.Normalize(targetPosition - transform.position);

            Vector3 force = fireForce * direction;

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
}