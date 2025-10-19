using UnityEngine;

public class PlayerController_Tag : MonoBehaviour
{
    [SerializeField] private TagGameSO tagGameInput;
    [SerializeField] private Rigidbody rigidbody_P1;
    [SerializeField] private Rigidbody rigidbody_P2;

    [Header("PlayerSetting")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float maxWalkSpeed = 6f;
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private float dashForce = 5f;

    private void OnEnable()
    {
        tagGameInput.OnL_LeftDir += HandleL_LeftDir;
        tagGameInput.OnL_RightDir += HandleL_RightDir;
    }

    private void HandleL_LeftDir()
    {
        MovePlayer(rigidbody_P1, -1f);
    }

    private void HandleL_RightDir()
    {
        MovePlayer(rigidbody_P1, 1f);
    }

    private void MovePlayer(Rigidbody rb, float dir)
    {
        if (rb == null) return;

        rb.AddForce(Vector3.right * dir * walkSpeed, ForceMode.Force);

        Vector3 v = rb.linearVelocity;
        v.x = Mathf.Clamp(v.x, -maxWalkSpeed, maxWalkSpeed);
        rb.linearVelocity = v;
    }
}