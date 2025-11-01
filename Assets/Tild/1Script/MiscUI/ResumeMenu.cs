using System;
using DG.Tweening;
using Tild.Menu;
using UnityEngine;

namespace Tild.MiscUI
{
    public class ResumeMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        bool isOpen = false;
        bool canOpen = true;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && canOpen)
            {
                if (!isOpen)
                {
                    isOpen = !isOpen;
                    canOpen = false; 
                    canvasGroup.DOFade(1f, 0.5f).SetUpdate(true).OnComplete((() =>
                    {
                        canvasGroup.blocksRaycasts = true;
                        canOpen = true;
                        Time.timeScale = 0f;
                    }));
                }
                else
                {
                    isOpen = !isOpen;
                    canOpen = false; 
                    canvasGroup.DOFade(0f, 0.5f).SetUpdate(true).OnComplete((() =>
                    {
                        canvasGroup.blocksRaycasts = true;
                        canOpen = true;
                        Time.timeScale = 0f;
                    }));
                }

            }
        }

        public void Resume()
        {
            print("Resume");
            canvasGroup.DOFade(0f, 0.5f).SetUpdate(true).OnComplete((() =>
            {
                canvasGroup.blocksRaycasts = false;
                Time.timeScale = 1f;
            }));
        }
        public void ReMinigame()
        {
            print("Reminigame");
            if (MinigameManager.instance.gameType == null) return;
            
            canvasGroup.DOFade(0f, 0.5f).SetUpdate(true).OnComplete((() =>
            {
                canvasGroup.blocksRaycasts = false;
                Time.timeScale = 1f;
            }));
            
            
                canvasGroup.DOFade(0f, 0.5f).SetUpdate(true).OnComplete((() =>
                {
                Time.timeScale = 1f;
                TransitionManager.Go("Choice_Scene");
            }));
          
        }
        public void GoHome()
        {
            print("GoHome");
            canvasGroup.DOFade(0f, 0.5f).SetUpdate(true).OnComplete((() =>
            {
                Time.timeScale = 1f;
                TransitionManager.Go("Start_Scene");
            }));
        }
    }
}