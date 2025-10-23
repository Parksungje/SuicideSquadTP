using Code.Player;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Code.PK
{
    public class PenaltyKickController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PenaltyKickSO _PKInput;
        [SerializeField] private GameObject _keeperPrefab;
        [SerializeField] private GameObject _startBall;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private TextMeshProUGUI _shooterText;
        [SerializeField] private TextMeshProUGUI _keeperText;
        [SerializeField] private Transform _orignPos;
        [SerializeField] private GameObject _shooterObj;
        [SerializeField] private Animator _keeperAnimator;
        [SerializeField] private Animator _shooterAnimator;
        [SerializeField] private PKUIManager _uiManager;


        [Header("Settings")]
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

        private int ShooterScore { get; set; }
        private int KeeperScore { get; set; }

        private enum Direction { Left, Middle, Right }
        private Direction ShooterDirType { get; set; } = Direction.Middle;
        private Direction KeeperDirType { get; set; } = Direction.Middle;

        private Vector3 ShooterDir { get; set; } = Vector3.forward;
        private Vector3 KeeperDir { get; set; } = Vector3.zero;

        private void Awake() => _keeperRb = _keeperPrefab.GetComponent<Rigidbody>();

        private void OnEnable()
        {
            _PKInput.OnSConfirm += () => { if (IsShoot) return; ShooterConfirmed = true; };
            _PKInput.OnSLeftDir += () => { if (IsShoot) return; SetShooterDirection(Direction.Left, new Vector3(-0.35f, 0.3f, 1f)); };
            _PKInput.OnSRightDir += () => { if (IsShoot) return; SetShooterDirection(Direction.Right, new Vector3(0.35f, 0.3f, 1f)); };
            _PKInput.OnSMiddleDir += () => { if (IsShoot) return; SetShooterDirection(Direction.Middle, new Vector3(0f, 0.3f, 1f)); };

            _PKInput.OnKConfirm += () => { if (IsShoot) return; KeeperConfirmed = true; };
            _PKInput.OnKLeftDir += () => { if (IsShoot) return; SetKeeperDirection(Direction.Left, Vector3.left); };
            _PKInput.OnKRightDir += () => { if (IsShoot) return; SetKeeperDirection(Direction.Right, Vector3.right); };
            _PKInput.OnKMiddleDir += () => { if (IsShoot) return; SetKeeperDirection(Direction.Middle, Vector3.zero); };

            _PKInput.OnConfirm += HandleConfirm;
        }

        private void OnDisable() => _PKInput.OnConfirm -= HandleConfirm;

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
        }

        private void HandleConfirm()
        {
            if (IsShoot || !ShooterConfirmed || !KeeperConfirmed) return;

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

            if (shooterScored)
                _uiManager.ShowGoalUI();
            else
                _uiManager.ShowSaveUI();

            StartCoroutine(DelayAddScore(shooterScored));
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
            _shooterText.text = shooterScore.ToString();
            _keeperText.text = keeperScore.ToString();
        }

        private IEnumerator DelayAddScore(bool shooterWin)
        {
            yield return new WaitForSeconds(1f);
            _uiManager.AddScore(shooterWin);
        }

        private IEnumerator ResetRoundAfterDelay()
        {
            yield return new WaitForSeconds(_resetDelay);

            _keeperRb.linearVelocity = Vector3.zero;
            _keeperPrefab.transform.SetPositionAndRotation(_keeperOriginPos, Quaternion.identity);

            if (_currentBall != null && _currentBall.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                _currentBall.transform.SetPositionAndRotation(_shootPoint.position, Quaternion.identity);
            }

            _uiManager.HideResultUI();

            IsShoot = false;
            _keeperAnimator.SetBool(IsKeepingHash, false);
        }
    }
}