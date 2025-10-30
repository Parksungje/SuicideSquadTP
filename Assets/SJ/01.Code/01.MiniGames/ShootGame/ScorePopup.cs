using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _moveUpDistance = 100f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Ease _easeType = Ease.OutCubic;

    private CanvasGroup _canvasGroup;
    private Vector3 _startPos;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        _startPos = transform.localPosition;
    }

    public void Show(string message, Color color)
    {
        _text.text = message;
        _text.color = color;
        _canvasGroup.alpha = 1;
        transform.localPosition = _startPos;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(_startPos.y + _moveUpDistance, _duration).SetEase(_easeType));
        seq.Join(_canvasGroup.DOFade(0, _duration));
        seq.OnComplete(() => Destroy(gameObject));
    }
}
