using Code.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "RussianRoulette Input", menuName = "8S/GameSO/RussinRoulette")]
public class RussianRoulletSO : BaseInputSO
{
    public Action OnL_Click;
    public Action OnR_Click;

    public override void OnWKey(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnL_Click.Invoke();
    }

    public override void OnUArrow(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnR_Click.Invoke();
    }
}