using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Polishing_RussianRoulette : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private GameObject[] cameras;
    private Transform cameraParent;

    [Header("Animatiors")]
    [SerializeField] private Animator p1Animator;
    [SerializeField] private Animator p2Animator;

    [Header("Revolvers")]
    [SerializeField] private GameObject revolverP1;
    [SerializeField] private GameObject revolverP2;
    [SerializeField] private GameObject revolverCenter;

    [Header("Lights")]
    [SerializeField] private GameObject centerLight;
    [SerializeField] private Light gunLight;

    [Header("Elements")]
    [SerializeField] private Image fade;
    [SerializeField] private CanvasGroup ui;
    [SerializeField] private GameManager_Russian gameManager;

    private Sequence centerBlinkSeq;

    private void Start()
    {
        cameraParent = cameras[1].transform.parent;
        cameras[0].transform.DORotate(new Vector3(90f, 180f, 180f), 6f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            gameManager.GameStart();
            ShowAllUIs();
        });
    }

    public void DeactiveAllCameras()
    {
        foreach (Transform t in cameraParent.GetComponentsInChildren<Transform>(true))
        {
            if (t != cameraParent) t.gameObject.SetActive(false);
        }
    }

    public void SetCamera(int num)
    {
        DeactiveAllCameras();
        cameras[num].SetActive(true);
    }

    public void SetAnimatorHolding(int player)
    {
        revolverCenter.SetActive(false);
        if (player == 1)
        {
            revolverP1.SetActive(true);
            revolverP2.SetActive(false);
            p1Animator.SetBool("Holding", true);
            p2Animator.SetBool("Holding", false);
        }
        else
        {
            revolverP2.SetActive(true);
            revolverP1.SetActive(false);
            p2Animator.SetBool("Holding", true);
            p1Animator.SetBool("Holding", false);
        }
    }

    public void SetAnimatorScaring(int num)
    {
        if (num == 1)
        {
            p1Animator.SetBool("Scaring", true);
            p2Animator.SetBool("Scaring", false);
        }
        else
        {
            p2Animator.SetBool("Scaring", true);
            p1Animator.SetBool("Scaring", false);
        }
    }

    public void SetAnimatorFire(int num)
    {
        if (num == 1)
        {
            p1Animator.SetTrigger("Fire");
        }
        else
        {
            p2Animator.SetTrigger("Fire");
        }
    }

    public void SetAnimatorDeath(int num)
    {
        if (num == 1)
        {
            p1Animator.SetTrigger("Death");
        }
        else
        {
            p2Animator.SetTrigger("Death");
        }
    }

    public void SetAnimationToIdle()
    {
        p1Animator.SetTrigger("ToIdle");
        p2Animator.SetTrigger("ToIdle");
    }

    private void ShowAllUIs()
    {
        ui.DOFade(1, 1);
    }    

    public void GunLight()
    {
        gunLight.intensity = 3000;
        gunLight.DOIntensity(0, 1);
        centerLight.SetActive(false);
        fade.DOFade(1, 0).SetDelay(.2f);
        fade.DOFade(0, 1).SetDelay(3f).OnComplete(() => BlinkingCenterLight());
    }

    private void BlinkingCenterLight()
    {
        if (centerBlinkSeq != null && centerBlinkSeq.IsActive()) centerBlinkSeq.Kill();
        int flashes = 3;
        float onTime = 0.06f;
        float offTime = 0.06f;
        var go = centerLight;
        centerBlinkSeq = DOTween.Sequence().SetUpdate(true).SetAutoKill(true);
        for (int i = 0; i < flashes; i++)
        {
            centerBlinkSeq.AppendCallback(() => go.SetActive(true));
            centerBlinkSeq.AppendInterval(onTime);
            centerBlinkSeq.AppendCallback(() => go.SetActive(false));
            centerBlinkSeq.AppendInterval(offTime);
        }
        centerBlinkSeq.OnComplete(() => go.SetActive(true));
    }
}