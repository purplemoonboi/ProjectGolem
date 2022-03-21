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
    [Header("Movement Tools")]
    [Tooltip("Scales the speed of movement along the spline.")]
    [SerializeField]
    [Range(0.001f, 1f)]
    private float reductionPercentage;
    [Tooltip("Like the other 'reduction' variable, scales the speed of strafing.")]
    [SerializeField]
    [Range(0.001f, 1f)]
    private float strafeReductionPercentage;


    // Start is called before the first frame update
    void Start()
    {
        distanceAlongSpline = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (spline == null)
            return;

        HandleInput();

        distanceAlongSpline = Mathf.Clamp01(distanceAlongSpline);
    }

    private void HandleInput()
    {
        Vector3 position = transform.position;

        //Move whole curve left and right.
        if (Input.GetKey(KeyCode.D))
        {
            splineOffset += (transform.right + new Vector3(1,0,0)) * speed * strafeReductionPercentage * Time.deltaTime;
            transform.eulerAngles += new Vector3(0f, 5f * Time.deltaTime, 0f);

        }
        if (Input.GetKey(KeyCode.A))
        {
            splineOffset += (transform.right + new Vector3(1, 0, 0)) * -speed * strafeReductionPercentage * Time.deltaTime;
            transform.eulerAngles -= new Vector3(0f, 5f * Time.deltaTime, 0f);
        }

        if (Input.GetKey(KeyCode.W))
        {
            distanceAlongSpline += ((speed * reductionPercentage) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            distanceAlongSpline -= ((speed * reductionPercentage) * Time.deltaTime);
        }

        transform.LookAt(transform.position + spline.GetDirection(distanceAlongSpline));

        position = spline.GetPointOnSpline(distanceAlongSpline) + splineOffset;

        transform.position = position;  
    }

}
