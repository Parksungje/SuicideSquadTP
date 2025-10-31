using System;
using DG.Tweening;
using UnityEngine;

public class AutoPushComponent : MonoBehaviour
{
    [SerializeField] private int pushForce = 10;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private ParticleSystem hitParticles;

    private bool _canPush = true;
    private bool _isCharging = false;
    private bool _isPushing = false;
    private Sequence _chargeSeq;

    public void Push(bool pressed)
    {
        if (pressed)
        {
            StartCharge();
        }
        else
        {
            ReleasePush();
        }
    }

    private void StartCharge()
    {
        if (!_canPush || _isCharging) return;

        _isCharging = true;
        _canPush = false;

        _chargeSeq?.Kill();
        _chargeSeq = DOTween.Sequence();
        _chargeSeq.Append(transform.DOLocalRotate(new Vector3(0, -75, 0), 0.3f)
            .SetEase(Ease.OutSine));
    }

    private void ReleasePush()
    {
        if (!_isCharging) return;

        _isCharging = false;

     
        _chargeSeq?.Kill();

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(0, 75, 0), 0.1f)
            .SetEase(Ease.InSine)
            .OnStart(() => _isPushing = true));
        seq.Append(transform.DOLocalRotate(new Vector3(0, 75, 0), 0.05f)
            .SetEase(Ease.InSine)
            .OnStart(() => _isPushing = false));
        seq.Append(transform.DOLocalRotate(Vector3.zero, 0.15f)
            .SetEase(Ease.OutSine)
            .SetDelay(0.1f));
        seq.OnComplete(() =>
        {
           
            _canPush = true;
        });
        seq.Play();
    }

    private void Update()
    {
        if (!_isPushing) return;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance))
        {
            Rigidbody targetRb = hit.collider.attachedRigidbody;
            if (targetRb == null) return;

            hitParticles.Play();
            targetRb.AddForce((transform.forward + Vector3.up * 0.5f) * pushForce, ForceMode.Impulse);
            targetRb.AddTorque(Vector3.up * pushForce, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}
