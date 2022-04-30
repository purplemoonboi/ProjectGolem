using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTesting : MonoBehaviour
{

    [Header("Player Health")]
    public int currentHealth;
    private int maxHealth = 1;
    private bool isDead = false;


    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float fireForce = 100f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, transform.rotation);

            Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();

            Vector3 force = fireForce * projectile.transform.forward;

            rigidbody.AddForce(force, ForceMode.Force);
        }

       
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            Debug.Log("Dead: " + currentHealth);
            Destroy(this.gameObject);
            isDead = true;
        }
    }
}
