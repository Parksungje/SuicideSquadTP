using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "Running Input", menuName = "SJ/GameSO/Running", order = 5)]
    public class RunningGameSO : BaseInputSO
    {
        public Action<bool> OnAKeyDown;
        public Action<bool> OnDKeyDown;

        public Action<bool> OnLeftArrowDown;
        public Action<bool> OnRightArrowDown;

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

    }
}
