using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tild.Minigames.BalanceGame
{
    public class ChatBubble : MonoBehaviour
    {
        [SerializeField] private Image bubble;
        [SerializeField] private TMP_Text chatText;
        private float _duration;

        public async void PopupBubble(string text, float duration)
        {
            
            bubble.transform.DOScale(Vector3.one, 0.7f).SetEase(Ease.InBounce).OnComplete(() =>
            {
                chatText.DOFade(1, 0.3f);
                chatText.SetText(text);
            });
            await Awaitable.WaitForSecondsAsync(duration);
            bubble.transform.DOScale(Vector3.zero, 0.7f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                chatText.DOFade(1, 0.3f);
                chatText.SetText(text);
            });
        }

    }
}