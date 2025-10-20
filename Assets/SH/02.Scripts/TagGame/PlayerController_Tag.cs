using UnityEngine;

public class PlayerController_Tag : MonoBehaviour
{
    [SerializeField] private TagGameSO tagGameInput;
    [SerializeField] private Rigidbody rigidbody_P1;
    [SerializeField] private Rigidbody rigidbody_P2;

    [Header("PlayerSetting")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpPower = 8f;
    [SerializeField] private float inertia = .05f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundRayDistance = 1.1f;

    private int dirP1;
    private int dirP2;

    private void OnEnable()
    {
        tagGameInput.OnL_LeftDir += HandleL_LeftDir;
        tagGameInput.OnL_RightDir += HandleL_RightDir;
        tagGameInput.OnR_LeftDir += HandleR_LeftDir;
        tagGameInput.OnR_RightDir += HandleR_RightDir;

        tagGameInput.OnL_Jump += HandleL_Jump;
        tagGameInput.OnR_Jump += HandleR_Jump;
    }

    private void OnDisable()
    {
        tagGameInput.OnL_LeftDir -= HandleL_LeftDir;
        tagGameInput.OnL_RightDir -= HandleL_RightDir;
        tagGameInput.OnR_LeftDir -= HandleR_LeftDir;
        tagGameInput.OnR_RightDir -= HandleR_RightDir;

        tagGameInput.OnL_Jump -= HandleL_Jump;
        tagGameInput.OnR_Jump -= HandleR_Jump;
    }

    private void HandleL_LeftDir(bool isHolding)
    {
        dirP1 = isHolding ? -1 : (dirP1 == -1 ? 0 : dirP1);
    }

    private void HandleL_RightDir(bool isHolding)
    {
        dirP1 = isHolding ? 1 : (dirP1 == 1 ? 0 : dirP1);
    }

    private void HandleL_Jump()
    {
        if (IsGrounded(rigidbody_P1))
        {
            Vector3 v = rigidbody_P1.linearVelocity;
            v.y = 0f;
            rigidbody_P1.linearVelocity = v;
            rigidbody_P1.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void HandleR_LeftDir(bool isHolding)
    {
        dirP2 = isHolding ? -1 : (dirP2 == -1 ? 0 : dirP2);
    }

    private void HandleR_RightDir(bool isHolding)
    {
        dirP2 = isHolding ? 1 : (dirP2 == 1 ? 0 : dirP2);
    }

    private void HandleR_Jump()
    {
        if (IsGrounded(rigidbody_P2))
        {
            Vector3 v = rigidbody_P2.linearVelocity;
            v.y = 0f;
            rigidbody_P2.linearVelocity = v;
            rigidbody_P2.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(rigidbody_P1, dirP1);
        ApplyExtraGravity(rigidbody_P1);
        MovePlayer(rigidbody_P2, dirP2);
        ApplyExtraGravity(rigidbody_P2);
    }

    private void MovePlayer(Rigidbody rb, int dir)
    {
        Vector3 velocity = rb.linearVelocity;
        float targetX = dir * walkSpeed;
        velocity.x = Mathf.Lerp(velocity.x, targetX, inertia);
        rb.linearVelocity = velocity;
    }

    private void ApplyExtraGravity(Rigidbody rb)
    {
        rb.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);
    }

    private bool IsGrounded(Rigidbody rb)
    {
        return Physics.Raycast(rb.position + Vector3.up, Vector3.down, groundRayDistance, groundMask);
    }
}