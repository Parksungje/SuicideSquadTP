using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Tild._1Script.Menu;
using Tild._1Script.Minigames.Rope;
using Tild.Core;
using Tild.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Tild._1Script.Menu
{
    public class MinigameChoicer : MonoBehaviour
    {
        public List<MinigameSO> minigame;
        [SerializeField] private MenuInputSO inputSO;
        private bool OnPopuped = false;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text minigameName;
        [SerializeField] private TMP_Text minigameDesc;
        [SerializeField] private Image minigameImage;
        [SerializeField] private Image minigameBackground;
        [SerializeField] private TMP_Text spaceText;

        public bool IsPopuped;

        [SerializeField] private GameObject ranndomMinigameWindow;
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

        [SerializeField] private GameObject minigameSelectWindow;
        [SerializeField] private Transform minigameBtnsParent;
        [SerializeField] private MinigameSelectButton minigameBtnPrefab;

        private MinigameSO currentMinigame;
        private List<MinigameSO> _available = new List<MinigameSO>();

        private void Awake()
        {
            GameEventBus.AddListener<OnMinigameBtnClicked>(SelectedMinigame);
        }

        private void SelectedMinigame(OnMinigameBtnClicked obj)
        {
            ScreenManager.instance.FadeIn(1, (() =>
            {
                info.SetActive(true);
                ViewInfo(obj.Minigame);
                minigameSelectWindow.SetActive(false);
                IsPopuped = true;
                ScreenManager.instance.FadeOut(1, 2, () => { });
            }));
        }

        private void Start()
        {
            _available = minigame.Where(m => !MinigameManager.instance.minigamePlayed.Contains(m)).ToList();

            if (MinigameManager.instance.isRandomMode)
            {
                ranndomMinigameWindow.SetActive(true);
                PlayRandomMinigame();
            }
            else
            {
                minigameSelectWindow.SetActive(true);
                foreach (Transform c in minigameBtnsParent) Destroy(c.gameObject);
                foreach (var mg in _available)
                {
                    MinigameSelectButton btn = Instantiate(minigameBtnPrefab, minigameBtnsParent);
                    btn.Initialize(mg);
                }
            }
        }

        private void PlayRandomMinigame()
        {
            _available = minigame.Where(m => !MinigameManager.instance.minigamePlayed.Contains(m)).ToList();
            foreach (Transform c in scroller.transform) Destroy(c.gameObject);
            if (_available.Count == 0) return;

            int temp = 0;
            for (int i = 0; i < 70; i++)
            {
                temp += 1;
                if (temp >= _available.Count) temp = 0;
                Instantiate(namePrefab, scroller.transform).SetText(_available[temp].gameName);
            }

            int rand = Random.Range(0, _available.Count);
            MinigameManager.instance.minigamePlayed.Add(_available[rand]);
            currentMinigame = _available[rand];

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
                                ScreenManager.instance.FadeIn(3f, () =>
                                {
                                    info.SetActive(true);
                                    ViewInfo(null);
                                    IsPopuped = true;
                                    ScreenManager.instance.FadeOut(0.3f, 4, null);
                                });
                            });
                    });
            });
        }

        private void ViewInfo(MinigameSO minigameSO)
        {
            if (minigameSO != null)
                currentMinigame = minigameSO;

            minigameName.text = currentMinigame.gameName;
            minigameDesc.text = currentMinigame.description;
            minigameImage.sprite = currentMinigame.playScreen;
            minigameBackground.color = currentMinigame.backgroundColor;
            spaceText.color = currentMinigame.backgroundColor;
        }

        private void OnEnable()
        {
            inputSO.OnSpacePressed += OnConfirmPressed;
        }

        private void OnConfirmPressed(bool obj)
        {
            if (IsPopuped)
            {
                MinigameManager.instance.NextMinigame(currentMinigame.scene);
                inputSO.OnSpacePressed -= OnConfirmPressed;
            }
        }

        private void OnDestroy()
        {
            inputSO.OnSpacePressed -= OnConfirmPressed;
        }
    }
}
