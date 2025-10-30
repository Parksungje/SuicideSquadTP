using Code.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ShootGame Input", menuName = "SJ/GameSO/ShootGame", order = 4)]

public class ShootGameSO : BaseInputSO
{
    public Action<bool> OnWKeyDown;
    public Action<bool> OnAKeyDown;
    public Action<bool> OnDKeyDown;
    public Action<bool> OnSKeyDown;

    public Action<bool> OnUpArrowDown;
    public Action<bool> OnLeftArrowDown;
    public Action<bool> OnRightArrowDown;
    public Action<bool> OnDownArrowDown;

    public Action<bool> OnEKeyDown;
    public Action<bool> OnEnterKeyDown;


    public override void OnWKey(InputAction.CallbackContext context)
    {
        base.OnWKey(context);

        if (context.performed)
            OnWKeyDown?.Invoke(true);
        else
            OnWKeyDown?.Invoke(false);
    }

    public override void OnAKey(InputAction.CallbackContext context)
    {
        base.OnAKey(context);

        if (context.performed)
            OnAKeyDown?.Invoke(true);
        else
            OnAKeyDown?.Invoke(false);
    }

    public override void OnDkey(InputAction.CallbackContext context)
    {
        base.OnDkey(context);

        if (context.performed)
            OnDKeyDown?.Invoke(true);
        else
            OnDKeyDown?.Invoke(false);
    }

    public override void OnSKey(InputAction.CallbackContext context)
    {
        base.OnSKey(context);

        if (context.performed)
            OnSKeyDown?.Invoke(true);   
        else
            OnSKeyDown?.Invoke(false);
    }

    public override void OnUArrow(InputAction.CallbackContext context)
    {
        base.OnUArrow(context);

        if (context.performed)
            OnUpArrowDown?.Invoke(true);
        else
            OnUpArrowDown?.Invoke(false);
    }

    public override void OnRArrow(InputAction.CallbackContext context)
    {
        base.OnRArrow(context);

        if (context.performed)
            OnRightArrowDown?.Invoke(true);
        else
            OnRightArrowDown?.Invoke(false);
    }

    public override void OnLArrow(InputAction.CallbackContext context)
    {
        base.OnLArrow(context);

        if (context.performed)
            OnLeftArrowDown?.Invoke(true);
        else
            OnLeftArrowDown?.Invoke(false);
    }

    public override void OnDArrow(InputAction.CallbackContext context)
    {
        base.OnDArrow(context);

        if (context.performed)
            OnDownArrowDown?.Invoke(true);
        else
            OnDownArrowDown?.Invoke(false);
    }

    public override void OnEKey(InputAction.CallbackContext context)
    {
        base.OnEKey(context);

        if (context.performed)
            OnEKeyDown?.Invoke(true);
        else
            OnEKeyDown?.Invoke(false);
    }

    public override void OnEnter(InputAction.CallbackContext context)
    {
        base.OnEnter(context);

        if (context.performed)
            OnEnterKeyDown?.Invoke(true);
        else
            OnEnterKeyDown?.Invoke(false);

    }
}
