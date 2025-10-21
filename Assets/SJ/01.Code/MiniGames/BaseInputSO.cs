using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    public abstract class BaseInputSO : ScriptableObject, Controls.IPlayerActions
    {
        protected Controls _controls;
        public Vector2 Movement { get; private set; }

        public Action<bool> OnWKeyPressed;
        public Action<bool> OnSKeyPressed;
        public Action<bool> OnAKeyPressed;
        public Action<bool> OnDKeyPressed;

        public Action<bool> OnLeftArrowPressed;
        public Action<bool> OnRightArrowPressed;
        public Action<bool> OnDownArrowPressed;
        public Action<bool> OnUpArrowPressed;

        public Action<bool> OnConfirmPressed;

        protected virtual void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        protected virtual void OnDisable()
        {
            _controls.Player.Disable();
        }

        public virtual void OnMove(InputAction.CallbackContext context)
        {
            Movement = context.ReadValue<Vector2>();
        }

        public virtual void OnWKey(InputAction.CallbackContext context)
        {
            OnWKeyPressed?.Invoke(context.performed);
        }

        public virtual void OnSKey(InputAction.CallbackContext context)
        {
            OnSKeyPressed?.Invoke(context.performed);
        }

        public virtual void OnAKey(InputAction.CallbackContext context)
        {
            OnAKeyPressed?.Invoke(context.performed);
        }

        public virtual void OnDkey(InputAction.CallbackContext context)
        {
            OnDKeyPressed?.Invoke(context.performed);
        }

        public virtual void OnLArrow(InputAction.CallbackContext context)
        {
            OnLeftArrowPressed?.Invoke(context.performed);
        }

        public virtual void OnRArrow(InputAction.CallbackContext context)
        {
            OnRightArrowPressed?.Invoke(context.performed);
        }

        public virtual void OnDArrow(InputAction.CallbackContext context)
        {
            OnDownArrowPressed?.Invoke(context.performed);
        }

        public virtual void OnUArrow(InputAction.CallbackContext context)
        {
            OnUpArrowPressed?.Invoke(context.performed);
        }

        public virtual void OnSpace(InputAction.CallbackContext context)
        {
            OnConfirmPressed?.Invoke(context.performed);
        }

        public bool IsAnyKeyPressed()
        {
            return Input.anyKey;
        }
    }
}
