using Code.Agents;
using Code.Player;
using System;
using UnityEngine;

namespace Code.Players
{
    public class Player : Agent
    {
        [field: SerializeField] public BaseInputSO PlayerInput { get; private set; }


        public override void Awake()
        {
            base.Awake();
            PlayerInput.OnWKeyPressed += HandleWKeyPressed;
            PlayerInput.OnSKeyPressed += HandleSKeyPressed;
            PlayerInput.OnAKeyPressed += HandleAKeyPressed;
            PlayerInput.OnDKeyPressed += HandleDKeyPressed;
        }

        private void HandleWKeyPressed(bool isPressed)
        {
            throw new NotImplementedException();
        }

        private void HandleSKeyPressed(bool isPressed)
        {
            throw new NotImplementedException();
        }

        private void HandleAKeyPressed(bool isPressed)
        {
            throw new NotImplementedException();
        }

        private void HandleDKeyPressed(bool isPressed)
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}