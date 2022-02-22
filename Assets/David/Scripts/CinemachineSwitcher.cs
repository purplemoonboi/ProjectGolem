using UnityEngine;
using Cinemachine;

//@author David Costa

public class CinemachineSwitcher : MonoBehaviour
{

    [SerializeField]
    private CinemachineVirtualCamera vCam1;
    [SerializeField]
    private CinemachineVirtualCamera vCam2;

    private bool frontCam = true;

    private GameObject target;
    public GameObject lookAt;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
        vCam1.Follow = target.transform;
        vCam1.LookAt = lookAt.transform;
        vCam2.Follow = target.transform;
        vCam2.LookAt = lookAt.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SwitchPriority();
        }
        
    }

    private void SwitchPriority()
    {
        if (frontCam)
        {
            vCam1.Priority = 0;
            vCam2.Priority = 1;
        }
        else
        {
            vCam1.Priority = 1;
            vCam2.Priority = 0;
        }
        frontCam = !frontCam;
    }
}
