using System;
using Tild._1Script.Minigames.Rope;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tild.Menu
{
    public class MinigameSetting : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown gameTypeDropdown;
        [SerializeField] private TMP_Dropdown isRandomModeDropdown;
        [SerializeField] private TMP_Dropdown amountDropdown;
        [SerializeField] private TMP_Text amountTitle;
        [SerializeField] private InputSO inputSO;
        [SerializeField] private GameObject cameras;
        private bool isEnded = false;

        private void Awake()
        {
            gameTypeDropdown.onValueChanged.AddListener(ChangeAmountTitle);
            inputSO.OnSpacePressed += OnConfirmPressed;
           
        }

        private void OnDisable()
        {
            inputSO.OnSpacePressed -= OnConfirmPressed;
        }

        private void ChangeAmountTitle(int index)
        {
            switch (index)
            {
                case 0:
                    amountTitle.text = "라운드 수";
                    break;
                case 1:
                    amountTitle.text = "승리 점수";
                    break;
                case 2:
                    amountTitle.text = "라운드 수";
                    break;
            }
        }

        public void OnConfirmPressed(bool a)
        {
            if (isEnded) return;
            
            isEnded = true;
                
            GameType gameType = (GameType)gameTypeDropdown.value;
            bool isRandomMode = (isRandomModeDropdown.value == 0);
            print(isRandomMode);
            int amount = 3;
            switch (amountDropdown.value)
            {
                case 0:
                    amount = 3;
                    break;
                case 1:
                    amount = 5;
                    break;
                case 2:
                    amount = 7;
                    break;
                case 3:
                    amount = 9;
                    break;  
            }
            SoundManager.Instance.Stop("GameSetting");
            MinigameManager.instance.Initialize(gameType, isRandomMode, amount);
            SoundManager.Instance.Play("StartScene");
            TransitionManager.Go("Choice_Scene");
           
        
        }
        
        
    }
}