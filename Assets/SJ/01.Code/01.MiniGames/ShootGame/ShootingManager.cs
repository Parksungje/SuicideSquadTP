using Code.Player;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Tild.Menu;

public class ShootingManager : MonoBehaviour
{
    [field: SerializeField] private ShootGameSO _shootGameSO;
    [SerializeField] private Rigidbody _p1CrossHair;
    [SerializeField] private Rigidbody _p2CrossHair;
    [SerializeField] private Rigidbody _p1Obj;
    [SerializeField] private Rigidbody _p2Obj;
    [SerializeField] private CrossHairComponent _p1CrossHairComponent;
    [SerializeField] private CrossHairComponent _p2CrossHairComponent;
    [SerializeField] private Animator _p1Animator;
    [SerializeField] private Animator _p2Animator;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _shootCooldown = 0.5f;
    [SerializeField] private GameObject _scorePopupPrefab;
    [SerializeField] private Canvas _worldCanvas;
    [SerializeField] private TMP_Text _p1ScoreText;
    [SerializeField] private TMP_Text _p2ScoreText;
    [SerializeField] private int _totalRounds = 3;
    [SerializeField] private int _roundTargetScore = 300;
    [SerializeField] private float _betweenRoundDelay = 1.25f;
    [SerializeField] private CanvasGroup _roundWinPanel;
    [SerializeField] private TMP_Text _roundWinText;
    [SerializeField] private CanvasGroup _finalWinPanel;
    [SerializeField] private TMP_Text _finalWinText;

    private Vector3 _p1HairDir;
    private Vector3 _p2HairDir;
    private bool _wPressed, _sPressed, _aPressed, _dPressed, _upArrowPressed, _downArrowPressed, _leftArrowPressed, _rightArrowPressed;
    private float _p1LastShootTime;
    private float _p2LastShootTime;
    private int _p1Score;
    private int _p2Score;
    private int _currentRound = 1;
    private int _p1RoundWins = 0;
    private int _p2RoundWins = 0;
    private bool _roundOver = false;
    private bool _gameEnded = false;
    private bool _acceptInput = true;

    private void Awake()
    {
        SoundManager.Instance.Play("Shooting_BGM");
        _p1HairDir = Vector3.zero;
        _p2HairDir = Vector3.zero;
        _p1LastShootTime = -_shootCooldown;
        _p2LastShootTime = -_shootCooldown;
        _p1Score = 0;
        _p2Score = 0;
        UpdateScoreUI();
        if (_roundWinPanel) { _roundWinPanel.alpha = 0; _roundWinPanel.interactable = false; _roundWinPanel.blocksRaycasts = false; }
        if (_finalWinPanel) { _finalWinPanel.alpha = 0; _finalWinPanel.interactable = false; _finalWinPanel.blocksRaycasts = false; }
    }

    private void OnEnable()
    {
        if (_shootGameSO == null) return;
        _shootGameSO.OnWKeyDown += SetP1W;
        _shootGameSO.OnSKeyDown += SetP1S;
        _shootGameSO.OnAKeyDown += SetP1A;
        _shootGameSO.OnDKeyDown += SetP1D;
        _shootGameSO.OnUpArrowDown += SetP2UpArrow;
        _shootGameSO.OnDownArrowDown += SetP2DownArrow;
        _shootGameSO.OnLeftArrowDown += SetP2LeftArrow;
        _shootGameSO.OnRightArrowDown += SetP2RightArrow;
        _shootGameSO.OnEKeyDown += SetP1Shoot;
        _shootGameSO.OnEnterKeyDown += SetP2Shoot;
    }

    private void OnDisable()
    {
        if (_shootGameSO == null) return;
        _shootGameSO.OnWKeyDown -= SetP1W;
        _shootGameSO.OnSKeyDown -= SetP1S;
        _shootGameSO.OnAKeyDown -= SetP1A;
        _shootGameSO.OnDKeyDown -= SetP1D;
        _shootGameSO.OnUpArrowDown -= SetP2UpArrow;
        _shootGameSO.OnDownArrowDown -= SetP2DownArrow;
        _shootGameSO.OnLeftArrowDown -= SetP2LeftArrow;
        _shootGameSO.OnRightArrowDown -= SetP2RightArrow;
        _shootGameSO.OnEKeyDown -= SetP1Shoot;
        _shootGameSO.OnEnterKeyDown -= SetP2Shoot;
    }

    private void SetP1W(bool isPressed) { if (_acceptInput && !_gameEnded) _wPressed = isPressed; }
    private void SetP1S(bool isPressed) { if (_acceptInput && !_gameEnded) _sPressed = isPressed; }
    private void SetP1A(bool isPressed) { if (_acceptInput && !_gameEnded) _aPressed = isPressed; }
    private void SetP1D(bool isPressed) { if (_acceptInput && !_gameEnded) _dPressed = isPressed; }
    private void SetP2UpArrow(bool isPressed) { if (_acceptInput && !_gameEnded) _upArrowPressed = isPressed; }
    private void SetP2DownArrow(bool isPressed) { if (_acceptInput && !_gameEnded) _downArrowPressed = isPressed; }
    private void SetP2LeftArrow(bool isPressed) { if (_acceptInput && !_gameEnded) _leftArrowPressed = isPressed; }
    private void SetP2RightArrow(bool isPressed) { if (_acceptInput && !_gameEnded) _rightArrowPressed = isPressed; }

    private void SetP1Shoot(bool isPressed)
    {
        if (!_acceptInput || _roundOver || _gameEnded) return;
        if (isPressed && Time.time - _p1LastShootTime >= _shootCooldown)
        {
            _p1LastShootTime = Time.time;
            AttemptShoot(_p1CrossHairComponent, _p1Animator, 1);
        }
    }

