using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Tild.Menu;
using System.Collections;

public class AvoidMovementManager : MonoBehaviour
{
    [SerializeField] private AvoidBallSO _avoidInput;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _sprintMultiplier = 1.6f;
    [SerializeField] private KeyCode _p1SprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _p2SprintKey = KeyCode.RightShift;
    [SerializeField] private Rigidbody _p1Rb;
    [SerializeField] private Rigidbody _p2Rb;
    [SerializeField] private Animator _p1Animator;
    [SerializeField] private Animator _p2Animator;
    [SerializeField] private float _yLoseThreshold = -20f;
    [SerializeField] private float _zLoseThreshold = -80f;
    [SerializeField] private int _totalRounds = 5;
    [SerializeField] private float _betweenRoundDelay = 1.2f;
    [SerializeField] private CanvasGroup _roundWinPanel;
    [SerializeField] private TMP_Text _roundWinText;
    [SerializeField] private CanvasGroup _finalWinPanel;
    [SerializeField] private TMP_Text _finalWinText;

    private Vector3 _p1moveDir;
    private Vector3 _p2moveDir;
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
    private bool wPressed, aPressed, sPressed, dPressed, upPressed, leftPressed, downPressed, rightPressed;
    private bool _p1Active = true;
    private bool _p2Active = true;
    private bool _acceptInput = true;
    private int _currentRound = 1;
    private int _p1RoundWins = 0;
    private int _p2RoundWins = 0;
    private bool _roundOver = false;
    private bool _gameEnded = false;
    private Vector3 _p1StartPos;
    private Vector3 _p2StartPos;
    private Quaternion _p1StartRot;
    private Quaternion _p2StartRot;
    private bool _p1Sprint;
    private bool _p2Sprint;
    private bool _isRunningSoundPlaying;

    private void Start()
    {
        SoundManager.Instance.Play("AvoidBall_BGM");
        _p1StartPos = _p1Rb.position;
        _p2StartPos = _p2Rb.position;
        _p1StartRot = _p1Rb.rotation;
        _p2StartRot = _p2Rb.rotation;
        if (_roundWinPanel) { _roundWinPanel.alpha = 0; _roundWinPanel.interactable = false; _roundWinPanel.blocksRaycasts = false; _roundWinPanel.gameObject.SetActive(false); }
        if (_finalWinPanel) { _finalWinPanel.alpha = 0; _finalWinPanel.interactable = false; _finalWinPanel.blocksRaycasts = false; _finalWinPanel.gameObject.SetActive(false); }
    }

    public void DisablePlayer(int playerIndex)
    {
        if (playerIndex == 1) _p1Active = false;
        else if (playerIndex == 2) _p2Active = false;
    }

    private void OnEnable()
    {
        if (_avoidInput == null) return;
        _avoidInput.OnWKeyDown += OnWKey;
        _avoidInput.OnSKeyDown += OnSKey;
        _avoidInput.OnAKeyDown += OnAKey;
        _avoidInput.OnDKeyDown += OnDKey;
        _avoidInput.OnUpArrowDown += OnUpKey;
        _avoidInput.OnDownArrowDown += OnDownKey;
        _avoidInput.OnLeftArrowDown += OnLeftKey;
        _avoidInput.OnRightArrowDown += OnRightKey;
    }

    private void OnDisable()
    {
        if (_avoidInput == null) return;
        _avoidInput.OnWKeyDown -= OnWKey;
        _avoidInput.OnSKeyDown -= OnSKey;
        _avoidInput.OnAKeyDown -= OnAKey;
        _avoidInput.OnDKeyDown -= OnDKey;
        _avoidInput.OnUpArrowDown -= OnUpKey;
        _avoidInput.OnDownArrowDown -= OnDownKey;
        _avoidInput.OnLeftArrowDown -= OnLeftKey;
        _avoidInput.OnRightArrowDown -= OnRightKey;
    }

    private void OnWKey(bool pressed) { if (_acceptInput && !_gameEnded) wPressed = pressed; }
    private void OnSKey(bool pressed) { if (_acceptInput && !_gameEnded) sPressed = pressed; }
    private void OnAKey(bool pressed) { if (_acceptInput && !_gameEnded) aPressed = pressed; }
    private void OnDKey(bool pressed) { if (_acceptInput && !_gameEnded) dPressed = pressed; }
    private void OnUpKey(bool pressed) { if (_acceptInput && !_gameEnded) upPressed = pressed; }
    private void OnDownKey(bool pressed) { if (_acceptInput && !_gameEnded) downPressed = pressed; }
    private void OnLeftKey(bool pressed) { if (_acceptInput && !_gameEnded) leftPressed = pressed; }
    private void OnRightKey(bool pressed) { if (_acceptInput && !_gameEnded) rightPressed = pressed; }

