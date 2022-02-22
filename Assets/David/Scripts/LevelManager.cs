using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public GameObject player;
    public Transform respawnPoint;

    private void Awake()
    {
        instance = this;
    }

    public void Respawn()
    {
        player.transform.position = respawnPoint.transform.position;
    }
}
