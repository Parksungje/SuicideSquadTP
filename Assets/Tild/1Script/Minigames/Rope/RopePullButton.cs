using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Tild._1Script.Minigames.Rope
{
    public class RopePullButton : MonoBehaviour
    {
        [SerializeField] private RectTransform _scaler;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private CanvasGroup _group;

        public Control currentControl;

        public void Initialize(Control control)
        {
            switch (control)
            {
                case Control.W:
                    _text.SetText("W");
                    break;
                case Control.A:
                    _text.SetText("A");
                    break;
                case Control.S:
                    _text.SetText("S");
                    break;
                case Control.D:
                    _text.SetText("D");
                    break;
                case Control.Left:
                    _text.SetText("\u2190");
                    break;
                case Control.Right:
                    _text.SetText("\u2192");
                    break;
                case Control.Up:
                    _text.SetText("\u2191");
                    break;
                case Control.Down:
                    _text.SetText("\u2193");
                    break;
            }
            currentControl = control;
            
        }

        public void Scaling()
        {
            if (_scaler == null) return;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
            _scaler.DOSizeDelta(new Vector2(250f, 300f), 0.15f)
                .SetEase(Ease.OutQuad);
            _scaler.DOScale(0.9f, 0.15f);
            _image.DOColor(Color.white, 0.1f);
        }
        public void UnCorrect()
        {
            _image.DOColor(Color.red, 0.1f); 
            _image.DOColor(Color.white, 0.1f).SetDelay(1f);
        }

        public void Disappear()
        {
            _group.DOFade(0, 0.1f);
            _scaler.DOSizeDelta(new Vector2(0f, 300f), 0.1f).OnComplete(() =>
            {
                
                Destroy(gameObject);
            });
        }
        
    }
}