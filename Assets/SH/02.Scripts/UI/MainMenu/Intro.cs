using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Intro : MonoBehaviour
{
    private CanvasGroup fade;
    [SerializeField] private CanvasGroup team;

    private void Start()
    {
        fade = GetComponent<CanvasGroup>();
        team.DOFade(1, 1).SetDelay(3).OnComplete(()=>
        {
            team.DOFade(0, 2).SetDelay(1);
            fade.DOFade(0, 1).SetDelay(3).OnComplete(()=> fade.blocksRaycasts = false);
        });
    }
}