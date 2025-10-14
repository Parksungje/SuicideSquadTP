using Code.Agents;
using Code.Player;
using System;
using UnityEngine;

namespace Code.Players
{
    public class Player : Agent
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        private IMovement _movement;
        private WeaponController _weaponController;
        private CharacterMovement _characterMovement;

        public override void Awake()
        {
            base.Awake();
            _movement = GetCompo<IMovement>();
            _characterMovement = GetCompo<CharacterMovement>();
            _weaponController = GetCompo<WeaponController>();
            PlayerInput.OnJumpKeyPressed += HandleJumpKeyPressed;
            PlayerInput.OnAttackKeyPressed += HandleAttackKeyPressed;
        }

        private void HandleAttackKeyPressed(bool isPressed)
        {
            _weaponController.SetAttacking(isPressed);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void HandleJumpKeyPressed(bool isRunning)
        {
            _characterMovement.Jump();
        }

        private void Update()
        {
            _movement.SetMovementInput(PlayerInput.Movement);
        }
    }
}