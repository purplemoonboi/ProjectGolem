using UnityEngine;

public class ChangeTarget : MonoBehaviour
{
    private Camera cam;

    public Transform target;



    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            cam.transform.LookAt(target.position);
            Debug.Log(target.name);
        }
    }
}
