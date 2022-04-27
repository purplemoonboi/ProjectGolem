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

    [Range(0.001f, 1f)]
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
    [SerializeField]
    private float rotationLerpThreshold = 0.8f;

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
    private bool isTurning = false;
    [SerializeField]
    private float currentDisplacement = 0f;
    private float currentVelocity = 0f;
    public bool inBase = false;
    public bool DisableInput { get; set; }

    //Collision
    private bool isCollision = false;
    private int numPoints;

    List<ContactPoint> contactList = new List<ContactPoint>();

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //Fails if rigidbody is null.
        Debug.Assert(rigidbody);
        transform.position = spawnPoint.position;
        DisableInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!DisableInput)
        {
            //Update forward vector and check for input.
            recievedInput = UpdateCharacter();

            if (recievedInput || currentVelocity > 0.001f)
            {

                emitter.Play();
                //Update current velocity.
                currentDisplacement = UpdateVelocity();

                //Update character's position.
                transform.position += (transform.forward * currentDisplacement);
            }
            else
            {
                currentVelocity = 0f;
                currentDisplacement = 0f;
            }
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
        s = Mathf.Clamp(s, -maxVelocity, maxVelocity);
        return s;
    }

    // @brief Simulates a spring effect.
    private Vector3 SimpleHarmonicMotion()
    {
        float omega = springConstant / mass;
        float x = 0f;
        float z = 0f;
        float offset = 1f;
        float a = maxAmplitude;
        if (recievedInput)
        {
            a += offset;
            x = 0.5f * a * Mathf.Sin(omega * Time.time);
            //z = a * Mathf.Sin(omega * Time.time);
        }

        float y = a * Mathf.Cos(omega * Time.time);

        return (transform.right * x) + (transform.up * y);
    }

    // @brief Updates the new forward vector and evaluates if *any* key press has occurred.
    private bool UpdateCharacter()
    {
        bool receivedInput = false;
        isTurning = false;

        //Default look at rotation.
        Vector3 lookVector = transform.forward;
        Vector3 pLookVector = transform.forward;

        if(!isCollision)
        {
            if (Input.GetKey(KeyCode.W))
            {
                receivedInput = true;
                //For clarity.
                lookVector = (lookVector + new Vector3(0f, -tiltAmount, 0f)).normalized;
            }
        }

        float t = Time.deltaTime;
        float a = angularAcceleration;
        float u, s;
        u = angularVelocity + a * t;
        s = (u * t) + (0.5f * a * t * t);

        //Update the direction of the forward vector.
        if (Input.GetKey(KeyCode.D))
        {
            
            //transform.eulerAngles += new Vector3(0f, s, 0f);
            lookVector = (transform.right + lookVector + new Vector3(0f, -tiltAmount, 0f)).normalized;
            pLookVector = (transform.right + transform.forward).normalized;
            isTurning = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //transform.eulerAngles -= new Vector3(0f, s, 0f);
            lookVector = (-transform.right + lookVector + new Vector3(0f, -tiltAmount, 0f)).normalized;
            pLookVector = (-transform.right + transform.forward).normalized;
            isTurning = true;
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

       if(receivedInput)
       {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            int layer = 1 << 8;
            if (Physics.Raycast(ray, out hit, 10f, layer))
            {
                Vector3 normal = hit.normal;
                Vector3 tangent = transform.forward;
                Vector3 forward = tangent - normal * Vector3.Dot(tangent, normal);

                Quaternion bodyRotation = Quaternion.LookRotation(forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, bodyRotation, 2f * Time.deltaTime);
            }
       }
        
        return receivedInput;
    }

    public void OnTriggertEnter(Collider other)
    {
        if (other.tag == "InBase")
        {
            inBase = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Environment")
        {
            isCollision = true;
            transform.position += collision.GetContact(0).normal * 10f * Time.deltaTime;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Environment")
        {
            isCollision = false;
        }
    }

   
}
