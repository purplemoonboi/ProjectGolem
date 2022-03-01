using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @brief Builder camera needs to fire a ray at the grid and query which cell was hit.

public class BuilderCamera : MonoBehaviour
{

    [SerializeField]
    private float cameraSpeed = 2.0f;

    // Start is called before the first frame update
    public void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        LookAround();
    }

    public void FixedUpdate()
    {

    }

  

    void LookAround()
    {
        if(Input.GetMouseButton(1))
        {
            DebugCamera();
            //Convert to screen space.
            Vector3 mouseCoordinatesScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            //Remap from [0,1] to [-1,1]
            mouseCoordinatesScreen.x = RemapRange(mouseCoordinatesScreen.x, 0, 1, -1, 1);
            mouseCoordinatesScreen.y = RemapRange(mouseCoordinatesScreen.y, 0, 1, -1, 1);

            float angle = cameraSpeed * Time.deltaTime;

            //Convert to radians.
            angle = (angle / 180.0f) * 3.1415f;
            transform.forward = Vector3.RotateTowards(transform.forward, mouseCoordinatesScreen, angle, 2.0f);
        }

        //Clamp rotations.
        Mathf.Clamp(transform.rotation.eulerAngles.z, -45.0f, 45.0f);
        Mathf.Clamp(transform.rotation.eulerAngles.y, -25.0f, 25.0f);
    }

    private void DebugCamera()
    {
        Vector3 mouseCoordinatesScreen = Input.mousePosition;
        Debug.Log("Mouse position : " + mouseCoordinatesScreen);

        //Convert to screen space.
        mouseCoordinatesScreen = Camera.main.ScreenToViewportPoint(mouseCoordinatesScreen);

        //Remap from [0,1] to [-1,1]
        mouseCoordinatesScreen.x = RemapRange(mouseCoordinatesScreen.x, 0, 1, -1, 1);
        mouseCoordinatesScreen.y = RemapRange(mouseCoordinatesScreen.y, 0, 1, -1, 1);

        Debug.Log("Mouse position screen : " + mouseCoordinatesScreen);
        Debug.Log("Camera Position : " + transform.position);
    }

    // @brief Remaps 't' in the range of [a,b] to range [c,d].
    float RemapRange(float t, float a, float b, float c, float d)
    {
        //Inline
        return ((t - a) / (b - a)) * (d - c) + c;
    }

}
