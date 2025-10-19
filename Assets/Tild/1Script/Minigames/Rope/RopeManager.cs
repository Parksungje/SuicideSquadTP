using System;
using Code.Player;
using Tild.Minigames.BalanceGame;
using UnityEngine;

namespace Tild._1Script.Minigames.Rope
{
    public class RopeManager : MonoBehaviour
    {
        [SerializeField] private BaseInputSO baseInputSO;

        private bool _isPlaying;
        private bool _1PLeft;
        private bool _1PRight;
        private bool _2PLeft;
        private bool _2PRight;
        
        private void OnEnable()
        {
            baseInputSO.OnAKeyPressed = (bool a) =>
            {
                
                if (!_isPlaying) return;
              
                _1PLeft = true;
        
            };
            baseInputSO.OnDKeyPressed = (bool a) =>
            {
                if (!_isPlaying) return;
          
                _1PLeft = false;
                _1PRight = true;
            };
            baseInputSO.OnLeftArrowPressed = (bool a) =>
            {
                if (!_isPlaying) return;
              
                _2PLeft = true;
                _2PRight = false;
            };
            baseInputSO.OnRightArrowPressed = (bool a) =>
            {
                if (!_isPlaying) return;
              
                _2PLeft = false;
                _2PRight = true;
            };
        }
    }
}