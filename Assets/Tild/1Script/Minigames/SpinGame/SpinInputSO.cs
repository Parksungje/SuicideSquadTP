using System;
using Code.Player;
using UnityEngine;

namespace Tild.Minigames.BalanceGame
{
    [CreateAssetMenu(fileName = "SpinInput Input", menuName = "Inputs/SpinInput", order = 1)]
    public class SpinInputSO : BaseInputSO
    {
        public Action AKeyPressed;
        public Action DKeyPressed;
        public Action SKeyPressed;
        public Action WKeyPressed;
        public Action LeftKeyPressed;
        public Action DownKeyPressed;
        public Action UpKeyPressed;
        public Action RightKeyPressed;
        protected override void OnEnable()
        {
            base.OnEnable();
            OnAKeyPressed += HandleAKey;
            OnDKeyPressed += HandleDKey;
            OnWKeyPressed += HandleWKey;
            OnSKeyPressed += HandleSKey;
            OnDownArrowPressed += HandleDownKey;
            OnUpArrowPressed += HandleUpKey;
            OnLeftArrowPressed += HandleLeftKey;
            OnRightArrowPressed += HandleRightKey;
            
        }

        private void HandleRightKey(bool obj) => RightKeyPressed?.Invoke();
        private void HandleDownKey(bool obj) => DownKeyPressed?.Invoke();
        private void HandleUpKey(bool obj) => UpKeyPressed?.Invoke();
        private void HandleLeftKey(bool obj) => LeftKeyPressed?.Invoke();
        private void HandleAKey(bool obj) => AKeyPressed?.Invoke();
        private void HandleDKey(bool obj) => DKeyPressed?.Invoke();
        private void HandleWKey(bool obj) => WKeyPressed?.Invoke();
        private void HandleSKey(bool obj) => SKeyPressed?.Invoke();

        protected override void OnDisable()
        {
            base.OnDisable();
            OnAKeyPressed -= HandleAKey;
            OnDKeyPressed -= HandleDKey;
            OnLeftArrowPressed -= HandleLeftKey;
            OnRightArrowPressed -= HandleRightKey;
            OnSKeyPressed -= HandleSKey;
            OnDownArrowPressed -= HandleDownKey;
            OnUpArrowPressed -= HandleUpKey;
            OnWKeyPressed -= HandleWKey;
            
        }
    }
}