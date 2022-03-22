using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{

    public GameObject trigger;
    public GameObject player;
    public GameObject cameraGamebject;
    public GameObject newParent;
    public GameObject mainParent;
    public float speed = 1.5f;

    float counter = 10f;
    bool isActive = false;
    bool timeStart = false;

    private void Update()
    {
        if (isActive)
        {
            StartCoroutine(newPosition());
            timeStart = true;
        }
        if (timeStart)
            counter -= Time.deltaTime;

        if (counter <= 0.0f)
        {
            trigger.SetActive(false);
            timeStart = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isActive = true;
        }
    }

    public IEnumerator newPosition()
    {
        cameraGamebject.transform.SetParent(newParent.transform);
        Vector3 desiredPosition = newParent.transform.position;
        Vector3 newPosition = Vector3.Lerp(cameraGamebject.transform.position, desiredPosition, speed * Time.deltaTime);
        cameraGamebject.transform.position = newPosition;
        yield return new WaitForSeconds(5f);
        cameraGamebject.transform.SetParent(mainParent.transform);
        Vector3 previousPos = mainParent.transform.position;
        Vector3 pos = Vector3.Lerp(cameraGamebject.transform.position, previousPos, speed * Time.deltaTime);
        cameraGamebject.transform.position = pos;
        isActive = false;
    }
}
