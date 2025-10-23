using System;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [field: SerializeField] private ShootGameSO _shootGameSO;

    [SerializeField] private Rigidbody _p1CrossHair;
    [SerializeField] private Rigidbody _p2CrossHair;
    [SerializeField] private Rigidbody _p1Obj;
    [SerializeField] private Rigidbody _p2Obj;

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

        _shootGameSO.OnWKeyDown += SetP1W;
        _shootGameSO.OnSKeyDown += SetP1S;
        _shootGameSO.OnAKeyDown += SetP1A;
        _shootGameSO.OnDKeyDown += SetP1D;

        _shootGameSO.OnUpArrowDown += SetP2UpArrow;
        _shootGameSO.OnDownArrowDown += SetP2DownArrow;
        _shootGameSO.OnLeftArrowDown += SetP2LeftArrow;
        _shootGameSO.OnRightArrowDown += SetP2RightArrow;

        _shootGameSO.OnEKeyDown += SetP1Shoot;
        _shootGameSO.OnEnterKeyDown += SetP2Shoot;
    }

    

    private void OnDisable()
    {
        if (_shootGameSO == null) return;

        _shootGameSO.OnWKeyDown -= SetP1W;
        _shootGameSO.OnSKeyDown -= SetP1S;
        _shootGameSO.OnAKeyDown -= SetP1A;
        _shootGameSO.OnDKeyDown -= SetP1D;

        _shootGameSO.OnUpArrowDown -= SetP2UpArrow;
        _shootGameSO.OnDownArrowDown -= SetP2DownArrow;
        _shootGameSO.OnLeftArrowDown -= SetP2LeftArrow;
        _shootGameSO.OnRightArrowDown -= SetP2RightArrow;
    }

    private void SetP1W(bool isPressed) => _wPressed = isPressed;
    private void SetP1S(bool isPressed) => _sPressed = isPressed;
    private void SetP1A(bool isPressed) => _aPressed = isPressed;
    private void SetP1D(bool isPressed) => _dPressed = isPressed;
    private void SetP2UpArrow(bool isPressed) => _upArrowPressed = isPressed;
    private void SetP2DownArrow(bool isPressed) => _downArrowPressed = isPressed;
    private void SetP2LeftArrow(bool isPressed) => _leftArrowPressed = isPressed;
    private void SetP2RightArrow(bool isPressed) => _rightArrowPressed = isPressed;
    private void SetP1Shoot(bool isPressed)
    {
    }

    private void SetP2Shoot(bool isPressed)
    {
    }

    private void FixedUpdate()
    {
        UpdateP1Direction();
        UpdateP2Direction();

        _p1CrossHair.linearVelocity = _p1HairDir.normalized * _moveSpeed;
        _p2CrossHair.linearVelocity = _p2HairDir.normalized * _moveSpeed;

        Vector3 p1TargetPos = _p1CrossHair.position;
        p1TargetPos.y = _p1Obj.transform.position.y;
        _p1Obj.transform.LookAt(p1TargetPos);

        Vector3 p2TargetPos = _p2CrossHair.position;
        p2TargetPos.y = _p2Obj.transform.position.y;
        _p2Obj.transform.LookAt(p2TargetPos);
    }

    private void UpdateP1Direction()
    {
        _p1HairDir = Vector3.zero;

        if (_wPressed) _p1HairDir += Vector3.up;
        if (_sPressed) _p1HairDir += Vector3.down;
        if (_aPressed) _p1HairDir += Vector3.left;
        if (_dPressed) _p1HairDir += Vector3.right;
    }

    private void UpdateP2Direction()
    {
        _p2HairDir = Vector3.zero;

        if (_upArrowPressed) _p2HairDir += Vector3.up;
        if (_downArrowPressed) _p2HairDir += Vector3.down;
        if (_leftArrowPressed) _p2HairDir += Vector3.left;
        if (_rightArrowPressed) _p2HairDir += Vector3.right;
    }
}
