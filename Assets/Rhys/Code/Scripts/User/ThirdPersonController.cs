using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Spawn Point")]
    [SerializeField]
    private Transform spawnPoint;

    [Header("Movement Settings")]

    [Tooltip("The maximum velocity the character will reach.")]
    [SerializeField]
    private float maxVelocity = 12f;

    [Tooltip("How quickly the character will reach their maximum velocity.")]
    [SerializeField]
    private float acceleration = 1.2f;

    [Header("Rotation Settings")]

    [SerializeField]
    private float angularVelocity = 12f;
    [SerializeField]
    private float angularAcceleration = 1.7f;

    [Tooltip("Amount the character should tilt in the direction they travel in.")]
    [Range(0.01f, 1f)]
    [SerializeField]
    private float tiltAmount = 0.25f;

    [Tooltip("How quickly the character tilts.")]
    [SerializeField]
    private float tiltSpeed = 1f;

    [Header("Oscillation Settings")]

    [Tooltip("Is the maximum and minimum height the character will reach when Oscillating.")]

    [Range(0.001f, 100f)]
    [SerializeField]
    private float maxAmplitude = 1f;

    [Tooltip("The spring constant dictates how far the oscillation will occur.")]
    [SerializeField]
    private float springConstant = 2f;

    [Tooltip("Represents the mass of the character. Influences the strength of the oscillation force.")]
    [SerializeField]
    private float mass = 1f;

    [SerializeField]
    private Transform graphicsTransform;

    [Header("Camera options")]
    [SerializeField]
    private Camera camera;

    [Tooltip("How quickly the camera faces the new direction.")]
    [SerializeField]
    private float cameraTiltSpeed = 1f;

    [Tooltip("The percentage between the GFX forward vector and the GameObjects forward vector.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float cameraLookAtInterpolant = 0.5f;

    [Header("An emitter for playing a dust effect.")]
    [SerializeField]
    private ParticleSystem emitter;

    [Header("Character Offset")]
    [Tooltip("How far away he is from the ground.")]
    [Range(1f, 10f)]
    [SerializeField]
    private float offsetScalar = 1f;
    [SerializeField]
    private Vector3 contactPoint;

    /*Other attributes that needn't be exposed*/
    private Rigidbody rigidbody = null;
    private Vector3 forward = new Vector3();
    private bool recievedInput = false;
    private float currentDisplacement = 0f;
    [SerializeField]
    private float currentVelocity = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //Fails if rigidbody is null.
        Debug.Assert(rigidbody);
        transform.position = spawnPoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Update forward vector and check for input.
        recievedInput = UpdateCharacter();
  
        if (recievedInput || currentVelocity > 0.001f)
        {
            
            emitter.Play();
            //Update current velocity.
            currentDisplacement += UpdateVelocity();
            
            //Update character's position.
            transform.position += (transform.forward * currentDisplacement);
        }
        else
        {
            currentVelocity = 0f;
            currentDisplacement = 0f;
        }

        //Oscillate the character (Affects the GFX only).
        graphicsTransform.position += SimpleHarmonicMotion() * Time.deltaTime;
    }

    // @brief Called after all evaluations and updates are completed for this frame.
    private void LateUpdate()
    {
        //Update the camera.

        //Create the two forward vectors.
        Vector3 graphicsForward = graphicsTransform.forward;
        Vector3 gameObjectForward = transform.forward;

        //Create the two rotation goals.
        Quaternion graphicsRotationGoal = Quaternion.LookRotation(graphicsForward);
        Quaternion gameObjectRotationGoal = Quaternion.LookRotation(gameObjectForward);

        //Now create the interpolated rotation goal.
        Quaternion rotationGoal = Quaternion.Lerp(graphicsRotationGoal, gameObjectRotationGoal, cameraLookAtInterpolant);

        //Perform tilt if necessary.
        if (camera.transform.rotation != rotationGoal)
        {
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, rotationGoal, cameraTiltSpeed * Time.deltaTime);
        }
    }

    // @brief Increases displacement of object using linear motion.
    private float UpdateVelocity()
    {
        float t = Time.deltaTime;
        // If input has occurred acceleration is +ve else -ve.
        float a = (recievedInput) ? acceleration : -(acceleration * acceleration);
        currentVelocity = a * t;
        float s = (currentVelocity * t) + (0.5f * a) * (t * t);
        return s;
    }

    // @brief Simulates a spring effect.
    private Vector3 SimpleHarmonicMotion()
    {
        float omega = springConstant / mass;
        float y = maxAmplitude * Mathf.Cos(omega * Time.time);
        return new Vector3(0f, y, 0f);
    }

    // @brief Updates the new forward vector and evaluates if *any* key press has occurred.
    private bool UpdateCharacter()
    {
        bool receivedInput = false;

        //Default look at rotation.
        Vector3 lookVector = transform.forward;
        Vector3 pLookVector = transform.forward;

        if (Input.GetKey(KeyCode.W))
        {
            receivedInput = true;
            //For clarity.
            lookVector = (lookVector + new Vector3(0f, -tiltAmount, 0f)).normalized;
        }

        float t = Time.deltaTime;
        float a = angularAcceleration;
        float u, s;

        //Update the direction of the forward vector.
        if (Input.GetKey(KeyCode.D))
        {
            u = angularVelocity + a * t;
            s = (u * t) + (0.5f * a * t * t);
            //transform.eulerAngles += new Vector3(0f, s, 0f);
            lookVector = (transform.right + lookVector + new Vector3(0f, -tiltAmount, 0f)).normalized;
            pLookVector = (transform.right + transform.forward).normalized;
        }
        if (Input.GetKey(KeyCode.A))
        {
            u = angularVelocity + a * t;
            s = (u * t) + (0.5f * a * t * t);
            //transform.eulerAngles -= new Vector3(0f, s, 0f);
            lookVector = (-transform.right + lookVector + new Vector3(0f, -tiltAmount, 0f)).normalized;
            pLookVector = (-transform.right + transform.forward).normalized;

        }

        Quaternion rotationGoal = Quaternion.LookRotation(lookVector);

        //Perform tilt if necessary.
        if (graphicsTransform.rotation != rotationGoal)
        {
            graphicsTransform.rotation = Quaternion.Slerp(graphicsTransform.rotation, rotationGoal, tiltSpeed * Time.deltaTime);
        }

        rotationGoal = Quaternion.LookRotation(pLookVector);

        if (transform.rotation != rotationGoal)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, 2f * Time.deltaTime);
        }

        return receivedInput;
    }

}
