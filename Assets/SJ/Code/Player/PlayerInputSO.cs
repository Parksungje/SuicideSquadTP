using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "Player input", menuName = "SO/Player input", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public Vector2 Movement { get; private set; }
        public Vector2 MousePosition { get; private set; }


        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {   
            _controls.Player.Disable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            Movement = context.ReadValue<Vector2>();
        }

        public void OnW(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnS(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnA(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnD(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnLArrow(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnRArrow(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnDArrow(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnUArrow(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}