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
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private GameObject keeperPrefab;
        [SerializeField] private GameObject startBall;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private float shootPower = 20f;
        [SerializeField] private float keeperJumpPower = 10f;
        [SerializeField] private Animator keeperAnimator;

        private readonly int IsKeepingHash = Animator.StringToHash("isKeeping");

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
            PKInput.OnSConfrim -= () => shooterConfirmed = true;
            PKInput.OnSLeftDir -= () => SetShooterDir(Direction.Left, new Vector3(-0.5f, 0.3f, 1f));
            PKInput.OnSRightDir -= () => SetShooterDir(Direction.Right, new Vector3(0.5f, 0.3f, 1f));
            PKInput.OnSMiddleDir -= () => SetShooterDir(Direction.Middle, new Vector3(0f, 0.3f, 1f));

            PKInput.OnKConfrim -= () => keeperConfirmed = true;
            PKInput.OnKLeftDir -= () => SetKeeperDir(Direction.Left, Vector3.left);
            PKInput.OnKRightDir -= () => SetKeeperDir(Direction.Right, Vector3.right);
            PKInput.OnKMiddleDir -= () => SetKeeperDir(Direction.Middle, Vector3.zero);

            PKInput.OnConfirm -= HandleConfirm;
        }

        private void Start()
        {
            keeperOriginPos = keeperPrefab.transform.position;
            startBall.SetActive(true);
            ResetScores();
            ResetStatus();
        }

        private void HandleConfirm()
        {
            if (!shooterConfirmed || !keeperConfirmed)
                return;

            isShoot = true;
            keeperAnimator.SetBool(IsKeepingHash, isShoot);
            startBall.SetActive(false);

            SpawnAndShootBall();
            KeeperJump();
            RotateKeeper();
            DetermineWinner();

            StartCoroutine(ResetKeeper());
            ResetStatus();
        }

        private void SpawnAndShootBall()
        {
            if (currentBall != null)
                Destroy(currentBall);

            currentBall = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
            var rb = currentBall.GetComponent<Rigidbody>();

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(shooterDir.normalized * shootPower, ForceMode.VelocityChange);
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

            keeperPrefab.transform.DORotate(new Vector3(0, 0, zRot), 0.3f, RotateMode.Fast);
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
                currentBall.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                currentBall.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                currentBall.transform.DOMove(shootPoint.position, 0.5f).OnComplete(() =>
                {
                    startBall.SetActive(true);
                    Destroy(currentBall);
                    currentBall = null;
                });
            }
            else
            {
                startBall.SetActive(true);
            }

            isShoot = false;
            keeperAnimator.SetBool(IsKeepingHash, isShoot);
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
