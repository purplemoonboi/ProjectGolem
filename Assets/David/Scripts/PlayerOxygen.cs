using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOxygen : MonoBehaviour
{
    public static PlayerOxygen instance;

    public Slider oxygenBar;
    public float oxygenAmount = 100.0f;

    [SerializeField]
    public float currentOxygen;

    [SerializeField]
    private float maxDistance = 500.0f;

    public float reduceAmount = 5.0f;

    private GameObject player;

    [SerializeField]
    private Transform basePosition;
    [SerializeField]
    private bool lowOxygen;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        currentOxygen = oxygenAmount;

        if (basePosition == null)
            return;

        basePosition = FindObjectOfType<CampBuilding>().GetComponent<Transform>();

        lowOxygen = false;
    }

    // Update is called once per frame
    void Update()
    {
        //  if (Player == null)
        //     return;

        ReduceOxygen();
      //LoseOxygen();
      //PlayerDeath();
    }

    public void LoseOxygen()
    {
        if (RecoverOxygen.instance.isRecovering == false)
        {
            currentOxygen -= reduceAmount * Time.deltaTime;
            oxygenBar.value = currentOxygen / oxygenAmount;
        }
    }

    void PlayerDeath()
    {
        if (currentOxygen <= 0f)
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0f);
        LevelManager.instance.Respawn();
        currentOxygen = oxygenAmount;
    }

    public void ReduceOxygen()
    {
        if (basePosition == null)
            return;

        float playerDistance = Vector3.Distance(player.transform.position, basePosition.position);

        //Remaps the player distance between the [min,max] distance to [0, oxygenAmount].
        float newValue = MathsUtils.RemapRange(playerDistance, 0.0f, maxDistance, 0.0f, oxygenAmount);
        newValue = Mathf.Clamp(newValue, 1.0f, oxygenAmount);
        currentOxygen = (oxygenAmount - newValue);
        oxygenBar.value = currentOxygen / oxygenAmount;
        //Debug.Log("O2 Value : " + oxygenBar.value);
        //If current oxygen is below 5% flag this as low oxygen.
        lowOxygen = (oxygenBar.value < 0.05f) ? true : false;
    }

    public bool LowOxygen() => lowOxygen;

}
