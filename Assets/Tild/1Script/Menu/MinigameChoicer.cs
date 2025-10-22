using System;
using System.Collections.Generic;
using DG.Tweening;
using Tild._1Script.Minigames.Rope;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

namespace Tild.Menu
{
    public class MinigameChoicer : MonoBehaviour
    {
        
        public List<MinigameSO> minigame;
        
        #region Minigame Info Resources
      
        [SerializeField] private MenuInputSO inputSO;
   
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text minigameName;
        [SerializeField] private TMP_Text minigameDesc;
        [SerializeField] private Image minigameImage;
        [SerializeField] private Image minigameBackground;
        [SerializeField] private TMP_Text spaceText;
        
      
        #endregion
        #region Random Minigame Resources
        [SerializeField] private RectTransform scroller;
        [SerializeField] private TMP_Text namePrefab;
        [SerializeField] private CanvasGroup finalMinigame;
        [SerializeField] private TMP_Text finalMinigameName;
        [SerializeField] private Image fadeImage;
        [SerializeField] private GameObject info;
        private AnimationCurve rouletteCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 3f),  
            new Keyframe(0.6f, 0.8f, 0.5f, 0.5f),
            new Keyframe(1f, 1f, 0f, 0f)    
        );

        #endregion

        #region Minigame Select Resources

        [SerializeField] private GameObject minigameSelectWindow;
        [SerializeField] private Transform minigameBtnsParent;
        [SerializeField] private GameObject minigameBtnPrefab;
        #endregion
        
        private MinigameSO currentMinigame;

        private void Start()
        {
            ScreenManager.instance.FadeOut(0.3f,0, (() =>
            {
                if (MinigameManager.instance.isRandomMode)
                {
                    PlayRandomMinigame();
                }
                else
                {
                    ScreenManager.instance.FadeIn(0.3f,(() =>
                    {
                        minigameSelectWindow.SetActive(true);
                        ScreenManager.instance.FadeOut(0.3f, 1f, null);
                    }));
                }
            }));
          
        }

        private void PlayRandomMinigame()
        {
            int temp = 0;
            for (int i = 0; i < 70; i++)
            {
                temp += 1;
                if (temp >= minigame.Count) temp = 0;
                Instantiate(namePrefab, scroller.transform).SetText(minigame[temp].gameName);
            }

            int rand = Random.Range(0, minigame.Count);
            MinigameManager.instance.minigamePlayed.Add(minigame[rand]);

            currentMinigame = minigame[rand];

            Instantiate(namePrefab, scroller.transform).SetText(currentMinigame.gameName);
            finalMinigameName.text = currentMinigame.gameName;
            
            scroller.DOAnchorPosY(-9636f, 2f).SetEase(Ease.InBounce).SetDelay(2).OnComplete(() =>
            {
                scroller.DOAnchorPosY(8565, 7f)
                    .SetEase(rouletteCurve)
                    .OnComplete(() =>
                    {
                        finalMinigame.DOFade(1f, 0.3f).SetDelay(1f);
                        finalMinigame.transform
                            .DOScale(Vector3.one, 0.5f)
                            .SetEase(Ease.OutBack)
                            .SetDelay(1f);
                        finalMinigameName.transform
                            .DOScale(new Vector3(1, 0.75f, 1), 0.2f)
                            .SetEase(Ease.OutQuad)
                            .SetDelay(1.1f).OnComplete(() =>
                            {
                                ScreenManager.instance.FadeIn(0.3f, () =>
                                {
                                    info.SetActive(true);
                                    ViewInfo();
                                    ScreenManager.instance.FadeOut(0.3f, 4, null);
                                });
                            });
                    });
            });

        }
        
        private void ViewInfo()
        {
            minigameName.text = currentMinigame.gameName;
            minigameDesc.text = currentMinigame.description;
            minigameImage.sprite = currentMinigame.playScreen;
            minigameBackground.color = currentMinigame.backgroundColor;
            spaceText.color = currentMinigame.backgroundColor;
            inputSO.OnConfirmPressed += OnConfirmPressed;
        }

        private void OnConfirmPressed(bool obj)
        {
            MinigameManager.instance.NextMinigame(currentMinigame.scene);
        }

        private void OnDisable()
        {
            inputSO.OnConfirmPressed -= OnConfirmPressed;
        }
    }
}