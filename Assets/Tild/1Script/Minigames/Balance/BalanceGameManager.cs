using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tild.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Tild.Minigames.BalanceGame
{
    public class BalanceGameManager : MonoBehaviour
    {
        [SerializeField] private BalanceGameInputSO balanceGameInputSO;
        [SerializeField] private BalanceQuestionSO balanceQuestionSO;

        [SerializeField] private TMP_Text questionLeft;
        [SerializeField] private TMP_Text questionRight;
        [SerializeField] private TMP_Text answerLeft;
        [SerializeField] private TMP_Text answerRight;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private TMP_Text notificationText;
        [SerializeField] private CanvasGroup notificationGroup;
        [SerializeField] private Image checkedLeft;
        [SerializeField] private Image checkedRight;
        [SerializeField] private CanvasGroup controlGuide;
        [SerializeField] private CanvasGroup questionGroup;

        [SerializeField] private int roundCount = 10;
        [SerializeField] private int chooseTime = 10;
        [SerializeField] private int guessTime = 10;
        [SerializeField] private float fadeDur = 0.2f;
        [SerializeField] private float resultHold = 3.0f;

        [SerializeField] private ParticleSystem correctParticle1P;
        [SerializeField] private ParticleSystem correctParticle2P;
        [SerializeField] private ParticleSystem celebParticle1P;
        [SerializeField] private ParticleSystem celebParticle2P;
        [SerializeField] private GameObject celebCamera1P;
        [SerializeField] private GameObject celebCamera2P;
        
        [SerializeField] private CanvasGroup keyGuide;
        [SerializeField] private Image guideBackground;
        [SerializeField] private TMP_Text infoText;

        private List<Question> questions;

        private enum Phase { Idle, ShowQuestion, Choose, Guess, Result }
        private Phase phase = Phase.Idle;

        private bool _1PChoiceLeft;
        private bool _2PChoiceLeft;
        private bool _1PGuessLeft;
        private bool _2PGuessLeft;

        private int _score1P = 0;
        private int _score2P = 0;
        

        private void OnEnable()
        {
            balanceGameInputSO.AKeyPressed = () =>
            {
                if (phase == Phase.Choose)
                {
                    _1PChoiceLeft = true;
                    checkedLeft.DOFade(1f, 0.2f);
                }
                else if (phase == Phase.Guess)
                {
                    _1PGuessLeft = true;
                    checkedLeft.DOFade(1f, 0.2f);
                }
            };

            balanceGameInputSO.DKeyPressed = () =>
            {
                if (phase == Phase.Choose)
                {
                    _1PChoiceLeft = false;
                    checkedLeft.DOFade(1f, 0.2f);
                }
                else if (phase == Phase.Guess)
                {
                    _1PGuessLeft = false;
                    checkedLeft.DOFade(1f, 0.2f);
                }
            };

            balanceGameInputSO.LeftKeyPressed = () =>
            {
                if (phase == Phase.Choose)
                {
                    _2PChoiceLeft = true;
                    checkedRight.DOFade(1f, 0.2f);
                }
                else if (phase == Phase.Guess)
                {
                    _2PGuessLeft = true;
                    checkedRight.DOFade(1f, 0.2f);
                }
            };

            balanceGameInputSO.RightKeyPressed = () =>
            {
                if (phase == Phase.Choose)
                {
                    _2PChoiceLeft = false;
                    checkedRight.DOFade(1f, 0.2f);
                }
                else if (phase == Phase.Guess)
                {
                    _2PGuessLeft = false;
                    checkedRight.DOFade(1f, 0.2f);
                }
            };
        }

        private void Awake()
        {
            questions = balanceQuestionSO.GetRandomQuestions(roundCount);
        }

        private int questionIndex = 0;
        private IEnumerator Start()
        {
            SoundManager.Instance.Play("Balance_BGM");

            keyGuide.DOFade(1, 0.5f);
            yield return new WaitForSeconds(4f);
            keyGuide.DOFade(0, 0.5f);
            yield return new WaitForSeconds(1f);
            infoText.DOFade(1, 0.5f);
            yield return new WaitForSeconds(3.5f);
            infoText.DOFade(0, 0.5f);
            guideBackground.DOFade(0, 0.5f);
            
            var oneSec = new WaitForSeconds(1f);

            for (int i = 0; i < roundCount; i++)
            {
                phase = Phase.ShowQuestion;
                ResetRoundVisuals();
                
                var q = questions[questionIndex];
                questionIndex++;
                yield return new WaitForSeconds(1f);
                notificationText.SetText("문제 등장!");
               
                notificationGroup.DOFade(1f, fadeDur); SoundManager.Instance.Play("Balance_Bell");
                yield return new WaitForSeconds(0.8f);
                notificationGroup.DOFade(0f, fadeDur);

                questionLeft.SetText(q.left);
                questionRight.SetText(q.right);
                questionGroup.DOFade(1f, 0.3f);
                controlGuide.DOFade(1f, 0.3f);

                phase = Phase.Choose;
                
                notificationText.SetText("<size=70>정답 선택!");
                notificationGroup.DOFade(1f, fadeDur); SoundManager.Instance.Play("Balance_Bell");

                for (int t = chooseTime; t > 0; t--)
                {
                    timerText.SetText(t.ToString());
                    yield return oneSec;
                }
                notificationGroup.DOFade(0f, fadeDur);

                phase = Phase.Guess;
                checkedLeft.DOFade(0f, 0.2f);
                checkedRight.DOFade(0f, 0.2f);

                notificationText.SetText("<size=70>상대의 선택을 맞춰보세요!</size>");
                notificationGroup.DOFade(1f, fadeDur); SoundManager.Instance.Play("Balance_Bell");

                for (int t = guessTime; t > 0; t--)
                {
                    timerText.SetText(t.ToString());
                    yield return oneSec;
                }
                notificationGroup.DOFade(0f, fadeDur);

                phase = Phase.Result;

                bool p1Correct = (_1PGuessLeft == _2PChoiceLeft);
                bool p2Correct = (_2PGuessLeft == _1PChoiceLeft);
                bool bothCorrect = p1Correct && p2Correct;

                questionGroup.DOFade(0f, 0.3f);
                controlGuide.DOFade(0f, 0.3f);

                if (bothCorrect)
                {
                    _score1P++;
                    _score2P++;
                    SoundManager.Instance.Play("Balance_Correct");
                    correctParticle1P.Play();
                    correctParticle2P.Play();
                    notificationText.SetText($"{_score1P} : {_score2P}\n<size=70>두 플레이어 모두 상대의 선택을 정확히 맞췄어요!</size>");
                }
                else if (p1Correct)
                {
                    SoundManager.Instance.Play("Balance_Correct");
                    correctParticle1P.Play();
                    _score1P++;
                    notificationText.SetText($"{_score1P} : {_score2P}\n<size=70>1P가 상대의 선택을 맞췄어요.</size>");
                }
                else if (p2Correct)
                {
                    SoundManager.Instance.Play("Balance_Correct");
                    correctParticle2P.Play();
                    _score2P++;
                    notificationText.SetText($"{_score1P} : {_score2P}\n<size=70>2P가 상대의 선택을 맞췄어요.</size>");
                }
                else
                {
                    SoundManager.Instance.Play("Balance_NoneCorrect");
                    notificationText.SetText($"{_score1P} : {_score2P}\n<size=70>둘 다 상대의 선택을 틀렸어요.</size>");
                }

                notificationGroup.DOFade(1f, fadeDur); SoundManager.Instance.Play("Balance_Bell");
               
                yield return new WaitForSeconds(resultHold);
                if (_score1P == 5)
                {
                    celebParticle1P.Play();
                    celebCamera1P.SetActive(true);
                    notificationText.SetText("1P 승리!!");
                    yield return new WaitForSeconds(3);
                    SoundManager.Instance.Stop("Balance_BGM");

                    MinigameManager.instance.Finish(true);
                   
                }
                else if (_score2P == 5)
                {
                    celebParticle2P.Play();
                    celebCamera2P.SetActive(true);
                    notificationText.SetText("2P 승리!!");
                    yield return new WaitForSeconds(3);
                    SoundManager.Instance.Stop("Balance_BGM");
                    MinigameManager.instance.Finish(false);
                }
                
                notificationGroup.DOFade(0f, fadeDur);

                ClearFlags();
            }

            phase = Phase.Idle;
            
            notificationGroup.DOFade(1f, fadeDur); SoundManager.Instance.Play("Balance_Bell");
        }

        private void ResetRoundVisuals()
        {
            timerText.SetText("");
            notificationGroup.DOFade(0f, 0f);
            questionGroup.DOFade(0f, 0f);
            controlGuide.DOFade(0f, 0f);
            checkedLeft.DOFade(0f, 0f);
            checkedRight.DOFade(0f, 0f);
            ClearFlags();
        }

        private void ClearFlags()
        {
            _1PChoiceLeft = false;
            _2PChoiceLeft = false;
            _1PGuessLeft = false;
            _2PGuessLeft = false;
        }
    }
}
