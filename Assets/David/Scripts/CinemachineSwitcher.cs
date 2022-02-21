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

    // Update is called once per frame
    void Update()
    {
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
