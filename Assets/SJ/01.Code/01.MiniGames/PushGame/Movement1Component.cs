using UnityEngine;

public class Movement1Component : MonoBehaviour
{
    [field:SerializeField] private PushGameSO _pushInput;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Transform _targetTrm;

    private Rigidbody _rigid;
    private Vector3 _moveDir;

    private bool wPressed, aPressed, sPressed, dPressed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (_pushInput == null)
        {
            Debug.LogError("PushGameSO reference is missing for Movement1Component.");
            return;
        }

        _pushInput.OnWKeyDown += OnWKey;
        _pushInput.OnSKeyDown += OnSKey;
        _pushInput.OnAKeyDown += OnAKey;
        _pushInput.OnDKeyDown += OnDKey;
    }

    private void OnDisable()
    {
        if (_pushInput == null) return;

        _pushInput.OnWKeyDown -= OnWKey;
        _pushInput.OnSKeyDown -= OnSKey;
        _pushInput.OnAKeyDown -= OnAKey;
        _pushInput.OnDKeyDown -= OnDKey;
    }

    private void OnWKey(bool pressed) => wPressed = pressed;
    private void OnSKey(bool pressed) => sPressed = pressed;
    private void OnAKey(bool pressed) => aPressed = pressed;
    private void OnDKey(bool pressed) => dPressed = pressed;

    private void FixedUpdate()
    {
        this.transform.rotation = _targetTrm.rotation;

        _moveDir = Vector3.zero;
        if (wPressed) _moveDir += Vector3.forward;
        if (sPressed) _moveDir += Vector3.back;
        if (aPressed) _moveDir += Vector3.left;
        if (dPressed) _moveDir += Vector3.right;

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        _moveDir = _moveDir.normalized;
        _rigid.linearVelocity = new Vector3(
            _moveDir.x * _moveSpeed,
            _rigid.linearVelocity.y,
            _moveDir.z * _moveSpeed
        );
    }
}