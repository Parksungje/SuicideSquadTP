using System;
using Code.Player;
using UnityEngine;

namespace Tild.Minigames.PoleGame
{
    [CreateAssetMenu(fileName = "Pole Input", menuName = "Inputs/Pole", order = 1)]
    public class PoleInputSO : BaseInputSO
    {
        public Action AKeyPressed;
        public Action DKeyPressed;
        public Action WKeyPressed;
        public Action LeftKeyPressed;
        public Action UpKeyPressed;
        public Action RightKeyPressed;
        protected override void OnEnable()
        {
            base.OnEnable();
            OnAKeyPressed += HandleAKey;
            OnDKeyPressed += HandleDKey;
            OnWKeyPressed += HandleWKey;
         
          
            OnUpArrowPressed += HandleUpKey;
            OnLeftArrowPressed += HandleLeftKey;
            OnRightArrowPressed += HandleRightKey;
            
        }

        private void HandleRightKey(bool obj) => RightKeyPressed?.Invoke();
     
        private void HandleUpKey(bool obj) => UpKeyPressed?.Invoke();
        private void HandleLeftKey(bool obj) => LeftKeyPressed?.Invoke();
        private void HandleAKey(bool obj) => AKeyPressed?.Invoke();
        private void HandleDKey(bool obj) => DKeyPressed?.Invoke();
        private void HandleWKey(bool obj) => WKeyPressed?.Invoke();


        protected override void OnDisable()
        {
            base.OnDisable();
            OnAKeyPressed -= HandleAKey;
            OnDKeyPressed -= HandleDKey;
            OnLeftArrowPressed -= HandleLeftKey;
            OnRightArrowPressed -= HandleRightKey;
            OnUpArrowPressed -= HandleUpKey;
            OnWKeyPressed -= HandleWKey;
            
        }
    }
}