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

    private Animator animator_P1;
    private Animator animator_P2;

    private int dirP1;
    private int dirP2;

    private bool isJumpingP1 = false;
    private bool isJumpingP2 = false;

    private readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    private readonly int IsRunningHash = Animator.StringToHash("IsRunning");

    private void Start()
    {
        animator_P1 = rigidbody_P1.GetComponentInChildren<Animator>();
        animator_P2 = rigidbody_P2.GetComponentInChildren<Animator>();
    }

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
        animator_P1.SetBool(IsRunningHash, isHolding);
    }

    private void HandleL_RightDir(bool isHolding)
    {
        dirP1 = isHolding ? 1 : (dirP1 == 1 ? 0 : dirP1);
        animator_P1.SetBool(IsRunningHash, isHolding);
    }

    private void HandleL_Jump()
    {
        if (IsGrounded(rigidbody_P1))
        {
            isJumpingP1 = true;
            animator_P1.SetBool(IsJumpingHash, true);

            Vector3 v = rigidbody_P1.linearVelocity;
            v.y = 0f;
            rigidbody_P1.linearVelocity = v;
            rigidbody_P1.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void HandleR_LeftDir(bool isHolding)
    {
        dirP2 = isHolding ? -1 : (dirP2 == -1 ? 0 : dirP2);
        animator_P2.SetBool(IsRunningHash, isHolding);
    }

    private void HandleR_RightDir(bool isHolding)
    {
        dirP2 = isHolding ? 1 : (dirP2 == 1 ? 0 : dirP2);
        animator_P2.SetBool(IsRunningHash, isHolding);
    }

    private void HandleR_Jump()
    {
        if (IsGrounded(rigidbody_P2))
        {
            isJumpingP2 = true;
            animator_P2.SetBool(IsJumpingHash, true);

            Vector3 v = rigidbody_P2.linearVelocity;
            v.y = 0f;
            rigidbody_P2.linearVelocity = v;
            rigidbody_P2.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(rigidbody_P1, dirP1);
        MovePlayer(rigidbody_P2, dirP2);

        ApplyExtraGravity(rigidbody_P1);
        ApplyExtraGravity(rigidbody_P2);

        UpdateAnimatorStates();
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

    private void UpdateAnimatorStates()
    {
        bool grounded1 = IsGrounded(rigidbody_P1);
        if (isJumpingP1 && grounded1)
        {
            isJumpingP1 = false;
            animator_P1.SetBool(IsJumpingHash, false);
        }

        bool grounded2 = IsGrounded(rigidbody_P2);
        if (isJumpingP2 && grounded2)
        {
            isJumpingP2 = false;
            animator_P2.SetBool(IsJumpingHash, false);
        }
    }
}
