using Code.Player;
using DG.Tweening;
using System.Collections;
using Tild.Menu;
using TMPro;
using UnityEngine;

namespace Code.PK
{
    public class PenaltyKickController : MonoBehaviour
    {
        [field: SerializeField] private PenaltyKickSO _PKInput;
        [SerializeField] private GameObject _keeperPrefab;
        [SerializeField] private GameObject _startBall;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Transform _orignPos;
        [SerializeField] private GameObject _shooterObj;
        [SerializeField] private Animator _keeperAnimator;
        [SerializeField] private Animator _shooterAnimator;
        [SerializeField] private PKUIManager _uiManager;
        [SerializeField] private float _shootPower = 20f;
        [SerializeField] private float _keeperJumpPower = 10f;
        [SerializeField] private float _runDuration = 0.6f;
        [SerializeField] private float _returnDuration = 0.5f;
        [SerializeField] private float _keeperActionDelay = 0.25f;
        [SerializeField] private float _resetDelay = 3f;

        private static readonly int IsKeepingHash = Animator.StringToHash("isKeeping");
        private static readonly int IsShootingHash = Animator.StringToHash("isShooting");
        private static readonly int IsRunningHash = Animator.StringToHash("isRunning");

        private Rigidbody _keeperRb;
        private Vector3 _keeperOriginPos;
        private GameObject _currentBall;

        private bool IsShoot { get; set; }
        private bool ShooterConfirmed { get; set; }
        private bool KeeperConfirmed { get; set; }
        private bool _isGameOver = false;

        private int ShooterScore { get; set; }
        private int KeeperScore { get; set; }

        private enum Direction { Left, Middle, Right }
        private Direction ShooterDirType { get; set; } = Direction.Middle;
        private Direction KeeperDirType { get; set; } = Direction.Middle;

        private Vector3 ShooterDir { get; set; } = Vector3.forward;
        private Vector3 KeeperDir { get; set; } = Vector3.zero;

        private bool is1Pwin;
        private bool is2Pwin;

        private void Awake()
        {
            _keeperRb = _keeperPrefab.GetComponent<Rigidbody>();
            _uiManager.OnGameEnd += EndGame;
        }

        private void OnEnable()
        {
            _PKInput.OnSConfirm += () => { if (IsShoot || _isGameOver) return; ShooterConfirmed = true; };
            _PKInput.OnSLeftDir += () => { if (IsShoot || _isGameOver) return; SetShooterDirection(Direction.Left, new Vector3(-0.35f, 0.3f, 1f)); };
            _PKInput.OnSRightDir += () => { if (IsShoot || _isGameOver) return; SetShooterDirection(Direction.Right, new Vector3(0.35f, 0.3f, 1f)); };
            _PKInput.OnSMiddleDir += () => { if (IsShoot || _isGameOver) return; SetShooterDirection(Direction.Middle, new Vector3(0f, 0.3f, 1f)); };
            _PKInput.OnKConfirm += () => { if (IsShoot || _isGameOver) return; KeeperConfirmed = true; };
            _PKInput.OnKLeftDir += () => { if (IsShoot || _isGameOver) return; SetKeeperDirection(Direction.Left, Vector3.left); };
            _PKInput.OnKRightDir += () => { if (IsShoot || _isGameOver) return; SetKeeperDirection(Direction.Right, Vector3.right); };
            _PKInput.OnKMiddleDir += () => { if (IsShoot || _isGameOver) return; SetKeeperDirection(Direction.Middle, Vector3.zero); };
            _PKInput.OnConfirm += HandleConfirm;
            _PKInput.OnEKeyDown += HandleRandom;
        }

        private void OnDisable()
        {
            _PKInput.OnConfirm -= HandleConfirm;
            _PKInput.OnEKeyDown -= HandleRandom;
            if (_uiManager != null) _uiManager.OnGameEnd -= EndGame;
        }

        private void Start()
        {
            _keeperOriginPos = _keeperPrefab.transform.position;
            _currentBall = _startBall;
            _currentBall.SetActive(true);
            InitializeGame();
        }

        private void InitializeGame()
        {
            ResetScores();
            ResetConfirmationStatus();
            _uiManager.ShowConfirmationUI();
        }

        private void HandleConfirm()
        {
            if (_isGameOver || IsShoot || !ShooterConfirmed || !KeeperConfirmed) return;
            _uiManager.HideConfirmationUI();
            IsShoot = true;
            _keeperAnimator.SetBool(IsKeepingHash, true);
            StartCoroutine(ExecuteShootSequence());
            StartCoroutine(DelayedKeeperAction(_keeperActionDelay));
            StartCoroutine(ResetRoundAfterDelay());
            ResetConfirmationStatus();
        }

