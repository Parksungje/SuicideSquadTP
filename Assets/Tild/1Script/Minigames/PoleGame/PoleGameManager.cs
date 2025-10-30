using System;
using System.Collections;
using DG.Tweening;
using Tild._1Script.Minigames.Rope;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tild.Minigames.PoleGame
{
    public class PoleGameManager : MonoBehaviour
    {
        public static PoleGameManager instance;
        [SerializeField] private InputSO inputSO;
        
        [SerializeField] private Rigidbody rigid1P;
        [SerializeField] private Rigidbody rigid2P;
        
        [SerializeField] private Transform spawnPoint1P;
        [SerializeField] private Transform spawnPoint2P;
        [SerializeField] private PoleCoconut coconutPrefab;

        private bool _canRotate1P = true;
        private bool _isLeft1P = true;
        private bool _canClimb1P = true;
        private bool _canRotate2P = true;
        private bool _isLeft2P = true;
        private bool _canClimb2P = true;

        private float _fallSpeed = 5;
        private float _spawnTime = 7;
        private float _playedTime = 0;
        void Awake()
        {
            if (instance == null) 
                instance = this; 
            
            else if (instance != this) 
                Destroy(gameObject);
        }

        private void Update()
        {
            _playedTime += Time.deltaTime;
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

        private IEnumerator Start()
        {
            while (true)
            {
                Transform spawnPoint;
                Vector3 offset;
                
                if (Random.value < 0.5f)
                {
                    spawnPoint = spawnPoint1P;
                    offset = new Vector3(-1.5f, 0, 0);
                }
                else
                {
                    spawnPoint = spawnPoint2P;
                    offset = new Vector3(1.5f, 0, 0);
                }

                PoleCoconut coconut = Instantiate(
                    coconutPrefab,
                    spawnPoint.position + offset,
                    Quaternion.identity);

                coconut.Shoot(Random.Range(_fallSpeed, _fallSpeed + 10));

            
                yield return new WaitForSeconds(_spawnTime);

      
                _playedTime += _spawnTime;

                _spawnTime = Mathf.Max(0.5f, _spawnTime - 0.2f);
                _fallSpeed += 1.5f;

                if (_playedTime >= 120f)
                {
                    Debug.Log("게임 종료");
                    Time.timeScale = 0f;
                    yield break;
                }
            }
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
                
                StartCoroutine(CoolTime1P(0.1f));
                   
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

                StartCoroutine(CoolTime2P(0.1f));

            }
        }
        private IEnumerator CoolTime1P(float time)
        {
            yield return new WaitForSeconds(time);
            _canClimb1P = true;
            rigid1P.linearVelocity = Vector3.zero;
        }
        private IEnumerator CoolTime2P(float time)
        {
            yield return new WaitForSeconds(time);
            _canClimb2P = true;
            rigid2P.linearVelocity = Vector3.zero;
        }

        public void GetFall(Rigidbody rigidbody)
        {
            if (rigidbody == rigid1P)
            StartCoroutine(CoolTime1P(0.5f));
            if (rigidbody == rigid2P)
                StartCoroutine(CoolTime2P(0.5f));
        }
       
    }
}