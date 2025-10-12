using Code.Agents;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    public class Player : Agent
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        private IMovement _movement;

        public override void Awake()
        {
            base.Awake();
            _movement = GetCompo<IMovement>();
            PlayerInput.OnAttackKeyPressed += HandleAttackKeyPressed;
            PlayerInput.OnJumpKeyPressed += HandleJumpKeyPressed;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void HandleJumpKeyPressed(bool isPressed)
        {
            throw new NotImplementedException();
        }

        private void HandleAttackKeyPressed(bool isPressed)
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
            _movement.SetMovementInput(PlayerInput.Movement);
        }
    }
}