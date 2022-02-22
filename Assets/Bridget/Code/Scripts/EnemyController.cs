using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyScriptableObject enemyData;

    [SerializeField]
    private GameObject healthText;

    [SerializeField]
    private GameObject healthBar;
    private RectTransform healthBarRect;

    private float health;

    private float maxWidth;

    // Start is called before the first frame update
    void Start()
    {
        health = enemyData.MAX_HEALTH;

        healthText.GetComponent<Text>().text = "HEALTH: " + health;
        healthBarRect = healthBar.GetComponent<RectTransform>();
        maxWidth = healthBarRect.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIComponents();
    }

    public void UpdateUIComponents()
    {
        healthText.GetComponent<Text>().text = "HEALTH: " + health;

        float newWidth = Remap(health, 0.0f, enemyData.MAX_HEALTH, 0.0f, maxWidth);
        healthBarRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
    }

    private float Remap(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return ((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin) + newMin;
    }

    public void SetHealth(float h) { health = h; }

    public float GetHealth() { return health; }

    public float GetMaxHealth() { return enemyData.MAX_HEALTH; }

    public float GetPower() { return enemyData.power; }
}
