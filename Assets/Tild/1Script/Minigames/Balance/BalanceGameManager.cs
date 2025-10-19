using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tild.Minigames.BalanceGame
{
    public class BalanceGameManager : MonoBehaviour
    {
        [SerializeField] private BalanceGameInputSO balanceGameInputSO;
        [SerializeField] private BalanceQuestionSO balanceQuestionSO;
        
        private List<Question> questions;

        [SerializeField] private TMP_Text questionLeft;
        [SerializeField] private TMP_Text questionRight;
        [SerializeField] private TMP_Text answerLeft;
        [SerializeField] private TMP_Text answerRight;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private Image checkedLeft;
        [SerializeField] private Image checkedRight;
        [SerializeField] private CanvasGroup controlGuide;
        [SerializeField] private CanvasGroup questionGroup;
       
        
        private int roundCount = 5;
        private int waitTime = 5;
        private bool _isPlaying;
        private bool _1PLeft;
        private bool _1PRight;
        private bool _2PLeft;
        private bool _2PRight;
        private void OnEnable()
        {
            balanceGameInputSO.AKeyPressed = () =>
            {
                
                if (!_isPlaying) return;
                checkedLeft.DOFade(1, 1f);
                    _1PLeft = true;
                    _1PRight = false;
            };
            balanceGameInputSO.DKeyPressed = () =>
            {
                if (!_isPlaying) return;
                checkedLeft.DOFade(1, 1f);
                _1PLeft = false;
                _1PRight = true;
            };
            balanceGameInputSO.LeftKeyPressed = () =>
            {
                if (!_isPlaying) return;
                checkedRight.DOFade(1, 1f);
                _2PLeft = true;
                _2PRight = false;
            };
            balanceGameInputSO.RightKeyPressed = () =>
            {
                if (!_isPlaying) return;
                checkedRight.DOFade(1, 1f);
                _2PLeft = false;
                _2PRight = true;
            };
        }

        private void Awake()
        {
            questions = balanceQuestionSO.GetRandomQuestions(roundCount);
        }

        IEnumerator Start()
        {
            WaitForSeconds oneSecond = new WaitForSeconds(1);
            WaitForSeconds notificationTime = new WaitForSeconds(3);
            for (int i = 0; i < roundCount; i++)
            {
             
                notificationText.DOFade(1, 0.5f);
                Question question = questions[i];
                notificationText.SetText("3초 뒤에\n문제가\n등장합니다.");
                notificationText.SetText("");
                yield return notificationTime;
                notificationText.DOFade(0, 0.5f);
                questionGroup.DOFade(1f, 0.3f);
                controlGuide.DOFade(1, 0.5f);
                _isPlaying = true;
                questionLeft.SetText(question.left);
                questionRight.SetText(question.right);
                
                
                
                for (int time = waitTime; time > 0; time--)
                {
                    timerText.SetText(time.ToString());
                    yield return oneSecond;
                }
                questionGroup.DOFade(0f, 0.3f);
                controlGuide.DOFade(0, 0.5f);
                notificationText.DOFade(1, 0.5f);
                if (_1PLeft == _2PLeft && _1PRight == _2PRight)
                {
                    notificationText.SetText("성공!\n<size=70>같은 답을 선택했어요!");
                }
                else
                {
                    notificationText.SetText("실패!\n<size=70>다른 답을 선택했어요.");
                }
                
                yield return notificationTime;
                notificationText.DOFade(0, 0.5f);
                _isPlaying = false;
                
                checkedLeft.DOFade(0, 1f);
                checkedRight.DOFade(0, 1f);
                
                _1PLeft = false;
                _1PRight = false;
                _2PLeft = false;
                _2PRight = false;
                
            }
        }
    }
}