        private IEnumerator ExecuteShootSequence()
        {
            _shooterAnimator.SetBool(IsRunningHash, true);
            yield return _shooterObj.transform.DOMove(_shootPoint.position, _runDuration).WaitForCompletion();
            _shooterAnimator.SetBool(IsRunningHash, false);
            _shooterAnimator.SetBool(IsShootingHash, true);
            ShootBall();
            yield return new WaitForSeconds(0.4f);
            _shooterAnimator.SetBool(IsShootingHash, false);
            yield return _shooterObj.transform.DOMove(_orignPos.position, _returnDuration).WaitForCompletion();
        }

        private void ShootBall()
        {
            if (!_currentBall.TryGetComponent(out Rigidbody rb)) return;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(ShooterDir.normalized * _shootPower, ForceMode.VelocityChange);
        }

        private IEnumerator DelayedKeeperAction(float delay)
        {
            yield return new WaitForSeconds(delay);
            ExecuteKeeperAction();
            DetermineRoundResult();
        }

        private void ExecuteKeeperAction()
        {
            _keeperRb.linearVelocity = Vector3.zero;
            Vector3 jumpDir = KeeperDir + Vector3.up * 1.2f;
            _keeperRb.AddForce(jumpDir.normalized * _keeperJumpPower, ForceMode.VelocityChange);
            RotateKeeper();
        }

        private void RotateKeeper()
        {
            float zRot = KeeperDirType switch
            {
                Direction.Left => 25f,
                Direction.Right => -25f,
                _ => 0f
            };
            _keeperPrefab.transform.rotation = Quaternion.Euler(0, 0, zRot);
        }

        private void DetermineRoundResult()
        {
            bool shooterScored = ShooterDirType != KeeperDirType;
            StartCoroutine(DelayShowResult(shooterScored));
        }

        private void SetShooterDirection(Direction type, Vector3 dir)
        {
            ShooterDirType = type;
            ShooterDir = dir;
        }

        private void SetKeeperDirection(Direction type, Vector3 dir)
        {
            KeeperDirType = type;
            KeeperDir = dir;
        }

        private void ResetConfirmationStatus()
        {
            ShooterConfirmed = false;
            KeeperConfirmed = false;
        }

        private void ResetScores()
        {
            ShooterScore = 0;
            KeeperScore = 0;
            UpdateScoreDisplay(ShooterScore, KeeperScore);
        }

        private void UpdateScoreDisplay(int shooterScore, int keeperScore)
        {
            _scoreText.text = $"{shooterScore}:{keeperScore}";
        }

        private IEnumerator ResetRoundAfterDelay()
        {
            yield return new WaitForSeconds(_resetDelay);
            if (_isGameOver) yield break;
            _keeperRb.linearVelocity = Vector3.zero;
            _keeperPrefab.transform.SetPositionAndRotation(_keeperOriginPos, Quaternion.identity);
            if (_currentBall != null && _currentBall.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                _currentBall.transform.SetPositionAndRotation(_shootPoint.position, Quaternion.identity);
            }
            _uiManager.HideResultUI();
            _uiManager.ShowConfirmationUI();
            IsShoot = false;
            _keeperAnimator.SetBool(IsKeepingHash, false);
        }

        private IEnumerator DelayShowResult(bool shooterScored)
        {
            yield return new WaitForSeconds(0.8f);
            if (shooterScored)
                _uiManager.ShowGoalUI();
            else
                _uiManager.ShowSaveUI();
            yield return new WaitForSeconds(0.5f);
            _uiManager.AddScore(shooterScored);
        }

        private void HandleRandom()
        {
            if (IsShoot || _isGameOver) return;
            ShooterDirType = (Direction)UnityEngine.Random.Range(0, 3);
            ShooterDir = ShooterDirType switch
            {
                Direction.Left => new Vector3(-0.35f, 0.3f, 1f),
                Direction.Middle => new Vector3(0f, 0.3f, 1f),
                Direction.Right => new Vector3(0.35f, 0.3f, 1f),
                _ => Vector3.forward
            };
            KeeperDirType = (Direction)UnityEngine.Random.Range(0, 3);
            KeeperDir = KeeperDirType switch
            {
                Direction.Left => Vector3.left,
                Direction.Middle => Vector3.zero,
                Direction.Right => Vector3.right,
                _ => Vector3.zero
            };
            ShooterConfirmed = true;
            KeeperConfirmed = true;
            HandleConfirm();
        }

        private void EndGame(bool p1Win)
        {
            if (_isGameOver) return;
            _isGameOver = true;
            is1Pwin = p1Win;
            is2Pwin = !p1Win;
            _uiManager.ShowWinUI(p1Win);
            StopAllCoroutines();
            _keeperAnimator.SetBool(IsKeepingHash, false);
            _shooterAnimator.SetBool(IsRunningHash, false);
            _shooterAnimator.SetBool(IsShootingHash, false);
            if (_keeperRb != null) _keeperRb.linearVelocity = Vector3.zero;
            MinigameManager.instance?.Finish(is1Pwin);
        }
    }
}