    private void SetP2Shoot(bool isPressed)
    {
        if (!_acceptInput || _roundOver || _gameEnded) return;
        if (isPressed && Time.time - _p2LastShootTime >= _shootCooldown)
        {
            _p2LastShootTime = Time.time;
            AttemptShoot(_p2CrossHairComponent, _p2Animator, 2);
        }
    }

    private void FixedUpdate()
    {
        if (_gameEnded) return;
        UpdateP1Direction();
        UpdateP2Direction();
        _p1CrossHair.linearVelocity = _p1HairDir.normalized * _moveSpeed;
        _p2CrossHair.linearVelocity = _p2HairDir.normalized * _moveSpeed;
        Vector3 p1TargetPos = _p1CrossHair.position;
        p1TargetPos.y = _p1Obj.transform.position.y;
        _p1Obj.transform.LookAt(p1TargetPos);
        Vector3 p2TargetPos = _p2CrossHair.position;
        p2TargetPos.y = _p2Obj.transform.position.y;
        _p2Obj.transform.LookAt(p2TargetPos);
    }

    private void UpdateP1Direction()
    {
        _p1HairDir = Vector3.zero;
        if (_wPressed) _p1HairDir += Vector3.up;
        if (_sPressed) _p1HairDir += Vector3.down;
        if (_aPressed) _p1HairDir += Vector3.left;
        if (_dPressed) _p1HairDir += Vector3.right;
    }

    private void UpdateP2Direction()
    {
        _p2HairDir = Vector3.zero;
        if (_upArrowPressed) _p2HairDir += Vector3.up;
        if (_downArrowPressed) _p2HairDir += Vector3.down;
        if (_leftArrowPressed) _p2HairDir += Vector3.left;
        if (_rightArrowPressed) _p2HairDir += Vector3.right;
    }

    private void AttemptShoot(CrossHairComponent crossHair, Animator animator, int playerNumber)
    {
        if (crossHair == null || _roundOver || _gameEnded) return;

        SoundManager.Instance.Play("Shooting_Fire");

        if (animator != null)
        {
            animator.SetBool("isFire", true);
            Invoke(nameof(ResetFireFlags), 0.1f);
        }

        TargetComponent target = crossHair.GetCurrentTarget();
        if (target != null)
        {
            target.OnHit();

            SoundManager.Instance.Play("Shooting_Target");

            if (playerNumber == 1)
            {
                _p1Score += 20;
                _p1ScoreText.text = _p1Score.ToString();
                ShowScorePopup(crossHair.transform.position, "+20", Color.red);
                if (_p1Score >= _roundTargetScore) EndRound(1);
            }
            else
            {
                _p2Score += 20;
                _p2ScoreText.text = _p2Score.ToString();
                ShowScorePopup(crossHair.transform.position, "+20", Color.blue);
                if (_p2Score >= _roundTargetScore) EndRound(2);
            }
        }
    }

    private void EndRound(int winnerPlayer)
    {
        if (_roundOver || _gameEnded) return;
        _roundOver = true;
        _acceptInput = false;
        if (winnerPlayer == 1) _p1RoundWins++; else _p2RoundWins++;
        if (_roundWinPanel)
        {
            _roundWinText.text = winnerPlayer == 1 ? "P1 ½Â¸®!" : "P2 ½Â¸®!";
            _roundWinPanel.alpha = 0;
            _roundWinPanel.gameObject.SetActive(true);
            _roundWinPanel.DOFade(1f, 0.35f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(_betweenRoundDelay, () =>
                {
                    _roundWinPanel.DOFade(0.35f, 0.25f).OnComplete(() =>
                    {
                        _roundWinPanel.alpha = 0;
                        _roundWinPanel.interactable = false;
                        _roundWinPanel.blocksRaycasts = false;
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
        yield return new WaitForSeconds(0.01f);
        if (_currentRound < _totalRounds)
        {
            _currentRound++;
            _p1Score = 0;
            _p2Score = 0;
            UpdateScoreUI();
            _p1LastShootTime = Time.time;
            _p2LastShootTime = Time.time;
            _roundOver = false;
            _acceptInput = true;
        }
        else
        {
            _gameEnded = true;
            bool is1Pwin = _p1RoundWins > _p2RoundWins;
            if (_finalWinPanel)
            {
                _finalWinText.text = "°ÔÀÓ Á¾·á!"; //is1Pwin ? "P1 ÃÖÁ¾ ½Â¸®!" : "P2 ÃÖÁ¾ ½Â¸®!";
                _finalWinPanel.alpha = 0;
                _finalWinPanel.gameObject.SetActive(true);
                _finalWinPanel.DOFade(1f, 0.5f).OnComplete(() =>
                {
                    MinigameManager.instance?.Finish(is1Pwin);
                });
            }
            else
            {
                MinigameManager.instance?.Finish(is1Pwin);
            }
        }
    }

    private void ResetFireFlags()
    {
        if (_p1Animator != null) _p1Animator.SetBool("isFire", false);
        if (_p2Animator != null) _p2Animator.SetBool("isFire", false);
    }

    private void ShowScorePopup(Vector3 worldPos, string message, Color color)
    {
        if (_scorePopupPrefab == null || _worldCanvas == null) return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        GameObject popup = Instantiate(_scorePopupPrefab, _worldCanvas.transform);
        popup.transform.position = screenPos;
        var popupComp = popup.GetComponent<ScorePopup>();
        if (popupComp != null) popupComp.Show(message, color);
    }

    private void UpdateScoreUI()
    {
        if (_p1ScoreText != null) _p1ScoreText.text = _p1Score.ToString();
        if (_p2ScoreText != null) _p2ScoreText.text = _p2Score.ToString();
    }
}
