using UnityEngine;

namespace Tild.Minigames.Falling
{
    public class FallingManager : MonoBehaviour
    {
        [field:SerializeField] private PushGameSO pushInput;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Rigidbody target;
    

        private Rigidbody _rigid;
        private Animator _animator;
        private Vector3 _moveDir;

        private bool wPressed, aPressed, sPressed, dPressed, ePressed;
        public bool _isBeingPushed = false;
        private void Awake()
        {
            _rigid = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            if (pushInput == null)
            {
                Debug.LogError("PushGameSO reference is missing for Movement1Component.");
                return;
            }

            pushInput.OnWKeyDown += OnWKey;
            pushInput.OnSKeyDown += OnSKey;
            pushInput.OnAKeyDown += OnAKey;
            pushInput.OnDKeyDown += OnDKey;
            
        }

        private void OnDisable()
        {
            if (pushInput == null) return;

            pushInput.OnWKeyDown -= OnWKey;
            pushInput.OnSKeyDown -= OnSKey;
            pushInput.OnAKeyDown -= OnAKey;
            pushInput.OnDKeyDown -= OnDKey;
            
        }

        private void OnWKey(bool pressed) => wPressed = pressed;
        private void OnSKey(bool pressed) => sPressed = pressed;
        private void OnAKey(bool pressed) => aPressed = pressed;
        private void OnDKey(bool pressed) => dPressed = pressed;
       

        private void FixedUpdate()
        {
            ApplyRotation();
            
            _moveDir = Vector3.zero;
            if (wPressed) _moveDir += Vector3.forward;
            if (sPressed) _moveDir += Vector3.back;
            if (aPressed) _moveDir += Vector3.left;
            if (dPressed) _moveDir += Vector3.right;

            ApplyMovement();
          
        
            _animator.SetBool("isRunning", _moveDir.sqrMagnitude > 0.001f);

           
        }
     
        private void ApplyMovement()
        {
            if (_isBeingPushed) return;
            _moveDir = _moveDir.normalized;
            _rigid.linearVelocity = new Vector3(
                _moveDir.x * moveSpeed,
                _rigid.linearVelocity.y,
                _moveDir.z * moveSpeed
            );
        }   
        private void ApplyRotation()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0f; 
            
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _rigid.MoveRotation(Quaternion.Slerp(_rigid.rotation, targetRotation, 60 * Time.fixedDeltaTime));
            
        }
        }
}