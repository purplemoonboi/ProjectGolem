using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandScript : MonoBehaviour
{

    [SerializeField]
    private Material sandMaterial;
    [SerializeField]
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        sandMaterial.SetVector("Player Position", new Vector4(playerPos.x, playerPos.y, playerPos.z, 1f));
    }
}
