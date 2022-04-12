using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@author David Costa
public class CameraController : MonoBehaviour
{
    private Transform target;
    public Vector3 offSet;

    public bool followPlayer = true;
    private Vector3 lastPos = Vector3.zero;

    private void Awake()
    {
        lastPos = transform.position;
    }


    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        //offSet = transform.position - target.position;
    }



    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position + offSet;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //if (followPlayer && (target.position - lastPos).z > 0)
        //{
        //    Vector3 temp = transform.position;
        //    temp = target.position + offSet;
        //    temp.x = target.position.y + 3.80f;
        //    transform.position = temp;
        //}

        //transform.LookAt(target);

        //temp.x += offSet;

        transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
