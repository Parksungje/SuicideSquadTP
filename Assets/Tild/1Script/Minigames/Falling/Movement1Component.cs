using UnityEngine;

namespace Tild.Minigames.Falling
{
    public class Movement1Component : MonoBehaviour
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
        private bool wPressed, aPressed, sPressed, dPressed;
        private bool isGrounded = false;

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            if (pushInput == null)
            {
                return;
            }

            pushInput.OnWKeyDown += OnWKey;
            pushInput.OnSKeyDown += OnSKey;
            pushInput.OnAKeyDown += OnAKey;
            pushInput.OnDKeyDown += OnDKey;
            pushInput.OnEKeyDown += OnEKey;
        }

        private void OnDisable()
        {
            if (pushInput == null) return;

            pushInput.OnWKeyDown -= OnWKey;
            pushInput.OnSKeyDown -= OnSKey;
            pushInput.OnAKeyDown -= OnAKey;
            pushInput.OnDKeyDown -= OnDKey;
            pushInput.OnEKeyDown -= OnEKey;
        }

        private void OnWKey(bool pressed) => wPressed = pressed;
        private void OnSKey(bool pressed) => sPressed = pressed;
        private void OnAKey(bool pressed) => aPressed = pressed;
        private void OnDKey(bool pressed) => dPressed = pressed;
        private void OnEKey(bool pressed)
        {
            if (pressed) Jump();
        }

        private void FixedUpdate()
        {
            CheckGround();
            ApplyRotation();

            _moveDir = Vector3.zero;
            if (wPressed) _moveDir += Vector3.forward;
            if (sPressed) _moveDir += Vector3.back;
            if (aPressed) _moveDir += Vector3.left;
            if (dPressed) _moveDir += Vector3.right;

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
