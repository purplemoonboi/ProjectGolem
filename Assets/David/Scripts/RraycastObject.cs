using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RraycastObject : MonoBehaviour
{
    public Vector3 collision = Vector3.zero;
    public GameObject popUpMessage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.collider.gameObject.CompareTag("Missing Parts"))
            {
                popUpMessage.SetActive(true);
            }
            else
            {
                popUpMessage.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(collision, .2f);
    }
}
