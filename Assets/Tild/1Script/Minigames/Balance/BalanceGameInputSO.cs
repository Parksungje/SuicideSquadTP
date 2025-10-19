using System;
using Code.Player;
using UnityEngine;

namespace Tild.Minigames.BalanceGame
{
    [CreateAssetMenu(fileName = "BalanceGame Input", menuName = "Inputs/BalanceGame", order = 1)]
    public class BalanceGameInputSO : BaseInputSO
    {
        public Action AKeyPressed;
        public Action DKeyPressed;
        public Action LeftKeyPressed;
        public Action RightKeyPressed;
        protected override void OnEnable()
        {
            base.OnEnable();
            OnAKeyPressed += HandleAKey;
            OnDKeyPressed += HandleDKey;
            OnLeftArrowPressed += HandleLeftKey;
            OnRightArrowPressed += HandleRightKey;
        }

        private void HandleRightKey(bool obj) => RightKeyPressed?.Invoke();
        private void HandleLeftKey(bool obj) => LeftKeyPressed?.Invoke();
        private void HandleAKey(bool obj) => AKeyPressed?.Invoke();
        private void HandleDKey(bool obj) => DKeyPressed?.Invoke();

        protected override void OnDisable()
        {
            base.OnDisable();
            OnAKeyPressed -= HandleAKey;
            OnDKeyPressed -= HandleDKey;
            OnLeftArrowPressed -= HandleLeftKey;
            OnRightArrowPressed -= HandleRightKey;
        }
    }
}