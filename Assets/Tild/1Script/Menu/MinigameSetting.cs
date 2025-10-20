using System;
using Tild._1Script.Minigames.Rope;
using TMPro;
using UnityEngine;

namespace Tild._1Script.Menu
{
    public class MinigameSetting : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown gameTypeDropdown;
        [SerializeField] private TMP_Dropdown isRandomModeDropdown;
        [SerializeField] private TMP_Dropdown amountDropdown;
        [SerializeField] private TMP_Text amountTitle;
        [SerializeField] private InputSO inputSO;

        private void Awake()
        {
            gameTypeDropdown.onValueChanged.AddListener(ChangeAmountTitle);
            inputSO.OnConfirmPressed += OnConfirmPressed;
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
            GameType gameType = (GameType)gameTypeDropdown.value;
            bool isRandomMode = (isRandomModeDropdown.value == 0);
            
            MinigameManager.instance.Initialize(gameType, isRandomMode);
        }
        
        
    }
}