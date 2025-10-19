using Code.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "VolleyBall Input", menuName = "SJ/GameSO/VolleyBall", order = 2)]

public class VolleyBallSO : BaseInputSO
{
    public Action OnLSpike;
    public Action OnLLeftDir;
    public Action OnLRightDir;
    public Action OnLJump;

    public Action OnRSpike;
    public Action OnRLeftDir;
    public Action OnRRightDir;
    public Action OnRJump;


    public override void OnWKey(InputAction.CallbackContext context)
    {
        base.OnWKey(context);

        if (context.performed)
            OnLSpike?.Invoke();
    }

    public override void OnAKey(InputAction.CallbackContext context)
    {
        base.OnAKey(context);

        if (context.performed)
            OnLLeftDir?.Invoke();
    }

    public override void OnDkey(InputAction.CallbackContext context)
    {
        base.OnDkey(context);

        if (context.performed)
            OnLRightDir?.Invoke();
    }

    public override void OnSKey(InputAction.CallbackContext context)
    {
        base.OnSKey(context);

        if (context.performed)
            OnLJump?.Invoke();
    }

    public override void OnUArrow(InputAction.CallbackContext context)
    {
        base.OnUArrow(context);

        if (context.performed)
            OnRSpike?.Invoke();
    }

    public override void OnRArrow(InputAction.CallbackContext context)
    {
        base.OnRArrow(context);

        if (context.performed)
            OnRRightDir?.Invoke();
    }

    public override void OnLArrow(InputAction.CallbackContext context)
    {
        base.OnLArrow(context);

        if (context.performed)
            OnRLeftDir?.Invoke();
    }

    public override void OnDArrow(InputAction.CallbackContext context)
    {
        base.OnDArrow(context);

        if (context.performed)
            OnRJump?.Invoke();
    }
}
