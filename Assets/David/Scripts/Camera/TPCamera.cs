using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour
{

    private Transform player;

    public float turnSpeed = 2.0f;
    public Quaternion turnTo;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        turnTo = player.transform.rotation;
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.Slerp(transform.rotation, turnTo, turnSpeed * Time.deltaTime);
    }
}
