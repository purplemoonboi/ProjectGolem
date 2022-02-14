//@author Bridget Casey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private float elapsedTime;
    private float attackTime;

    [SerializeField]
    private bool isDead = false;

    [SerializeField]
    private float health;
    private const float MAX_HEALTH = 100;

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject healthText;

    [SerializeField]
    private GameObject healthBar;
    private RectTransform healthBarRect;

    [SerializeField]
    private float maxWidth;

    [SerializeField]
    private float damageTaken;

    [SerializeField]
    private GameObject target;

    private NavMeshAgent agent;

    [SerializeField]
    private int id;
    [SerializeField]
    private int power;  //How much damage the enemy can do in one hit

    void Start()
    {
        health = MAX_HEALTH;
        attackTime = 1.0f;
        elapsedTime = attackTime;
        healthText.GetComponent<Text>().text = "HEALTH: " + health;
        healthBarRect = healthBar.GetComponent<RectTransform>();
        maxWidth = healthBarRect.rect.width;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = transform.forward;
        power = 10;
    }

    void Update()
    {
        UpdateCanvasRotation();

        if (health <= 0.0f)
        {
            agent.enabled = false;
            transform.position.Set(-999.9f, -999.9f, -999.9f);  //Moving the object far away so OnCollisionExit will trigger

            isDead = true;
        }

        if(target != null)
        {
            if(agent.enabled)
            agent.destination = target.transform.position;
        }

        if (damageTaken > 0.0f)
        {
            ProcessDamage();
        }
    }

    private void FixedUpdate()
    {
        Vector3 origin, direction;
        Ray ray;
        RaycastHit hit;
        float maxDistance = 50;

        origin = transform.position;
        direction = transform.forward;
        ray = new Ray(origin, direction);

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            EnemyTarget tower = hit.collider.GetComponent<EnemyTarget>();

            if(tower)
            {
                target = tower.gameObject;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        EnemyTarget tower = collision.transform.gameObject.GetComponent<EnemyTarget>();

        if (tower)
        {
            Debug.Log(gameObject.name + " IS colliding with " + collision.gameObject.name);

            damageTaken += Mathf.Max(0, tower.GetPower());
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        EnemyTarget tower = collision.transform.gameObject.GetComponent<EnemyTarget>();

        if (tower)
        {
            Debug.Log(gameObject.name + " IS NOT colliding with " + collision.gameObject.name);

            damageTaken -= Mathf.Max(0, tower.GetPower());  //Ensuring negative damage can't be done

            if (isDead)
            {
                target = null;
                Destroy(gameObject);
            }
        }
    }

    public void ProcessDamage()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > attackTime)
        {
            elapsedTime = 0.0f;

            health -= damageTaken;
            Mathf.Clamp(health, 0, MAX_HEALTH);

            healthText.GetComponent<Text>().text = "HEALTH: " + health;

            float newWidth = Remap(health, 0.0f, MAX_HEALTH, 0.0f, maxWidth);
            healthBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        }
    }

    public void UpdateCanvasRotation()
    {
        Vector3 direction = canvas.transform.position - Camera.main.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        canvas.transform.rotation = lookRotation;
    }

    private float Remap(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return ((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin) + newMin;
    }

    public void SetID(int enemyID) { id = enemyID; }

    public int GetID() { return id; }

    public int GetPower() { return power; }
}