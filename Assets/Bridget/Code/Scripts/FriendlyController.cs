using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FriendlyController : NpcController
{
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
    }

    void Update()
    {
        UpdateUIComponents();

        if(health <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void SetFriendlyType(FriendlyType type)
    {
        friendlyType = type;
        typeText.GetComponent<Text>().text = friendlyType.ToString();
    }

    public FriendlyType GetFriendlyType() { return friendlyType; }
}
