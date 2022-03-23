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

    [SerializeField]
    private Vector3 lookAt;
    [SerializeField]
    private Vector3 previousLook;
    [SerializeField]
    private Transform lookAtInteractable;
    [SerializeField]
    private bool enableInput = true;
    [SerializeField]
    private Vector3 repositionDirection;
    private Vector3 oldPosition;


    // Start is called before the first frame update
    void Start()
    {
        distanceAlongSpline = 0.0f;
        previousLook = transform.position + transform.forward;
        lookAt = (spline.GetDirection(0f)).normalized;
    }


    private float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (spline == null)
            return;

        Debug.Log("Input " + enableInput);


        if (enableInput)
        {
            HandleInput();
        }
        else
        {
            timer += Time.deltaTime;
            if(timer > 0.5f)
            {
                timer = 0f;
                enableInput = true;
            }
        }

       // Debug.Log("Player position " + transform.position);
       // Debug.Log("Collider position " + collision.position);

        distanceAlongSpline = Mathf.Clamp01(distanceAlongSpline);
    }


    private void HandleInput()
    {
        lookAt = previousLook;

        if (Input.GetKey(KeyCode.W))
        {
            distanceAlongSpline += speed * reductionPercentage * Time.deltaTime;
            lookAt = (spline.GetDirection(distanceAlongSpline)).normalized;
        }
        if (Input.GetKey(KeyCode.S))
        {
            distanceAlongSpline -= speed * reductionPercentage * Time.deltaTime;
            lookAt = (spline.GetDirection(distanceAlongSpline)).normalized;
        }

        Vector3 splinePosition = spline.GetPointOnSpline(distanceAlongSpline);

        if (Input.GetKey(KeyCode.A))
        {
            splineOffset += transform.right * -speed * strafeReductionPercentage * Time.deltaTime;
            lookAt = (-transform.right).normalized;
        }
        if (Input.GetKey(KeyCode.D))
        {
            splineOffset += transform.right * speed * strafeReductionPercentage * Time.deltaTime;
            lookAt = (transform.right).normalized;
        }

       // Quaternion rotGoal = Quaternion.LookRotation(lookAt);
       // Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, rotGoal, 0.5f * Time.deltaTime);

        transform.position = splinePosition + splineOffset;
        previousLook = lookAt;
    }
    public float DistanceAlongSpline() => distanceAlongSpline;
    public Vector3 SplineOffset() => splineOffset;
    public bool InputEnabled() => enableInput;
    public void ToggleInput(bool value) => enableInput = value;
    public Spline GetSpline() => spline; 
}
