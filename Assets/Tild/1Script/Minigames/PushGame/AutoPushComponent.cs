using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class AutoPushComponent : MonoBehaviour
{
    [SerializeField] private int pushForce = 10;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private float beingPushedTime = 0.5f;

    private bool _canPush = true;
    private bool _isCharging = false;
    private bool _isPushing = false;
    private Sequence _chargeSeq;
    private readonly Dictionary<UnityEngine.Object, Coroutine> _resetters = new Dictionary<UnityEngine.Object, Coroutine>();

    public void Push(bool pressed)
    {
        if (pressed) StartCharge();
        else ReleasePush();
    }

    private void StartCharge()
    {
        if (!_canPush || _isCharging) return;
        _isCharging = true;
        _canPush = false;
        _chargeSeq?.Kill();
        _chargeSeq = DOTween.Sequence();
        _chargeSeq.Append(transform.DOLocalRotate(new Vector3(0, -75, 0), 0.3f).SetEase(Ease.OutSine));
    }

    private void ReleasePush()
    {
        if (!_isCharging) return;
        _isCharging = false;
        _chargeSeq?.Kill();

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(0, 75, 0), 0.1f).SetEase(Ease.InSine).OnStart(() => _isPushing = true));
        seq.Append(transform.DOLocalRotate(new Vector3(0, 75, 0), 0.05f).SetEase(Ease.InSine).OnStart(() => _isPushing = false));
        seq.Append(transform.DOLocalRotate(Vector3.zero, 0.15f).SetEase(Ease.OutSine).SetDelay(0.1f));
        seq.OnComplete(() => { Invoke(nameof(TurnCanpush), 2f); });
        seq.Play();
    }

    private void TurnCanpush()
    {
        _canPush = true;
    }

    private void Update()
    {
        if (!_isPushing) return;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance))
        {
            Rigidbody targetRb = hit.rigidbody;
            if (targetRb == null) return;

            if (hitParticles) hitParticles.Play();

            float side = Random.Range(-1f, 1f);
            Vector3 dir = transform.right * side + transform.forward * 0.1f;
            dir.y = 0f;
            dir = dir.sqrMagnitude > 0f ? dir.normalized : transform.right;

            targetRb.AddForce(dir * pushForce, ForceMode.Impulse);
            targetRb.AddTorque(Vector3.up * pushForce, ForceMode.Impulse);

            TrySetBeingPushed(targetRb, beingPushedTime);
        }
    }

    private void TrySetBeingPushed(Rigidbody rb, float duration)
    {
        var m1 = rb.GetComponent<Movement1Component>();
        if (m1 != null)
        {
            SetBeingPushedTimed(m1, v => m1._isBeingPushed = v, duration);
            return;
        }

        var m2 = rb.GetComponent<Movement2Component>();
        if (m2 != null)
        {
            SetBeingPushedTimed(m2, v => m2._isBeingPushed = v, duration);
            return;
        }
    }

    private void SetBeingPushedTimed(UnityEngine.Object key, Action<bool> setter, float duration)
    {
        if (_resetters.TryGetValue(key, out var c)) StopCoroutine(c);
        setter(true);
        _resetters[key] = StartCoroutine(CoReset(key, setter, duration));
    }

    private System.Collections.IEnumerator CoReset(UnityEngine.Object key, Action<bool> setter, float duration)
    {
        yield return new WaitForSeconds(duration);
        setter(false);
        _resetters.Remove(key);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}
