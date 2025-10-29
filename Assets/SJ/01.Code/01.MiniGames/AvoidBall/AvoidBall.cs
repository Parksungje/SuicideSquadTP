using UnityEngine;

public class AvoidBall : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private float moveSpeed = 150f;

    private float knockbackForce = 100f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rb.freezeRotation = true;
    }

    private void Start()
    {
        Destroy(gameObject, 10f);
        _rb.linearVelocity = Vector3.back * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어 맞음!");

            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = _rb.linearVelocity.normalized;
                playerRb.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);
            }

            _rb.linearVelocity = Vector3.back * moveSpeed;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        _rb.linearVelocity = Vector3.back * moveSpeed;
        _rb.angularVelocity = Vector3.zero;
    }
}
