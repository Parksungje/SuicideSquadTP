using System;
using System.Collections;
using DG.Tweening;
using Febucci.UI;
using Tild.Menu;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PushGameManager : MonoBehaviour
{
    public static PushGameManager instance;
    
    [SerializeField] private Rigidbody rigid1P;
    [SerializeField] private Rigidbody rigid2P;

    [SerializeField] private Transform point1P;
    [SerializeField] private Transform point2P;
    
    [SerializeField] private CinemachineCamera targetCamera;
    [SerializeField] private CinemachineCamera celebCamera;
    [SerializeField] private CanvasGroup keyGuideGroup;
    [SerializeField] private TMP_Text timeCountText;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text score;
    [SerializeField] private Image background;
    
    [SerializeField] private ParticleSystem celebParticle1P;
    [SerializeField] private ParticleSystem celebParticle2P;
    
    [SerializeField] private GroundShrink groundShrinker;
    [SerializeField] private Movement1Component movement1Component;
    [SerializeField] private Movement2Component movement2Component;

    private int _1PScore;
    private int _2PScore;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    IEnumerator Start()
    {
        SoundManager.Instance.Play("Push_BGM");

        DisableRigidbodies();

        keyGuideGroup.DOFade(1, 2);
        yield return new WaitForSeconds(5);

        keyGuideGroup.DOFade(0, 1);
        yield return new WaitForSeconds(1);

        infoText.DOFade(1, 0.5f);
        yield return new WaitForSeconds(3);

        infoText.DOFade(0, 0.5f);
        for (int i = 3; i > 0; i--)
        {
            timeCountText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        timeCountText.text = "";
        background.DOFade(0, 0.2f);
        EnableRigidbodies();
        groundShrinker.Shrink();
    }

    public void Falled(Rigidbody rigidbody)
    {
        bool is1Pwin = rigidbody != rigid1P;
        
        if (is1Pwin) _1PScore++;
        else _2PScore++;
        
        score.text = _1PScore.ToString() + " : " + _2PScore.ToString() ;
        
        StartCoroutine(Celebration(is1Pwin));
    }

    IEnumerator Celebration(bool is1Pwin)
    {
    

        targetCamera.gameObject.SetActive(false);
        celebCamera.gameObject.SetActive(true);
        celebCamera.Follow = is1Pwin ? rigid1P.transform : rigid2P.transform;
        
        if (is1Pwin)
        {
            celebParticle1P.Play();
            SoundManager.Instance.Play("Confetti");

        }
        else
        {
            celebParticle2P.Play();
            SoundManager.Instance.Play("Confetti");

        }

        yield return new WaitForSeconds(4);
        DisableRigidbodies();
        targetCamera.gameObject.SetActive(true);
        celebCamera.gameObject.SetActive(false);

        rigid1P.linearVelocity = Vector3.zero;
        rigid2P.linearVelocity = Vector3.zero;
        rigid1P.position = point1P.position;
        rigid2P.position = point2P.position;
        
        groundShrinker.Reset();

        background.DOFade(0.7f, 0.2f);
        timeCountText.text = is1Pwin ? "1P 승리!" : "2P 승리!";

        yield return new WaitForSeconds(1.5f);
        timeCountText.text = "";

        if (_1PScore == 3 || _2PScore == 3)
        {
            SoundManager.Instance.Stop("Push_BGM");
            MinigameManager.instance.Finish(is1Pwin);
            yield break;
        }

        for (int i = 3; i > 0; i--)
        {
            timeCountText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        movement1Component._isBeingPushed = false;
        movement2Component._isBeingPushed = false;
        
        timeCountText.text = "";
        background.DOFade(0, 0.2f);
        groundShrinker.Shrink();
        EnableRigidbodies();
    }

    private void DisableRigidbodies()
    {
        rigid1P.isKinematic = true;
        rigid2P.isKinematic = true;
        rigid1P.linearVelocity = Vector3.zero;
        rigid2P.linearVelocity = Vector3.zero;
        rigid1P.angularVelocity = Vector3.zero;
        rigid2P.angularVelocity = Vector3.zero;
    }

    private void EnableRigidbodies()
    {
        ResetRigidbody(rigid1P);
        ResetRigidbody(rigid2P);
    }

    private void ResetRigidbody(Rigidbody rb)
    {
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
    }
}
