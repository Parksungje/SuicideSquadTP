using System.Collections;
using UnityEngine;

public class AutoPushComponent : MonoBehaviour
{
    [SerializeField] private int pushForce = 10;
    [SerializeField] private float pushDelay = 1f;

    private bool _canPush = true;

    private void OnTriggerStay(Collider other)
    {
        if (!_canPush) return;
        if (other.attachedRigidbody == null) return;

        StartCoroutine(PushRoutine(other.attachedRigidbody));
    }

    private IEnumerator PushRoutine(Rigidbody target)
    {
        _canPush = false;
        target.AddForce(Vector3.right * pushForce, ForceMode.Impulse);
        yield return new WaitForSeconds(pushDelay);
        _canPush = true;
    }
}
