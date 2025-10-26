using Code.Player;
using System;
using UnityEngine;

public class RunningGameManager : MonoBehaviour
{
    [field: SerializeField] private RunningGameSO _runningInput;

    private bool _aPressed, _dPressed, _upArrowPressed, _downArrowPressed;

    private void OnEnable()
    {
        if (_runningInput == null) return;

        _runningInput.OnAKeyDown += SetP1A;
        _runningInput.OnDKeyDown += SetP1D;

        _runningInput.OnLeftArrowDown += SetP2L;
        _runningInput.OnRightArrowDown += SetP2R;
    }

    private void SetP1A(bool obj)
    {
        throw new NotImplementedException();
    }

    private void SetP1D(bool obj)
    {
        throw new NotImplementedException();
    }

    private void SetP2L(bool obj)
    {
        throw new NotImplementedException();
    }

    private void SetP2R(bool obj)
    {
        throw new NotImplementedException();
    }
}
