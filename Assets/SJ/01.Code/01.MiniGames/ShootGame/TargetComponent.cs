using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TargetComponent : MonoBehaviour
{
    public enum TargetMovementType
    {
        None,
        Horizontal,
        Vertical,
        ArcJump
    }

    [SerializeField] private float appearDuration = 0.3f;
    [SerializeField] private float visibleTime = 1.5f;
    [SerializeField] private float disappearDuration = 0.3f;

    [SerializeField] private TargetMovementType movementType = TargetMovementType.Horizontal;
    [SerializeField] private float moveRange = 1.5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float arcHeight = 1f;

    private Vector3 _startPos;
    private bool _isActive = false;
    private Coroutine _routine;

    void Start()
    {
        _startPos = transform.localPosition;
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        if (_routine != null)
            StopCoroutine(_routine);

        SetRandomMovementType();

        transform.localPosition = _startPos;
        _routine = StartCoroutine(TargetRoutine());
    }

    private void SetRandomMovementType()
    {
        int randomValue = Random.Range(1, (int)TargetMovementType.ArcJump + 1);
        movementType = (TargetMovementType)randomValue;

    }

    private IEnumerator TargetRoutine()
    {
        _isActive = true;
        gameObject.SetActive(true);

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, appearDuration).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(appearDuration);

        ApplyMovement();

        yield return new WaitForSeconds(visibleTime);

        Disappear();
    }

    private void ApplyMovement()
    {
        transform.DOKill(true);

        switch (movementType)
        {
            case TargetMovementType.Horizontal:
                transform.DOLocalMoveX(_startPos.x + moveRange, moveSpeed)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
                break;

            case TargetMovementType.Vertical:
                transform.DOLocalMoveY(_startPos.y + moveRange, moveSpeed)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
                break;

            case TargetMovementType.ArcJump:
                Sequence arcSequence = DOTween.Sequence()
                    .Append(transform.DOLocalMoveY(_startPos.y + arcHeight, moveSpeed * 0.5f).SetEase(Ease.OutQuad))
                    .Join(transform.DOLocalMoveX(_startPos.x + moveRange, moveSpeed).SetEase(Ease.Linear))
                    .Append(transform.DOLocalMoveY(_startPos.y, moveSpeed * 0.5f).SetEase(Ease.InQuad));

                arcSequence.SetLoops(-1, LoopType.Restart);
                break;

            case TargetMovementType.None:
            default:
                break;
        }
    }

    private void Disappear()
    {
        if (!_isActive) return;

        _isActive = false;
        transform.DOKill(true);

        transform.DOScale(Vector3.zero, disappearDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
            transform.localPosition = _startPos;
        });
    }

    public void OnHit()
    {
        if (!_isActive) return;

        _isActive = false;
        transform.DOKill();

        gameObject.SetActive(false);
        transform.localPosition = _startPos;
    }
}