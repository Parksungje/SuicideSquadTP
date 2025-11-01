using UnityEngine;

public class AvoidBall : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private float moveSpeed = 150f;
    [SerializeField] private float spinForce = 50f;
    private float knockbackForce = 100f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _rb.freezeRotation = false;
    }

    private void Start()
    {
        Destroy(gameObject, 10f);
        _rb.linearVelocity = Vector3.back * moveSpeed;
    }

    private void FixedUpdate()
    {
        _rb.AddTorque(Vector3.right * spinForce, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.Play("AvoidBall_Hit");
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                Vector3 pushDirection = _rb.linearVelocity.normalized;
                playerRb.AddForce(pushDirection * knockbackForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        _rb.linearVelocity = Vector3.back * moveSpeed;
    }
}
