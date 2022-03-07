using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSplineController : MonoBehaviour
{

    [SerializeField]
    private Spline spline;
    [SerializeField]
    [Range(1f, 1000f)]
    private float speed = 1.0f;
    [SerializeField]
    private float distanceAlongSpline;
    [SerializeField]
    private Vector3 splineOffset;

    // Start is called before the first frame update
    void Start()
    {
        distanceAlongSpline = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        distanceAlongSpline = Mathf.Clamp01(distanceAlongSpline);
    }

    private void HandleInput()
    {
        Vector3 position = transform.position;

        //Move whole curve left and right.
        if (Input.GetKey(KeyCode.D))
        {
            splineOffset += transform.right * speed * 0.01f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            splineOffset += transform.right * -speed * 0.01f * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            distanceAlongSpline += ((speed * 0.001f) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            distanceAlongSpline -= ((speed * 0.001f) * Time.deltaTime);
        }

        transform.LookAt(transform.position + spline.GetDirection(distanceAlongSpline));

        position = spline.GetPointOnSpline(distanceAlongSpline) + splineOffset;

        transform.position = position;  
    }

}
