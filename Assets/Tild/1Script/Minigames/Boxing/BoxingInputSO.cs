using System;
using Code.Player;
using UnityEngine;

namespace Tild.Minigames.Boxing
{
    [CreateAssetMenu(fileName = "Boxing Input", menuName = "Inputs/Boxing", order = 1)]
    public class BoxingInputSO : BaseInputSO
    {
        public Action AKeyPressed;
        public Action<bool> SKeyPressed;
        public Action DKeyPressed;
        public Action LeftKeyPressed;
        public Action<bool> DownKeyPressed;
        public Action RightKeyPressed;
        protected override void OnEnable()
        {
            base.OnEnable();
            OnAKeyPressed += HandleAKey;
            OnDKeyPressed += HandleDKey;
            OnSKeyPressed += HandleSKey;
            OnLeftArrowPressed += HandleLeftKey;
            OnDownArrowPressed += HandleDownKey;
            OnRightArrowPressed += HandleRightKey;
        }

        private void HandleRightKey(bool obj) => RightKeyPressed?.Invoke();
        private void HandleLeftKey(bool obj) => LeftKeyPressed?.Invoke();
        private void HandleDownKey(bool obj) => DownKeyPressed?.Invoke(obj);
        private void HandleAKey(bool obj) => AKeyPressed?.Invoke();
        private void HandleDKey(bool obj) => DKeyPressed?.Invoke();
        private void HandleSKey(bool obj) => SKeyPressed?.Invoke(obj);

        protected override void OnDisable()
        {
            base.OnDisable();
            OnAKeyPressed -= HandleAKey;
            OnDKeyPressed -= HandleDKey;
            OnSKeyPressed -= HandleSKey;
            OnLeftArrowPressed -= HandleLeftKey;
            OnRightArrowPressed -= HandleRightKey;
            OnDownArrowPressed -= HandleDownKey;
        }
    }


}