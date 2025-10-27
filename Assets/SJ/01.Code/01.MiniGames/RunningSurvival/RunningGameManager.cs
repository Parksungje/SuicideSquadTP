using Code.Player;
using System;
using UnityEngine;
using DG.Tweening;
using System.Security.Cryptography;

public class RunningGameManager : MonoBehaviour
{
    [field: SerializeField] private RunningGameSO _runningInput;
    [SerializeField] private GameObject _p1Obj;
    [SerializeField] private GameObject _p2Obj;

    private bool _aPressed, _dPressed, _leftArrowPressed, _rightArrowPressed;

    private void OnEnable()
    {
        if (_runningInput == null) return;

        _runningInput.OnAKeyDown += SetP1A;
        _runningInput.OnDKeyDown += SetP1D;

        _runningInput.OnLeftArrowDown += SetP2L;
        _runningInput.OnRightArrowDown += SetP2R;
    }

    private void OnDestroy()
    {
        if (_runningInput == null) return;

        _runningInput.OnAKeyDown -= SetP1A;
        _runningInput.OnDKeyDown -= SetP1D;

        _runningInput.OnLeftArrowDown -= SetP2L;
        _runningInput.OnRightArrowDown -= SetP2R;
    }

    private void SetP1A(bool isPressed)
    {
        _aPressed = isPressed;
    }

    private void SetP1D(bool isPressed) 
    {
        _dPressed = isPressed;
    }

    private void SetP2L(bool isPressed)
    {
        _leftArrowPressed = isPressed;
    }

    private void SetP2R(bool isPressed)
    {
        _rightArrowPressed = isPressed;
    }

    private void FixedUpdate()
    {
        if (_aPressed) _p1Obj.transform.DOMove(new Vector3(-16, 0, 15), 0.3f);
        if (_dPressed) _p1Obj.transform.DOMove(new Vector3(-4, 0, 15), 0.3f);

        if (_leftArrowPressed) _p2Obj.transform.DOMove(new Vector3(16, 0, 15), 0.3f);
        if (_rightArrowPressed) _p2Obj.transform.DOMove(new Vector3(4, 0, 15), 0.3f);
    }
}
