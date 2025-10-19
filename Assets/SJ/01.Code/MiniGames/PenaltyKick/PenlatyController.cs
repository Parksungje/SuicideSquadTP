using Code.Player;
using TMPro;
using UnityEngine;

namespace Code.PK
{
    public class PenaltyKickController : MonoBehaviour
    {
        [SerializeField] private PenaltyKickSO PKInput;
        [SerializeField] private GameObject ballPrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float shootForce = 100f;
        [SerializeField] private GameObject _keeperPrefab;
        [SerializeField] private TextMeshProUGUI _scoreText;

        private Rigidbody _rbCompo;
        private GameObject currentBall;
        private Vector3 _originPos;

        private bool _sConfirm = false;
        private bool _kConfirm = false;

        private int _sScore = 0;
        private int _kScore = 0;

        private enum Direction
        {
            Left,
            Middle,
            Right
        }

        private Direction _shooterDirType = Direction.Middle;
        private Direction _keeperDirType = Direction.Middle;

        private Vector3 _shooterDir = Vector3.forward;
        private Vector3 _keeperDir = Vector3.zero;

        private void OnEnable()
        {
            PKInput.OnSConfrim += HandleSConfrim;
            PKInput.OnSLeftDir += HandleSLeftDir;
            PKInput.OnSRightDir += HandleSRightDir;
            PKInput.OnSMiddleDir += HandleSMiddleDir;

            PKInput.OnKConfrim += HandleKConfrim;
            PKInput.OnKLeftDir += HandleKLeftDir;
            PKInput.OnKRightDir += HandleKRightDir;
            PKInput.OnKMiddleDir += HandleKMiddleDir;

            PKInput.OnConfirm += HandleComfirm;
        }

        private void OnDisable()
        {
            PKInput.OnSConfrim -= HandleSConfrim;
            PKInput.OnSLeftDir -= HandleSLeftDir;
            PKInput.OnSRightDir -= HandleSRightDir;
            PKInput.OnSMiddleDir -= HandleSMiddleDir;

            PKInput.OnKConfrim -= HandleKConfrim;
            PKInput.OnKLeftDir -= HandleKLeftDir;
            PKInput.OnKRightDir -= HandleKRightDir;
            PKInput.OnKMiddleDir -= HandleKMiddleDir;

            PKInput.OnConfirm -= HandleComfirm;
        }

        private void Start()
        {
            _originPos = _keeperPrefab.transform.position;
            ScoreUpdate();
            StatusInit();
        }

        private void StatusInit()
        {
            _sConfirm = false;
            _kConfirm = false;
        }

        private void ScoreUpdate()
        {
            _sScore = 0;
            _kScore = 0;
            _scoreText.text = $"{_sScore} : {_kScore}";
        }

        private void ScoreUpdate(bool isShooterWin)
        {
            if (isShooterWin) _sScore++;
            else _kScore++;
            _scoreText.text = $"{_sScore} : {_kScore}";
        }

        private void HandleSConfrim() => _sConfirm = true;

        private void HandleSLeftDir()
        {
            _shooterDirType = Direction.Left;
            _shooterDir = new Vector3(-0.5f, 0f, 1f);
        }

        private void HandleSRightDir()
        {
            _shooterDirType = Direction.Right;
            _shooterDir = new Vector3(0.5f, 0f, 1f);
        }

        private void HandleSMiddleDir()
        {
            _shooterDirType = Direction.Middle;
            _shooterDir = Vector3.forward;
        }

        private void HandleKConfrim() => _kConfirm = true;

        private void HandleKLeftDir()
        {
            _keeperDirType = Direction.Left;
            _keeperDir = _originPos + Vector3.left * 6f;
        }

        private void HandleKRightDir()
        {
            _keeperDirType = Direction.Right;
            _keeperDir = _originPos + Vector3.right * 6f;
        }

        private void HandleKMiddleDir()
        {
            _keeperDirType = Direction.Middle;
            _keeperDir = _originPos;
        }

        private void HandleComfirm()
        {
            if (!_sConfirm || !_kConfirm)
                return;

            if (currentBall != null)
                Destroy(currentBall);

            currentBall = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
            _rbCompo = currentBall.GetComponent<Rigidbody>();

            var dir = _shooterDir.normalized;
            _rbCompo.linearVelocity = Vector3.zero;
            _rbCompo.angularVelocity = Vector3.zero;
            _rbCompo.AddForce(dir * shootForce, ForceMode.VelocityChange);

            _keeperPrefab.transform.position = _keeperDir;

            WhoIsWinner();
            StatusInit();
        }

        private void WhoIsWinner()
        {
            bool shooterWin = _shooterDirType != _keeperDirType;
            ScoreUpdate(shooterWin);
        }
    }
}
