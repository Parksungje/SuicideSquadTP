using UnityEngine;

public class Movement2Component : MonoBehaviour
{
    [SerializeField] private PushGameSO pushInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform _targetTrm;

    private Rigidbody _rigid;
    private Vector3 _moveDir;

    private bool upPressed, leftPressed, downPressed, rightPressed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
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
        this.transform.rotation = _targetTrm.rotation;

        _moveDir = Vector3.zero;
        if (upPressed) _moveDir += Vector3.forward;
        if (downPressed) _moveDir += Vector3.back;
        if (leftPressed) _moveDir += Vector3.left;
        if (rightPressed) _moveDir += Vector3.right;

        ApplyMovement();
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
}