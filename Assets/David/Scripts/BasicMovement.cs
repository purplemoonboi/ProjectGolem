using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//@author David Costa
public class BasicMovement : MonoBehaviour
{

    //public variables
    [Header("Player Settings")]
    public float speedX = 5f;
    public float speedY = 5f;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;

    [Header("Player Oxygen")]
    [SerializeField]
    private PlayerOxygen playerOxygen;

    //private variables
    float smoothVelocity;
    private CharacterController controller;
    bool initialRot = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(playerOxygen.LowOxygen())
        {
            speed  = 0f;
            speedX = 0f;
            speedY = 0f;
        }
        else
        {
            speed  = 100f;
            speedX = 25f;
            speedY = 25f;
        }

        PlayerMovement();
    }

    private Vector3 StoInput()
    {
        Vector3 r = Vector3.zero;

        r.x = Input.GetAxisRaw("Horizontal");
        r.y = Input.GetAxisRaw("Vertical");
        
        //It will only normalize if my vector is > then 1
        return (r.magnitude > 1) ? r.normalized : r;
    }

    void PlayerMovement()
    {
        //Check what key is press and store it.
        Vector3 inputVector = StoInput();
        Vector3 moveVector = Vector3.zero;
        Vector3 direction = Vector3.zero;

        if (initialRot)
        {
            //Multiply inputs with speed (x,y,z)
            moveVector = new Vector3(inputVector.x * speedX, 0, inputVector.y * speedY);
            direction = new Vector3(inputVector.x, 0, inputVector.y).normalized;
            if (!controller.isGrounded)
            {
                moveVector += Physics.gravity;
            }
        }
        else
        {
            moveVector = new Vector3(inputVector.x * -speedX, 0, inputVector.y * -speedY);
            direction = new Vector3(-inputVector.x, 0, -inputVector.y).normalized;
        }
        controller.Move(moveVector * Time.deltaTime);        

        if (direction.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            controller.Move(direction * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RotatePlayer();
        }
    }

    void RotatePlayer()
    {
        if (initialRot)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        initialRot = !initialRot;
    }

}
