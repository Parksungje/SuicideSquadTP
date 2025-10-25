using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TargetComponent : MonoBehaviour
{
    [SerializeField] private float appearDuration = 0.3f;
    [SerializeField] private float visibleTime = 1.5f; 
    [SerializeField] private float disappearDuration = 0.3f;
    [SerializeField] private float moveRange = 1.5f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private bool moveHorizontally = true;

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

        _routine = StartCoroutine(TargetRoutine());
    }

    private IEnumerator TargetRoutine()
    {
        _isActive = true;
        gameObject.SetActive(true);

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, appearDuration).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(appearDuration);

        if (moveHorizontally)
        {
            transform.DOLocalMoveX(_startPos.x + moveRange, moveSpeed)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        yield return new WaitForSeconds(visibleTime);

        transform.DOKill();
        transform.DOScale(Vector3.zero, disappearDuration).SetEase(Ease.InBack);
        yield return new WaitForSeconds(disappearDuration);

        gameObject.SetActive(false);
        _isActive = false;
    }

    public void OnHit()
    {
        if (!_isActive) return;

        _isActive = false;
        transform.DOKill();

        transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
