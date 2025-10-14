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

        [SerializeField] private LayerMask whatIsGround;

        public Action<bool> OnAttackKeyPressed;
        public Action<bool> OnJumpKeyPressed;

        private Vector3 _prevMousePosition;
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
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnAttackKeyPressed?.Invoke(true);
            if (context.canceled)
                OnAttackKeyPressed?.Invoke(false);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnJumpKeyPressed?.Invoke(true);
            if (context.canceled)
                OnJumpKeyPressed?.Invoke(false);
        }

    }
}