using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] private ShootGameSO _shootGameSO;
    [SerializeField] private Rigidbody _p1CrossHair;
    [SerializeField] private Rigidbody _p2CrossHair;
    [SerializeField] private float _moveSpeed;

    private Vector3 _p1HairDir;
    private Vector3 _p2HairDir;

    private bool _wPressed, _sPressed, _aPressed, _dPressed,
        _upArrowPressed, _downArrowPressed, _leftArrowPressed, _rightArrowPressed;

    private void Awake()
    {
        _p1HairDir = Vector3.zero;
        _p2HairDir = Vector3.zero;
    }

    private void OnEnable()
    {
        if (_shootGameSO == null) return;

        _shootGameSO.OnWKeyDown += OnP1Up;
        _shootGameSO.OnSKeyDown += OnP1Down;
        _shootGameSO.OnAKeyDown += OnP1Left;
        _shootGameSO.OnDKeyDown += OnP1Right;

        _shootGameSO.OnUpArrowDown += OnP2Up;
        _shootGameSO.OnDownArrowDown += OnP2Down;
        _shootGameSO.OnLeftArrowDown += OnP2Left;
        _shootGameSO.OnRightArrowDown += OnP2Right;
    }

    private void OnDisable()
    {
        if (_shootGameSO == null) return;

        _shootGameSO.OnWKeyDown -= OnP1Up;
        _shootGameSO.OnSKeyDown -= OnP1Down;
        _shootGameSO.OnAKeyDown -= OnP1Left;
        _shootGameSO.OnDKeyDown -= OnP1Right;

        _shootGameSO.OnUpArrowDown -= OnP2Up;
        _shootGameSO.OnDownArrowDown -= OnP2Down;
        _shootGameSO.OnLeftArrowDown -= OnP2Left;
        _shootGameSO.OnRightArrowDown -= OnP2Right;
    }

    private void OnP1Up(bool isPressed) => _wPressed = isPressed;
    private void OnP1Down(bool isPressed) => _sPressed = isPressed;
    private void OnP1Left(bool isPressed) => _aPressed = isPressed;
    private void OnP1Right(bool isPressed) => _dPressed = isPressed;
    private void OnP2Up(bool isPressed) => _upArrowPressed = isPressed;
    private void OnP2Down(bool isPressed) => _downArrowPressed = isPressed;
    private void OnP2Left(bool isPressed) => _leftArrowPressed = isPressed;
    private void OnP2Right(bool isPressed) => _rightArrowPressed = isPressed;

    private void FixedUpdate()
    {
        _p1HairDir = Vector3.zero;
        _p2HairDir = Vector3.zero;

        if (_wPressed) _p1HairDir += Vector3.up;
        if (_sPressed) _p1HairDir += Vector3.down;
        if (_aPressed) _p1HairDir += Vector3.left;
        if (_dPressed) _p1HairDir += Vector3.right;

        if (_upArrowPressed) _p2HairDir += Vector3.up;
        if (_downArrowPressed) _p2HairDir += Vector3.down;
        if (_leftArrowPressed) _p2HairDir += Vector3.left;
        if (_rightArrowPressed) _p2HairDir += Vector3.right;

        _p1CrossHair.linearVelocity = _p1HairDir.normalized * _moveSpeed;
        _p2CrossHair.linearVelocity = _p2HairDir.normalized * _moveSpeed;
    }
}
