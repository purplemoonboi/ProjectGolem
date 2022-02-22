//@author Bridget Casey

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    private float elapsedTime;
    private float attackTime;

    [SerializeField]
    private float health;
    private const float MAX_HEALTH = 1000;

    [SerializeField]
    private float damageTaken;

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
    private int power;  //How much damage the enemy can do in one hit

    private bool isActivated = false;

    void Start()
    {
        health = MAX_HEALTH;
        attackTime = 1.0f;
        elapsedTime = attackTime;
        healthText.GetComponent<Text>().text = "HEALTH: " + health;
        healthBarRect = healthBar.GetComponent<RectTransform>();
        maxWidth = healthBarRect.rect.width;
        power = 50;
    }

    void Update()
    {
        if(isActivated)
        {
            UpdateCanvasRotation();
            UpdateUIComponents();

            if (health <= 0.0f)
            {
                Destroy(gameObject);
            }

            if (damageTaken > 0.0f)
            {
                ProcessDamage();
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(isActivated)
        {
            EnemyMovement enemy = collision.transform.gameObject.GetComponent<EnemyMovement>();

            if (enemy)
            {
                damageTaken += Mathf.Max(0, enemy.GetPower());
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if(isActivated)
        {
            EnemyMovement enemy = collision.transform.gameObject.GetComponent<EnemyMovement>();

            if (enemy)
            {
                damageTaken -= Mathf.Max(0, enemy.GetPower());  //Ensuring negative damage can't be done
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
        }
    }

    public void UpdateCanvasRotation()
    {
        Vector3 direction = canvas.transform.position - Camera.main.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        canvas.transform.rotation = lookRotation;
    }

    public void UpdateUIComponents()
    {
        healthText.GetComponent<Text>().text = "HEALTH: " + health;

        float newWidth = Remap(health, 0.0f, MAX_HEALTH, 0.0f, maxWidth);
        healthBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    private float Remap(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return ((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin) + newMin;
    }

    public int GetPower() { return power; }

    public void SetIsActivated(bool value)
    {
        isActivated = value;
    }

    public bool IsActivated()
    {
        return isActivated;
    }

    public void SetHealth(float h) { health = h; }

    public float GetHealth()
    {
        return health;
    }
}