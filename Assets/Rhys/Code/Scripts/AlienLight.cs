using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienLight : MonoBehaviour
{

    [SerializeField]
    private Transform transformA;
    [SerializeField]
    private Transform transformB;
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private Vector3 currentTarget;
    private Vector3 startPosition;
    private float step = 0f;

    void Start()
    {
        currentTarget = transformB.position;
        startPosition = transformA.position;
    }

    // Update is called once per frame
    void Update()
    {
        step += speed * Time.deltaTime;

        //Move light closer to target.
        transform.position = Vector3.Lerp(startPosition, currentTarget, step);

        //If distance is close swap target.
        if(Vector3.Distance(transform.position, currentTarget) < 1f)
        {
           currentTarget = (currentTarget == transformA.position) ? transformB.position : transformA.position;
           startPosition = (startPosition == transformA.position) ? transformB.position : transformA.position;
            step = 0f;
        }
    }



}
