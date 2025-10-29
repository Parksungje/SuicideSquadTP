using System;
using DG.Tweening;
using Tild._1Script.Minigames.Rope;
using UnityEngine;

namespace Tild.Minigames.PoleGame
{
    public class PoleGameManager : MonoBehaviour
    {
        public static PoleGameManager instance;
        [SerializeField] private InputSO inputSO;
        
        [SerializeField] private Rigidbody rigid1P;
        [SerializeField] private Rigidbody rigid2P;

        private bool _canRotate1P = true;
        private bool _isLeft1P = true;
        private bool _canClimb1P = true;
        private bool _canRotate2P = true;
        private bool _isLeft2P = true;
        private bool _canClimb2P = true;
        void Awake()
        {
            if (instance == null) 
                instance = this; 
            
            else if (instance != this) 
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            inputSO.OnAKeyPressed += Rotate1P;
            inputSO.OnDKeyPressed += Rotate1P;
            inputSO.OnWKeyPressed += Climb1P;
            inputSO.OnLeftArrowPressed += Rotate2P;
            inputSO.OnRightArrowPressed += Rotate2P;
            inputSO.OnUpArrowPressed += Climb2P;
        }

        private void Rotate1P(bool obj)
        {
            if (_canRotate1P)
            {
                _canRotate1P = false;

                if (_isLeft1P)
                {
                    rigid1P.transform.DORotate(new Vector3(0, 180, 0), 0.5f, RotateMode.Fast)
                        .OnComplete(() =>
                        {
                            _isLeft1P = false;
                            _canRotate1P = true;
                        });
                }
                else
                {
                    rigid1P.transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)
                        .OnComplete(() =>
                        {
                            _isLeft1P = true;
                            _canRotate1P = true;
                        });
                }
            }
        }
       
        private void Climb1P(bool obj)
        {
            if (_canClimb1P)
            {
                _canClimb1P = false;
                rigid1P.linearVelocity += (Vector3.up * 15f);
                
                Invoke("CoolTime1P", 0.1f);
                   
            }
        }
        
        private void Rotate2P(bool obj)
        {
            if (_canRotate2P)
            {
                _canRotate2P = false;

                if (_isLeft2P)
                {
                    rigid2P.transform.DORotate(new Vector3(0, 180, 0), 0.5f, RotateMode.Fast)
                        .OnComplete(() =>
                        {
                            _isLeft2P = false;
                            _canRotate2P = true;
                        });
                }
                else
                {
                    rigid2P.transform.DORotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast)
                        .OnComplete(() =>
                        {
                            _isLeft2P = true;
                            _canRotate2P = true;
                        });
                }
            }
        }
       
        private void Climb2P(bool obj)
        {
            if (_canClimb2P)
            {
                _canClimb2P = false;
                rigid2P.linearVelocity += (Vector3.up * 15f);
                
                Invoke("CoolTime2P", 0.1f);
                   
            }
        }
        private void CoolTime1P()
        {
            _canClimb1P = true;
            rigid1P.linearVelocity = Vector3.zero;
        }
        private void CoolTime2P()
        {
            _canClimb2P = true;
            rigid2P.linearVelocity = Vector3.zero;
        }
    }
}