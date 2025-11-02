using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tild.MiscUI
{
    public class ResultUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup resultGroup;
        [SerializeField] private TMP_Text finishText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private RectTransform barOffset;
        [SerializeField] private CanvasGroup barGroup;
      
        private Action currentAction;
        private int _1PScore;
        private int _2PScore;
        private bool _is1PWon;
      

        string originalScoreText;

        public void ViewResult(int _1PScore, int _2PScore, bool _is1PWon, System.Action _finishAction)
        {
            this._1PScore = _1PScore;
            this._2PScore = _2PScore;
            this._is1PWon = _is1PWon;
            
            currentAction = _finishAction;
            
            if (string.IsNullOrEmpty(originalScoreText))
                originalScoreText = scoreText.text;

    
            scoreText.text = originalScoreText.Replace("{1P}", _1PScore.ToString())
                .Replace("{2P}", _2PScore.ToString());

            Sequence sequence = DOTween.Sequence().SetUpdate(true);
            SoundManager.Instance.Play("ResultScreen");
            sequence.Append(resultGroup.DOFade(1, 0.2f));
            sequence.Join(finishText.DOFade(1, 0.2f).SetDelay(0.3f).SetEase(Ease.InQuart));
            sequence.Join(finishText.transform.DOScale(Vector3.one, 0.2f).SetDelay(0.3f).SetEase(Ease.InQuart));
            sequence.Append(barGroup.DOFade(1, 0.3f).SetDelay(3));
            sequence.Append(barOffset.DOSizeDelta(new Vector2(barOffset.sizeDelta.x, 0f), 0.3f).SetEase(Ease.InExpo));
            sequence.Join(finishText.DOFade(0, 0.2f).SetEase(Ease.OutQuart));
            sequence.Join(finishText.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutQuart));
            sequence.Append(barOffset.DOSizeDelta(new Vector2(barOffset.sizeDelta.x, 600f), 0.5f).SetDelay(0.3f).SetEase(Ease.OutExpo));
            sequence.Join(scoreText.transform.DOScale(new Vector3(1.3f,1,1), 0.3f).SetEase(Ease.InQuart));
            sequence.Join(titleText.transform.DOScale(new Vector3(1.3f,1,1), 0.3f).SetEase(Ease.InQuart));
            sequence.Join(scoreText.DOFade(1, 0.3f).SetEase(Ease.InQuart));
            sequence.Join(titleText.DOFade(1, 0.3f).SetEase(Ease.InQuart));

            sequence.OnComplete(() =>
            {
                StartCoroutine(UnscaledInvokeRoutine());
            });

         
        }
        private IEnumerator UnscaledInvokeRoutine()
        {
            yield return new WaitForSecondsRealtime(2f);
            DelayScoring();

            yield return new WaitForSecondsRealtime(4.5f);
            DelayInvoke();

            yield return new WaitForSecondsRealtime(4.6f);
            ResetValues();
        }

        public void DelayScoring()
        {
            if (_is1PWon)
            {
                _1PScore++;
            }

            else
            {
                _2PScore++;
            }
            SoundManager.Instance.Play("ShowScore");
            scoreText.text = originalScoreText.Replace("{1P}", _1PScore.ToString())
                .Replace("{2P}", _2PScore.ToString());
        }
        public void DelayInvoke()
        {
            currentAction.Invoke();
        }
      

        private void ResetValues()
        {
            resultGroup.alpha = 0;
            finishText.transform.localScale = Vector3.zero;
            scoreText.transform.localScale = Vector3.zero;
            scoreText.alpha = 0;
            titleText.transform.localScale = Vector3.zero;
            titleText.alpha = 0;
            barOffset.sizeDelta = new Vector2(barOffset.sizeDelta.x, 600);
            barGroup.alpha = 0;
            
        }
    }
}