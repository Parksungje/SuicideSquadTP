using System;
using System.Collections;
using DG.Tweening;
using Febucci.UI;
using Tild.Menu;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FallingGameManager : MonoBehaviour
{
    public static FallingGameManager instance;
    
    [SerializeField] private Rigidbody rigid1P;
    [SerializeField] private Rigidbody rigid2P;
    
    [SerializeField] private CinemachineCamera targetCamera;
    [SerializeField] private CinemachineCamera celebCamera;
    [SerializeField] private CanvasGroup keyGuideGroup;
    [SerializeField] private TMP_Text timeCountText;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text score;
    [SerializeField] private Image background;
    
    [SerializeField] private ParticleSystem celebParticle1P;
    [SerializeField] private ParticleSystem celebParticle2P;
    
    
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    IEnumerator Start()
    {
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
    }

    public void Falled(Rigidbody rigidbody)
    {
       
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody != null && other.gameObject.CompareTag("Player"))
        {
            bool is1Pwin = other.rigidbody != rigid1P;
        
            StartCoroutine(Celebration(is1Pwin));
        }
    }

    IEnumerator Celebration(bool is1Pwin)
    {
    

        targetCamera.gameObject.SetActive(false);
        celebCamera.gameObject.SetActive(true);
        celebCamera.Follow = is1Pwin ? rigid1P.transform : rigid2P.transform;
        
        if (is1Pwin)
            celebParticle1P.Play();
        else
            celebParticle2P.Play();

        yield return new WaitForSeconds(3);
        
        MinigameManager.instance.Finish(is1Pwin);
        
       
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
