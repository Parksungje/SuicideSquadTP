using Code.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "TagGame Input", menuName = "8S/GameSO/TagGame")]
public class TagGameSO : BaseInputSO
{
    public Action OnL_LeftDir;
    public Action OnL_RightDir;
    public Action OnL_Jump;
    public Action OnL_Dash;

    public Action OnR_LeftDir;
    public Action OnR_RightDir;
    public Action OnR_Jump;
    public Action OnR_Dash;

    public override void OnWKey(InputAction.CallbackContext context)
    {
        base.OnWKey(context);
        if (context.performed) OnL_Jump.Invoke();
    }

    public override void OnAKey(InputAction.CallbackContext context)
    {
        base.OnAKey(context);
        if (context.performed) OnL_LeftDir.Invoke();
    }
    public override void OnSKey(InputAction.CallbackContext context)
    {
        base.OnSKey(context);
        if (context.performed) OnL_Dash.Invoke();
    }
    public override void OnDkey(InputAction.CallbackContext context)
    {
        base.OnDkey(context);
        if (context.performed) OnL_RightDir.Invoke();
    }


    public override void OnUArrow(InputAction.CallbackContext context)
    {
        base.OnUArrow(context);
        if (context.performed) OnR_Jump.Invoke();
    }

    public override void OnLArrow(InputAction.CallbackContext context)
    {
        base.OnLArrow(context);
        if (context.performed) OnR_LeftDir.Invoke();
    }
    public override void OnDArrow(InputAction.CallbackContext context)
    {
        base.OnDArrow(context);
        if (context.performed) OnR_Dash.Invoke();
    }
    public override void OnRArrow(InputAction.CallbackContext context)
    {
        base.OnRArrow(context);
        if (context.performed) OnR_RightDir.Invoke();
    }
}