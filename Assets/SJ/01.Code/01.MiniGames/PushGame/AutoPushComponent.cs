using System;
using DG.Tweening;
using UnityEngine;

public class AutoPushComponent : MonoBehaviour
{
    [SerializeField] private int pushForce = 10;
    [SerializeField] private float rayDistance = 2f;
    private bool _canPush = true;
    private bool _isPushing = false;

    public void Push()
    {
        if (!_canPush) return;

        _canPush = false;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(0, -50, 0), 0.3f)
            .SetEase(Ease.OutSine)
            .OnStart(() => _isPushing = true));
        seq.Append(transform.DOLocalRotate(new Vector3(0, 50, 0), 0.1f)
            .SetEase(Ease.InSine));
        seq.Append(transform.DOLocalRotate(Vector3.zero, 0.1f)
            .SetEase(Ease.OutSine)
            .SetDelay(0.15f));
        seq.OnComplete(() =>
        {
            _isPushing = false;
            _canPush = true;
        });
        seq.Play();
    }

    private void Update()
    {
        if (!_isPushing) return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, rayDistance))
        {
            
            Rigidbody targetRb = hit.collider.attachedRigidbody;
            targetRb.freezeRotation = false;
            if (targetRb != null)
            {
                // 밀리는 상태 적용
                var movement1 = targetRb.GetComponent<Movement1Component>();
                if (movement1 != null)
                {
                    movement1._isBeingPushed = true;
                }
                else
                {
                    var movement2 = targetRb.GetComponent<Movement2Component>();
                    if (movement2 != null)
                    {
                        movement2._isBeingPushed = true;
                    }
                }

                targetRb.AddForce((transform.forward + (Vector3.up / 2)) * pushForce, ForceMode.Impulse);
                targetRb.AddTorque(Vector3.up * pushForce, ForceMode.Impulse);
         
            }
        }
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}