using Code.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CharacterSelect Input", menuName = "8S/UISO/CharacterSelect")]
public class CharacterSelectSO : BaseInputSO
{
    public Action<bool> OnL_Previous;
    public Action<bool> OnL_Next;
    public Action<bool> OnL_Confirm;
    public Action<bool> OnL_Cancel;

    public Action<bool> OnR_Previous;
    public Action<bool> OnR_Next;
    public Action<bool> OnR_Confirm;
    public Action<bool> OnR_Cancel;

    private static void InvokeIfPerformed(Action<bool> action, InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        action?.Invoke(false);
    }

    public override void OnWKey(InputAction.CallbackContext context) => InvokeIfPerformed(OnL_Confirm, context);
    public override void OnAKey(InputAction.CallbackContext context) => InvokeIfPerformed(OnL_Previous, context);
    public override void OnSKey(InputAction.CallbackContext context) => InvokeIfPerformed(OnL_Cancel, context);
    public override void OnDkey(InputAction.CallbackContext context) => InvokeIfPerformed(OnL_Next, context);

    public override void OnUArrow(InputAction.CallbackContext context) => InvokeIfPerformed(OnR_Confirm, context);
    public override void OnLArrow(InputAction.CallbackContext context) => InvokeIfPerformed(OnR_Previous, context);
    public override void OnDArrow(InputAction.CallbackContext context) => InvokeIfPerformed(OnR_Cancel, context);
    public override void OnRArrow(InputAction.CallbackContext context) => InvokeIfPerformed(OnR_Next, context);
}