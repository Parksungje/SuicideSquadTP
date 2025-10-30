using System;
using System.Collections;
using DG.Tweening;
using Tild._1Script.Minigames.Rope;
using Tild.Menu;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tild.Minigames.PoleGame
{
    public class PoleGameManager : MonoBehaviour
    {
        public static PoleGameManager instance;
        [SerializeField] private InputSO inputSO;
        
        [SerializeField] private Rigidbody rigid1P;
        [SerializeField] private Animator animator1P;
        [SerializeField] private Animator animator2P;
        [SerializeField] private Rigidbody rigid2P;
        
        [SerializeField] private Transform spawnPoint1P;
        [SerializeField] private Transform spawnPoint2P;
        [SerializeField] private PoleCoconut coconutPrefab;
        [SerializeField] private TMP_Text score1P, score2P;

        private bool _canRotate1P = true;
        private bool _isLeft1P = true;
        private bool _canClimb1P = true;
        private bool _canRotate2P = true;
        private bool _isLeft2P = true;
        private bool _canClimb2P = true;

        private float _fallSpeed = 5;
        private float _spawnTime = 2;
        private float _playedTime = 0;

        private float _1PScore = 0;
        private float _2PScore = 0;
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

        private void FixedUpdate()
        {
            _1PScore = rigid1P.transform.position.y;
            _2PScore = rigid2P.transform.position.y;
            score1P.text = $"{_1PScore.ToString()}M";
            score2P.text = $"{_2PScore.ToString()}M";
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
                }
                else
                {
                    spawnPoint = spawnPoint2P;
                }
                if (Random.value < 0.5f)
                {
                    offset = new Vector3(-3.6f, 0, 0);
                }
                else
                {
                    offset = new Vector3(3.6f, 0, 0);
                }

                PoleCoconut coconut = Instantiate(
                    coconutPrefab,
                    spawnPoint.position + offset,
                    Quaternion.identity);

                coconut.Shoot(Random.Range(_fallSpeed, _fallSpeed + 10));

            
                yield return new WaitForSeconds(_spawnTime);

      
                _playedTime += _spawnTime;

                _spawnTime = Random.Range(0.5f, _spawnTime);
                

                if (_playedTime >= 60f)
                {
                    Debug.Log("게임 종료");
                    Time.timeScale = 0f;
                    MinigameManager.instance.Finish(_1PScore > _2PScore);
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
                animator1P.SetTrigger("Climb");
                
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
                animator2P.SetTrigger("Climb");
                StartCoroutine(CoolTime2P(0.1f));

            }
        }
        private IEnumerator CoolTime1P(float time)
        {
            
            yield return new WaitForSeconds(time);
            _canClimb1P = true;
            rigid1P.linearVelocity = Vector3.zero;
        }
        private IEnumerator Fall2P(float time)
        {
            animator2P.SetTrigger("Damaged");
            yield return new WaitForSeconds(time);
            _canClimb2P = true;
            rigid2P.linearVelocity = Vector3.zero;
        }

        private IEnumerator Fall1P(float time)
        {
            animator1P.SetTrigger("Damaged");
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
            StartCoroutine(Fall1P(1f));
            if (rigidbody == rigid2P)
                StartCoroutine(Fall2P(1f));
        }
       
    }
}