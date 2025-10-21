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
        [SerializeField] private PenaltyKickSO PKInput;
        [SerializeField] private GameObject keeperPrefab;
        [SerializeField] private GameObject startBall;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Transform orignPos;
        [SerializeField] private GameObject shooterObj;
        [SerializeField] private Animator keeperAnimator;
        [SerializeField] private Animator shooterAnimator;

        [Header("Settings")]
        [SerializeField] private float shootPower = 20f;
        [SerializeField] private float keeperJumpPower = 10f;
        [SerializeField] private float runDuration = 0.6f;
        [SerializeField] private float returnDuration = 0.5f;
        [SerializeField] private float keeperActionDelay = 0.25f;
        [SerializeField] private float resetDelay = 3f;

        private static readonly int IsKeepingHash = Animator.StringToHash("isKeeping");
        private static readonly int IsShootingHash = Animator.StringToHash("isShooting");
        private static readonly int IsRunningHash = Animator.StringToHash("isRunning");

        private Rigidbody keeperRb;
        private Vector3 keeperOriginPos;
        private GameObject currentBall;

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

        private void Awake() => keeperRb = keeperPrefab.GetComponent<Rigidbody>();

        private void OnEnable()
        {
            PKInput.OnSConfrim += () => { if (IsShoot) return; ShooterConfirmed = true; };
            PKInput.OnSLeftDir += () => { if (IsShoot) return; SetShooterDir(Direction.Left, new Vector3(-0.35f, 0.3f, 1f)); };
            PKInput.OnSRightDir += () => { if (IsShoot) return; SetShooterDir(Direction.Right, new Vector3(0.35f, 0.3f, 1f)); };
            PKInput.OnSMiddleDir += () => { if (IsShoot) return; SetShooterDir(Direction.Middle, new Vector3(0f, 0.3f, 1f)); };

            PKInput.OnKConfrim += () => { if (IsShoot) return; KeeperConfirmed = true; };
            PKInput.OnKLeftDir += () => { if (IsShoot) return; SetKeeperDir(Direction.Left, Vector3.left); };
            PKInput.OnKRightDir += () => { if (IsShoot) return; SetKeeperDir(Direction.Right, Vector3.right); };
            PKInput.OnKMiddleDir += () => { if (IsShoot) return; SetKeeperDir(Direction.Middle, Vector3.zero); };

            PKInput.OnConfirm += HandleConfirm;
        }

        private void OnDisable() => PKInput.OnConfirm -= HandleConfirm;

        private void Start()
        {
            keeperOriginPos = keeperPrefab.transform.position;
            currentBall = startBall;
            currentBall.SetActive(true);
            ResetScores();
            ResetStatus();
        }

        private void HandleConfirm()
        {
            if (IsShoot || !ShooterConfirmed || !KeeperConfirmed) return;

            IsShoot = true;
            keeperAnimator.SetBool(IsKeepingHash, true);

            StartCoroutine(ShooterSequence());
            StartCoroutine(DelayedKeeperAction(keeperActionDelay));
            StartCoroutine(ResetKeeper());
            ResetStatus();
        }


        private IEnumerator ShooterSequence()
        {
            shooterAnimator.SetBool(IsRunningHash, true);
            yield return shooterObj.transform.DOMove(shootPoint.position, runDuration).WaitForCompletion();

            shooterAnimator.SetBool(IsRunningHash, false);
            shooterAnimator.SetBool(IsShootingHash, true);

            ShootBall();

            yield return new WaitForSeconds(0.4f);
            shooterAnimator.SetBool(IsShootingHash, false);

            yield return shooterObj.transform.DOMove(orignPos.position, returnDuration).WaitForCompletion();
        }

        private void ShootBall()
        {
            if (!currentBall.TryGetComponent(out Rigidbody rb)) return;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(ShooterDir.normalized * shootPower, ForceMode.VelocityChange);
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
            keeperRb.linearVelocity = Vector3.zero;
            Vector3 jumpDir = KeeperDir + Vector3.up * 1.2f;
            keeperRb.AddForce(jumpDir.normalized * keeperJumpPower, ForceMode.VelocityChange);
        }

        private void RotateKeeper()
        {
            float zRot = KeeperDirType switch
            {
                Direction.Left => 25f,
                Direction.Right => -25f,
                _ => 0f
            };
            keeperPrefab.transform.rotation = Quaternion.Euler(0, 0, zRot);
        }

        private void DetermineWinner()
        {
            bool shooterWin = ShooterDirType != KeeperDirType;
            UpdateScore(shooterWin);
        }

        private IEnumerator ResetKeeper()
        {
            yield return new WaitForSeconds(resetDelay);

            keeperRb.linearVelocity = Vector3.zero;
            keeperPrefab.transform.SetPositionAndRotation(keeperOriginPos, Quaternion.identity);

            if (currentBall != null && currentBall.TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                currentBall.transform.SetPositionAndRotation(shootPoint.position, Quaternion.identity);
            }

            IsShoot = false;
            keeperAnimator.SetBool(IsKeepingHash, false);
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
            UpdateScoreText();
        }

        private void UpdateScore(bool shooterWin)
        {
            if (shooterWin) ShooterScore++;
            else KeeperScore++;
            UpdateScoreText();
        }

        private void UpdateScoreText() => scoreText.text = $"{ShooterScore} : {KeeperScore}";
    }
}