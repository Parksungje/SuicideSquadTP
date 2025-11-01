using UnityEngine;

namespace Tild.Minigames.Falling
{
    public class Movement2Component : MonoBehaviour
    {
        [Header("무브먼트")]
        [field: SerializeField] private FallingInputSO pushInput;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpSpeed = 5f;
        [SerializeField] private Rigidbody target;
        [SerializeField] private Animator _animator;

        [Header("땅 체크")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.2f;

        private Rigidbody _rigid;
        private Vector3 _moveDir;
        private bool upPressed, leftPressed, rightPressed, downPressed;
        private bool isGrounded = false;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            if (pushInput == null) return;

            pushInput.OnUpArrowDown += OnUpArrow;
            pushInput.OnLeftArrowDown += OnLeftArrow;
            pushInput.OnRightArrowDown += OnRightArrow;
            pushInput.OnDownArrowDown += OnDownArrow;
            
            pushInput.OnEnterPressed += OnEnterKey;
        }

        private void OnDisable()
        {
            if (pushInput == null) return;

            pushInput.OnUpArrowDown -= OnUpArrow;
            pushInput.OnLeftArrowDown -= OnLeftArrow;
            pushInput.OnRightArrowDown -= OnRightArrow;
            pushInput.OnDownArrowDown -= OnDownArrow;

            pushInput.OnEKeyDown -= OnEnterKey;
        }

        private void OnUpArrow(bool pressed) => upPressed = pressed;
        private void OnDownArrow(bool pressed) => downPressed = pressed;
        private void OnLeftArrow(bool pressed) => leftPressed = pressed;
        private void OnRightArrow(bool pressed) => rightPressed = pressed;
        private void OnEnterKey(bool pressed)
        {
            if (pressed) Jump();
        }

        private void FixedUpdate()
        {
            CheckGround();
            ApplyRotation();

            _moveDir = Vector3.zero;
            if (upPressed) _moveDir += Vector3.forward;
            if (downPressed) _moveDir += Vector3.back;
            if (leftPressed) _moveDir += Vector3.left;
            if (rightPressed) _moveDir += Vector3.right;

            ApplyMovement();

            _animator.SetBool("isRunning", _moveDir.sqrMagnitude > 0.001f);
        }

        private void CheckGround()
        {
            isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);
        }

        private void Jump()
        {
            if (!isGrounded) return;
            _rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            isGrounded = false;
        }

        private void ApplyMovement()
        {
            _moveDir = _moveDir.normalized;
            _rigid.linearVelocity = new Vector3(
                _moveDir.x * moveSpeed,
                _rigid.linearVelocity.y,
                _moveDir.z * moveSpeed
            );
        }

        private void ApplyRotation()
        {
            if (_moveDir.sqrMagnitude < 0.001f) return;

            Vector3 direction = _moveDir.normalized;
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _rigid.MoveRotation(Quaternion.Slerp(_rigid.rotation, targetRotation, 15f * Time.fixedDeltaTime));
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheckPoint == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
