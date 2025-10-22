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
            _PKInput.OnSLeftDir += () => { if (IsShoot) return; SetShooterDir(Direction.Left, new Vector3(-0.35f, 0.3f, 1f)); };
            _PKInput.OnSRightDir += () => { if (IsShoot) return; SetShooterDir(Direction.Right, new Vector3(0.35f, 0.3f, 1f)); };
            _PKInput.OnSMiddleDir += () => { if (IsShoot) return; SetShooterDir(Direction.Middle, new Vector3(0f, 0.3f, 1f)); };

            _PKInput.OnKConfirm += () => { if (IsShoot) return; KeeperConfirmed = true; };
            _PKInput.OnKLeftDir += () => { if (IsShoot) return; SetKeeperDir(Direction.Left, Vector3.left); };
            _PKInput.OnKRightDir += () => { if (IsShoot) return; SetKeeperDir(Direction.Right, Vector3.right); };
            _PKInput.OnKMiddleDir += () => { if (IsShoot) return; SetKeeperDir(Direction.Middle, Vector3.zero); };

            _PKInput.OnConfirm += HandleConfirm;
        }

        private void OnDisable() => _PKInput.OnConfirm -= HandleConfirm;

        private void Start()
        {
            _keeperOriginPos = _keeperPrefab.transform.position;
            _currentBall = _startBall;
            _currentBall.SetActive(true);
            ResetScores();
            ResetStatus();
        }

        private void HandleConfirm()
        {
            if (IsShoot || !ShooterConfirmed || !KeeperConfirmed) return;

            IsShoot = true;
            _keeperAnimator.SetBool(IsKeepingHash, true);

            StartCoroutine(ShooterSequence());
            StartCoroutine(DelayedKeeperAction(_keeperActionDelay));
            StartCoroutine(ResetKeeper());
            ResetStatus();
        }


        private IEnumerator ShooterSequence()
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
            KeeperJump();
            RotateKeeper();
            DetermineWinner();
        }

        private void KeeperJump()
        {
            _keeperRb.linearVelocity = Vector3.zero;
            Vector3 jumpDir = KeeperDir + Vector3.up * 1.2f;
            _keeperRb.AddForce(jumpDir.normalized * _keeperJumpPower, ForceMode.VelocityChange);
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

        private void DetermineWinner()
        {
            bool shooterWin = ShooterDirType != KeeperDirType;
            UpdateScore(shooterWin);
        }

        private IEnumerator ResetKeeper()
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

            IsShoot = false;
            _keeperAnimator.SetBool(IsKeepingHash, false);
        }


        private void SetShooterDir(Direction type, Vector3 dir)
        {
            ShooterDirType = type;
            ShooterDir = dir;
        }

        private void SetKeeperDir(Direction type, Vector3 dir)
        {
            KeeperDirType = type;
            KeeperDir = dir;
        }

        private void ResetStatus()
        {
            ShooterConfirmed = false;
            KeeperConfirmed = false;
        }

        private void ResetScores()
        {
            ShooterScore = 0;
            KeeperScore = 0;
            UpdateScoreText(ShooterScore, KeeperScore);
        }

        private void UpdateScore(bool shooterWin)
        {
            if (shooterWin) ShooterScore++;
            else KeeperScore++;
            UpdateScoreText(ShooterScore, KeeperScore);
        }

        private void UpdateScoreText(int shooterScore, int keeperScore)
        {
            _shooterText.text = shooterScore.ToString();
            _keeperText.text = keeperScore.ToString();
        }
    }
}