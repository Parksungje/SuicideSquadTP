using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "PenaltyKick Input", menuName = "SJ/GameSO/PenaltyKick", order = 1)]
    public class PenaltyKickSO : BaseInputSO
    {
        public Action OnSConfrim;
        public Action OnSLeftDir;
        public Action OnSRightDir;
        public Action OnSMiddleDir;

        public Action OnKConfrim;
        public Action OnKLeftDir;
        public Action OnKRightDir;
        public Action OnKMiddleDir;

        public Action OnConfirm;

        public override void OnWKey(InputAction.CallbackContext context)
        {
            base.OnWKey(context);

            if (context.performed)
                OnSConfrim?.Invoke();
        }

        public override void OnAKey(InputAction.CallbackContext context)
        {
            base.OnAKey(context);

            if (context.performed)
                OnSLeftDir?.Invoke();
        }

        public override void OnDkey(InputAction.CallbackContext context)
        {
            base.OnDkey(context);

            if (context.performed)
                OnSRightDir?.Invoke();
        }

        public override void OnSKey(InputAction.CallbackContext context)
        {
            base.OnSKey(context);

            if (context.performed)
                OnSMiddleDir?.Invoke();
        }

        public override void OnUArrow(InputAction.CallbackContext context)
        {
            base.OnUArrow(context);

            if (context.performed)
                OnKConfrim?.Invoke();
        }

        public override void OnRArrow(InputAction.CallbackContext context)
        {
            base.OnRArrow(context);

            if (context.performed)
                OnKRightDir?.Invoke();
        }

        public override void OnLArrow(InputAction.CallbackContext context)
        {
            base.OnLArrow(context);

            if (context.performed)
                OnKLeftDir?.Invoke();
        }

        public override void OnDArrow(InputAction.CallbackContext context)
        {
            base.OnDArrow(context);

            if (context.performed)
                OnKMiddleDir?.Invoke();
        }
        public override void OnSpace(InputAction.CallbackContext context)
        {
            base.OnSpace(context);

            if (context.performed)
                OnConfirm?.Invoke();
        }
    }
}
