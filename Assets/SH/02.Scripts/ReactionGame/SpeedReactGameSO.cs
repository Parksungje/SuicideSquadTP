using Code.Player;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SpeedReactGame Input", menuName = "8S/GameSO/SpeedReact")]
public class SpeedReactGameSO : BaseInputSO
{
    public Action OnL_Click;
    public Action OnR_Click;

    public override void OnWKey(InputAction.CallbackContext context)
    {
        if (context.performed) OnL_Click?.Invoke();
    }

    public override void OnAKey(InputAction.CallbackContext context)
    {
        if (context.performed) OnL_Click?.Invoke();
    }

    public override void OnSKey(InputAction.CallbackContext context)
    {
        if (context.performed) OnL_Click?.Invoke();
    }

    public override void OnDkey(InputAction.CallbackContext context)
    {
        if (context.performed) OnL_Click?.Invoke();
    }

    public override void OnSpace(InputAction.CallbackContext context)
    {
        if (context.performed) OnL_Click?.Invoke();
    }

    public override void OnLArrow(InputAction.CallbackContext context)
    {
        if (context.performed) OnR_Click?.Invoke();
    }

    public override void OnRArrow(InputAction.CallbackContext context)
    {
        if (context.performed) OnR_Click?.Invoke();
    }

    public override void OnUArrow(InputAction.CallbackContext context)
    {
        if (context.performed) OnR_Click?.Invoke();
    }

    public override void OnDArrow(InputAction.CallbackContext context)
    {
        if (context.performed) OnR_Click?.Invoke();
    }
}