using System;
using UnityEngine;

public class AvoidMovementManager : MonoBehaviour
{
    [SerializeField] private AvoidBallSO _avoidInput;
    [SerializeField] private float _moveSpeed = 5f;

    [SerializeField] private Rigidbody _p1Rb;
    [SerializeField] private Rigidbody _p2Rb;

    private Vector3 _p1moveDir;
    private Vector3 _p2moveDir;

    private bool wPressed, aPressed, sPressed, dPressed,
        upPressed, leftPressed, downPressed, rightPressed;

    private void OnEnable()
    {
        if (_avoidInput == null)
        {
            Debug.LogError("PushGameSO reference is missing for Movement1Component.");
            return;
        }

        _avoidInput.OnWKeyDown += OnWKey;
        _avoidInput.OnSKeyDown += OnSKey;
        _avoidInput.OnAKeyDown += OnAKey;
        _avoidInput.OnDKeyDown += OnDKey;

        _avoidInput.OnUpArrowDown += OnUpKey;
        _avoidInput.OnDownArrowDown += OnDownKey;
        _avoidInput.OnLeftArrowDown += OnLeftKey;
        _avoidInput.OnRightArrowDown += OnRightKey;
    }

    private void OnDisable()
    {
        if (_avoidInput == null) return;

        _avoidInput.OnWKeyDown -= OnWKey;
        _avoidInput.OnSKeyDown -= OnSKey;
        _avoidInput.OnAKeyDown -= OnAKey;
        _avoidInput.OnDKeyDown -= OnDKey;

        _avoidInput.OnUpArrowDown -= OnUpKey;
        _avoidInput.OnDownArrowDown -= OnDownKey;
        _avoidInput.OnLeftArrowDown -= OnLeftKey;
        _avoidInput.OnRightArrowDown -= OnRightKey;
    }

    private void OnWKey(bool pressed) => wPressed = pressed;
    private void OnSKey(bool pressed) => sPressed = pressed;
    private void OnAKey(bool pressed) => aPressed = pressed;
    private void OnDKey(bool pressed) => dPressed = pressed;

    private void OnUpKey(bool pressed) => upPressed = pressed;
    private void OnDownKey(bool pressed) => downPressed = pressed;
    private void OnLeftKey(bool pressed) => leftPressed = pressed;
    private void OnRightKey(bool pressed) => rightPressed = pressed;

    private void FixedUpdate()
    {
        _p1moveDir = Vector3.zero;
        _p2moveDir = Vector3.zero;

        if (wPressed) _p1moveDir += Vector3.forward;
        if (sPressed) _p1moveDir += Vector3.back;
        if (aPressed) _p1moveDir += Vector3.left;
        if (dPressed) _p1moveDir += Vector3.right;

        if (upPressed) _p2moveDir += Vector3.forward;
        if (downPressed) _p2moveDir += Vector3.back;
        if (leftPressed) _p2moveDir += Vector3.left;
        if (rightPressed) _p2moveDir += Vector3.right;

        ApplyMovement();
    }

    private void ApplyMovement()
    {
        _p1moveDir = _p1moveDir.normalized;
        _p1Rb.linearVelocity = new Vector3(
            _p1moveDir.x * _moveSpeed,
            _p1Rb.linearVelocity.y,
            _p1moveDir.z * _moveSpeed
        );

        _p2moveDir = _p2moveDir.normalized;
        _p2Rb.linearVelocity = new Vector3(
            _p2moveDir.x * _moveSpeed,
            _p2Rb.linearVelocity.y,
            _p2moveDir.z * _moveSpeed
        );
    }
}
