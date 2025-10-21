using Code.Player;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Code.PK
{
    public class PenaltyKickController : MonoBehaviour
    {
        [SerializeField] private PenaltyKickSO PKInput;
        [SerializeField] private GameObject keeperPrefab;
        [SerializeField] private GameObject startBall;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private float shootPower = 20f;
        [SerializeField] private float keeperJumpPower = 10f;
        [SerializeField] private Animator keeperAnimator;
        [SerializeField] private Animator shooterAnimator;
        [SerializeField] private Transform orignPos;
        [SerializeField] private GameObject shooterObj;

        private readonly int IsKeepingHash = Animator.StringToHash("isKeeping");
        private readonly int IsShootingHash = Animator.StringToHash("isShooting");
        private readonly int IsRunningHash = Animator.StringToHash("isRunning");

        private Rigidbody keeperRb;
        private GameObject currentBall;
        private Vector3 keeperOriginPos;

        private bool shooterConfirmed;
        private bool keeperConfirmed;
        private bool isShoot;

        private int shooterScore;
        private int keeperScore;

        private enum Direction { Left, Middle, Right }
        private Direction shooterDirType = Direction.Middle;
        private Direction keeperDirType = Direction.Middle;

        private Vector3 shooterDir = Vector3.forward;
        private Vector3 keeperDir = Vector3.zero;

        private void Awake()
        {
            keeperRb = keeperPrefab.GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            PKInput.OnSConfrim += () => shooterConfirmed = true;
            PKInput.OnSLeftDir += () => SetShooterDir(Direction.Left, new Vector3(-0.35f, 0.3f, 1f));
            PKInput.OnSRightDir += () => SetShooterDir(Direction.Right, new Vector3(0.35f, 0.3f, 1f));
            PKInput.OnSMiddleDir += () => SetShooterDir(Direction.Middle, new Vector3(0f, 0.3f, 1f));

            PKInput.OnKConfrim += () => keeperConfirmed = true;
            PKInput.OnKLeftDir += () => SetKeeperDir(Direction.Left, Vector3.left);
            PKInput.OnKRightDir += () => SetKeeperDir(Direction.Right, Vector3.right);
            PKInput.OnKMiddleDir += () => SetKeeperDir(Direction.Middle, Vector3.zero);

            PKInput.OnConfirm += HandleConfirm;
        }

        private void OnDisable()
        {
            PKInput.OnConfirm -= HandleConfirm;
        }

        private void Start()
        {
            keeperOriginPos = keeperPrefab.transform.position;
            startBall.SetActive(true);
            currentBall = startBall;
            ResetScores();
            ResetStatus();
        }

        private void HandleConfirm()
        {
            if (!shooterConfirmed || !keeperConfirmed)
                return;

            isShoot = true;
            keeperAnimator.SetBool(IsKeepingHash, isShoot);

            StartCoroutine(ShooterRunAndShoot());
            StartCoroutine(DelayedKeeperAction(0.25f));
            StartCoroutine(ResetKeeper());
            ResetStatus();
        }

        private IEnumerator ShooterRunAndShoot()
        {
            shooterAnimator.SetBool(IsRunningHash, true);

            yield return MoveTo(shooterObj, shootPoint.position, 0.6f);

            shooterAnimator.SetBool(IsRunningHash, false);
            shooterAnimator.SetBool(IsShootingHash, true);

            SpawnAndShootBall();

            yield return new WaitForSeconds(0.4f);
            shooterAnimator.SetBool(IsShootingHash, false);

            yield return MoveTo(shooterObj, orignPos.position, 0.5f);
        }

        private void SpawnAndShootBall()
        {
            currentBall = startBall;

            var rb = currentBall.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce(shooterDir.normalized * shootPower, ForceMode.VelocityChange);
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
            Vector3 jumpDir = keeperDir + Vector3.up * 1.2f;
            keeperRb.AddForce(jumpDir.normalized * keeperJumpPower, ForceMode.VelocityChange);
        }

        private void RotateKeeper()
        {
            float zRot = keeperDirType switch
            {
                Direction.Left => 25f,
                Direction.Right => -25f,
                _ => 0f
            };

            keeperPrefab.transform.rotation = Quaternion.Euler(0, 0, zRot);
        }

        private void DetermineWinner()
        {
            bool shooterWin = shooterDirType != keeperDirType;
            UpdateScore(shooterWin);
        }

        private IEnumerator ResetKeeper()
        {
            yield return new WaitForSeconds(2.5f);

            keeperRb.linearVelocity = Vector3.zero;
            keeperPrefab.transform.position = keeperOriginPos;
            keeperPrefab.transform.rotation = Quaternion.identity;

            if (currentBall != null)
            {
                Rigidbody rb = currentBall.GetComponent<Rigidbody>();
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // ✅ Lerp 대신 그냥 바로 리셋
                currentBall.transform.position = shootPoint.position;
                currentBall.transform.rotation = Quaternion.identity;
            }

            isShoot = false;
            keeperAnimator.SetBool(IsKeepingHash, isShoot);
        }

        private IEnumerator MoveTo(GameObject obj, Vector3 target, float duration)
        {
            Vector3 start = obj.transform.position;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                obj.transform.position = Vector3.Lerp(start, target, t);
                yield return null;
            }

            obj.transform.position = target;
        }

        private void SetShooterDir(Direction type, Vector3 dir)
        {
            shooterDirType = type;
            shooterDir = dir;
        }

        private void SetKeeperDir(Direction type, Vector3 dir)
        {
            keeperDirType = type;
            keeperDir = dir;
        }

        private void ResetStatus()
        {
            shooterConfirmed = false;
            keeperConfirmed = false;
        }

        private void ResetScores()
        {
            shooterScore = 0;
            keeperScore = 0;
            UpdateScoreText();
        }

        private void UpdateScore(bool shooterWin)
        {
            if (shooterWin) shooterScore++;
            else keeperScore++;
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            scoreText.text = $"{shooterScore} : {keeperScore}";
        }
    }
}
