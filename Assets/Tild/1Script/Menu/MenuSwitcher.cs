

using UnityEngine;
using Unity.Cinemachine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera camA;
    [SerializeField] private CinemachineCamera camB;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchTo(camA);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchTo(camB);
        }
    }

    void SwitchTo(CinemachineCamera targetCam)
    {
        camA.Priority = (targetCam == camA) ? 20 : 10;
        camB.Priority = (targetCam == camB) ? 20 : 10;
    }
}

