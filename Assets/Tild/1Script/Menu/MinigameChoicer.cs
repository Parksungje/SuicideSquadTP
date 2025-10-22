using System;
using System.Collections.Generic;
using Tild._1Script.Minigames.Rope;
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
        private MinigameSO currentMinigame;

        private void Start()
        {
            int rand = Random.Range(0, minigame.Count);
            MinigameManager.instance.minigamePlayed.Add(minigame[rand]);
            
            currentMinigame = minigame[rand];
            minigameName.text = currentMinigame.gameName;
            minigameDesc.text = currentMinigame.description;
            minigameImage.sprite = currentMinigame.playScreen;
            minigameBackground.color = currentMinigame.backgroundColor;
            spaceText.color = currentMinigame.backgroundColor;
        }

        private void OnEnable()
        {
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