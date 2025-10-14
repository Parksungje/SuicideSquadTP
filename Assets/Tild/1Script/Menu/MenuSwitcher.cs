

using DG.Tweening;
using UnityEngine;
using Unity.Cinemachine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineCamera camA;
    [SerializeField] private CinemachineCamera camB;
    [SerializeField] private CanvasGroup commonBlockGroup;

    private int _groupAlpha;
    private float _waitTime;

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
        
        _groupAlpha = (targetCam == camB) ? 1 : 0;
        _waitTime = (targetCam == camB) ? 1.2f : 0;

        commonBlockGroup.DOFade(_groupAlpha, 0.3f).SetDelay(_waitTime);

    }
}

