using UnityEngine;

public class Movement2Component : MonoBehaviour
{
    [SerializeField] private PushGameSO pushInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody target;


    private Rigidbody _rigid;
    private Animator _animator;
    private Vector3 _moveDir;

    private bool upPressed, leftPressed, downPressed, rightPressed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (pushInput == null) return;

        pushInput.OnUpArrowDown += OnUpKey;
        pushInput.OnDownArrowDown += OnDownKey;
        pushInput.OnLeftArrowDown += OnLeftKey;
        pushInput.OnRightArrowDown += OnRightKey;
    }

    private void OnDisable()
    {
        if (pushInput == null) return;

        pushInput.OnUpArrowDown -= OnUpKey;
        pushInput.OnDownArrowDown -= OnDownKey;
        pushInput.OnLeftArrowDown -= OnLeftKey;
        pushInput.OnRightArrowDown -= OnRightKey;
    }

    private void OnUpKey(bool pressed) => upPressed = pressed;
    private void OnDownKey(bool pressed) => downPressed = pressed;
    private void OnLeftKey(bool pressed) => leftPressed = pressed;
    private void OnRightKey(bool pressed) => rightPressed = pressed;

    private void FixedUpdate()
    {
        _moveDir = Vector3.zero;
        ApplyRotation();
        
        if (upPressed) _moveDir += Vector3.forward;
        if (downPressed) _moveDir += Vector3.back;
        if (leftPressed) _moveDir += Vector3.left;
        if (rightPressed) _moveDir += Vector3.right;
        
        ApplyMovement();
        _animator.SetBool("isRunning", _moveDir.sqrMagnitude > 0.001f);
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
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f; 
        
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        _rigid.MoveRotation(Quaternion.Slerp(_rigid.rotation, targetRotation, 60 * Time.fixedDeltaTime));
        
    }
}