    private void FixedUpdate()
    {
        if (_gameEnded) return;

        _p1moveDir = Vector3.zero;
        _p2moveDir = Vector3.zero;

        if (_p1Active)
        {
            if (wPressed) _p1moveDir += Vector3.forward;
            if (sPressed) _p1moveDir += Vector3.back;
            if (aPressed) _p1moveDir += Vector3.left;
            if (dPressed) _p1moveDir += Vector3.right;
        }

        if (_p2Active)
        {
            if (upPressed) _p2moveDir += Vector3.forward;
            if (downPressed) _p2moveDir += Vector3.back;
            if (leftPressed) _p2moveDir += Vector3.left;
            if (rightPressed) _p2moveDir += Vector3.right;
        }

        _p1Sprint = _p1Active && Input.GetKey(_p1SprintKey);
        _p2Sprint = _p2Active && Input.GetKey(_p2SprintKey);

        ApplyMovement();

        _p1Animator.SetBool(IsRunningHash, _p1Active && _p1moveDir != Vector3.zero);
        _p2Animator.SetBool(IsRunningHash, _p2Active && _p2moveDir != Vector3.zero);

        HandleRunningSound();

        if (!_roundOver)
        {
            bool p1Lose = _p1Rb.position.y < _yLoseThreshold || _p1Rb.position.z < _zLoseThreshold;
            bool p2Lose = _p2Rb.position.y < _yLoseThreshold || _p2Rb.position.z < _zLoseThreshold;
            if (p1Lose && !p2Lose) EndRound(2);
            else if (p2Lose && !p1Lose) EndRound(1);
            else if (p1Lose && p2Lose) EndRound(0);
        }
    }

    private void ApplyMovement()
    {
        if (_p1Active && _p1moveDir != Vector3.zero)
        {
            float speed = _moveSpeed * (_p1Sprint ? _sprintMultiplier : 1f);
            _p1Rb.MovePosition(_p1Rb.position + _p1moveDir * speed * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(_p1moveDir);
            _p1Rb.MoveRotation(Quaternion.Slerp(_p1Rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }

        if (_p2Active && _p2moveDir != Vector3.zero)
        {
            float speed = _moveSpeed * (_p2Sprint ? _sprintMultiplier : 1f);
            _p2Rb.MovePosition(_p2Rb.position + _p2moveDir * speed * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(_p2moveDir);
            _p2Rb.MoveRotation(Quaternion.Slerp(_p2Rb.rotation, targetRotation, Time.fixedDeltaTime * 10f));
        }
    }

    private void HandleRunningSound()
    {
        bool anyRunning = (_p1Active && _p1moveDir != Vector3.zero) || (_p2Active && _p2moveDir != Vector3.zero);

        if (anyRunning && !_isRunningSoundPlaying)
        {
            SoundManager.Instance.Play("AvoidBall_Run");
            _isRunningSoundPlaying = true;
        }
        else if (!anyRunning && _isRunningSoundPlaying)
        {
            SoundManager.Instance.Stop("AvoidBall_Run");
            _isRunningSoundPlaying = false;
        }
    }

    public void EnablePlayer(int playerIndex)
    {
        if (playerIndex == 1) _p1Active = true;
        else if (playerIndex == 2) _p2Active = true;
    }

    public void EnableAllPlayers()
    {
        _p1Active = true;
        _p2Active = true;
    }

    private void EndRound(int winnerPlayer)
    {
        if (_roundOver || _gameEnded) return;
        _roundOver = true;
        _acceptInput = false;
        _p1Active = false;
        _p2Active = false;

        if (winnerPlayer == 1) _p1RoundWins++;
        else if (winnerPlayer == 2) _p2RoundWins++;

        if (_roundWinPanel)
        {
            _roundWinText.text = winnerPlayer == 0 ? "¹«½ÂºÎ" : (winnerPlayer == 1 ? "P1 ¶ó¿îµå ½Â!" : "P2 ¶ó¿îµå ½Â!");
            _roundWinPanel.gameObject.SetActive(true);
            _roundWinPanel.alpha = 0f;
            _roundWinPanel.DOFade(1f, 0.3f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(_betweenRoundDelay, () =>
                {
                    _roundWinPanel.DOFade(0f, 0.25f).OnComplete(() =>
                    {
                        _roundWinPanel.gameObject.SetActive(false);
                        StartCoroutine(NextRoundOrFinish());
                    });
                });
            });
        }
        else
        {
            StartCoroutine(NextRoundOrFinish());
        }
    }

    private IEnumerator NextRoundOrFinish()
    {
        yield return null;

        if (_p1RoundWins >= 3 || _p2RoundWins >= 3 || _currentRound >= _totalRounds)
        {
            _gameEnded = true;
            bool is1Pwin = _p1RoundWins > _p2RoundWins;
            if (_finalWinPanel)
            {
                _finalWinText.text = is1Pwin ? "P1 ÃÖÁ¾ ½Â¸®!" : "P2 ÃÖÁ¾ ½Â¸®!";
                _finalWinPanel.gameObject.SetActive(true);
                _finalWinPanel.alpha = 0f;
                _finalWinPanel.DOFade(1f, 0.45f).OnComplete(() =>
                {
                    MinigameManager.instance?.Finish(is1Pwin);
                });
            }
            else
            {
                MinigameManager.instance?.Finish(is1Pwin);
            }
            yield break;
        }

        _currentRound++;
        ResetToStart();
        _roundOver = false;
        _acceptInput = true;
        _p1Active = true;
        _p2Active = true;
    }

    private void ResetToStart()
    {
        _p1Rb.linearVelocity = Vector3.zero;
        _p1Rb.angularVelocity = Vector3.zero;
        _p2Rb.linearVelocity = Vector3.zero;
        _p2Rb.angularVelocity = Vector3.zero;
        _p1Rb.position = _p1StartPos;
        _p2Rb.position = _p2StartPos;
        _p1Rb.rotation = _p1StartRot;
        _p2Rb.rotation = _p2StartRot;
        _p1Animator.SetBool(IsRunningHash, false);
        _p2Animator.SetBool(IsRunningHash, false);
        wPressed = aPressed = sPressed = dPressed = false;
        upPressed = leftPressed = downPressed = rightPressed = false;
        _p1Sprint = false;
        _p2Sprint = false;
        _isRunningSoundPlaying = false;
        SoundManager.Instance.Stop("AvoidBall_Run");
    }
}
