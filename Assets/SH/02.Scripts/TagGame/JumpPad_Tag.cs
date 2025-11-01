using UnityEngine;

public class JumpPad_Tag : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            SoundManager.Instance.Play("Tag_Jump");
        }
    }
}