using System.Collections;
using UnityEngine;

public class AutoPushComponent : MonoBehaviour
{
    [SerializeField] private int pushForce = 10;
    [SerializeField] private float pushDelay = 1f;
    [SerializeField] private Rigidbody targetRb;

    private bool _canPush = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!_canPush) return;
        if (other.attachedRigidbody == null) return;

        StartCoroutine(PushRoutine(other.attachedRigidbody));
    }

    private IEnumerator PushRoutine(Rigidbody target)
    {
        _canPush = false;
        targetRb.AddForce(Vector3.back * pushForce, ForceMode.Impulse);
        yield return new WaitForSeconds(pushDelay);
        _canPush = true;
    }
}
