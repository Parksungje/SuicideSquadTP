using Code.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "TagGame Input", menuName = "8S/GameSO/TagGame")]
public class TagGameSO : BaseInputSO
{
    public Action<bool> OnL_LeftDir;
    public Action<bool> OnL_RightDir;
    public Action OnL_Jump;
    public Action OnL_Dash;

    public Action <bool> OnR_LeftDir;
    public Action<bool> OnR_RightDir;
    public Action OnR_Jump;
    public Action OnR_Dash;

    public override void OnWKey(InputAction.CallbackContext context)
    {
        base.OnWKey(context);
        if (context.performed) OnL_Jump.Invoke();
    }

    public override void OnAKey(InputAction.CallbackContext context)
    {
        OnL_LeftDir.Invoke(context.performed);
    }
    public override void OnSKey(InputAction.CallbackContext context)
    {
        base.OnSKey(context);
        if (context.performed) OnL_Dash.Invoke();
    }
    public override void OnDkey(InputAction.CallbackContext context)
    {
        OnL_RightDir.Invoke(context.performed);
    }


    public override void OnUArrow(InputAction.CallbackContext context)
    {
        base.OnUArrow(context);
        if (context.performed) OnR_Jump.Invoke();
    }

    public override void OnLArrow(InputAction.CallbackContext context)
    {
        OnR_LeftDir.Invoke(context.performed);
    }
    public override void OnDArrow(InputAction.CallbackContext context)
    {
        base.OnDArrow(context);
        if (context.performed) OnR_Dash.Invoke();
    }
    public override void OnRArrow(InputAction.CallbackContext context)
    {
        OnR_RightDir.Invoke(context.performed);
    }
}