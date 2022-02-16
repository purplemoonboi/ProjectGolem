using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStats : MonoBehaviour
{

    [SerializeField]
    private float damage = 100.0f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            EnemyMovement enemy = collision.transform.GetComponent<EnemyMovement>();

            if(enemy != null)
            {
                enemy.ProcessDamage();
            }

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            if(this != null)
            {
                Destroy(this);
            }
        }
    }


}
