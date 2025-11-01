using System;
using UnityEngine;

public class AvoidMovementManager : MonoBehaviour
{
    [SerializeField] private AvoidBallSO _avoidInput;
    [SerializeField] private float _moveSpeed = 5f;

    [SerializeField] private Rigidbody _p1Rb;
    [SerializeField] private Rigidbody _p2Rb;

    [SerializeField] private Animator _p1Animator;
    [SerializeField] private Animator _p2Animator;

    private Vector3 _p1moveDir;
    private Vector3 _p2moveDir;

    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");

    private bool wPressed, aPressed, sPressed, dPressed,
        upPressed, leftPressed, downPressed, rightPressed;

    private bool _p1Active = true;
    private bool _p2Active = true;

    public void DisablePlayer(int playerIndex)
    {
        if (playerIndex == 1) _p1Active = false;
        else if (playerIndex == 2) _p2Active = false;
    }

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

        if (_p1Active)
        {
            if (wPressed) _p1moveDir += Vector3.forward;
            if (sPressed) _p1moveDir += Vector3.back;
            if (aPressed) _p1moveDir += Vector3.left;
            if (dPressed) _p1moveDir += Vector3.right;
        }

        if (_p2Active)
        {
            if (upPressed) _p2moveDir += Vector3.forward;
            if (downPressed) _p2moveDir += Vector3.back;
            if (leftPressed) _p2moveDir += Vector3.left;
            if (rightPressed) _p2moveDir += Vector3.right;
        }

        ApplyMovement();

        _p1Animator.SetBool(IsRunningHash, _p1Active && _p1moveDir != Vector3.zero);
        _p2Animator.SetBool(IsRunningHash, _p2Active && _p2moveDir != Vector3.zero);
    }

    private void ApplyMovement()
    {
        if (_p1Active && _p1moveDir != Vector3.zero)
        {
            _p1Rb.MovePosition(_p1Rb.position + _p1moveDir * _moveSpeed * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(_p1moveDir);
            _p1Rb.MoveRotation(Quaternion.Slerp(_p1Rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }

        if (_p2Active && _p2moveDir != Vector3.zero)
        {
            _p2Rb.MovePosition(_p2Rb.position + _p2moveDir * _moveSpeed * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(_p2moveDir);
            _p2Rb.MoveRotation(Quaternion.Slerp(_p2Rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }
    }

    public void EnablePlayer(int playerIndex)
    {
        if (playerIndex == 1) _p1Active = true;
        else if (playerIndex == 2) _p2Active = true;
    }

    public void EnableAllPlayers()
    {
        _p1Active = true;
        _p2Active = true;
    }

}
