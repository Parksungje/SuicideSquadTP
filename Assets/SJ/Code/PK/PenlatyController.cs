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

        private Rigidbody _rbCompo;
        private Vector3 _shooterDir = Vector3.zero;
        private Vector3 _keeperDir = Vector3.zero;
        private GameObject currentBall;

        [SerializeField] private GameObject _keeperPrefab;

        private Vector3 _originPos;

        private bool _sConfirm = false;
        private bool _kConfirm = false;

        [SerializeField] private TextMeshProUGUI _scoreText;

        private int _sScore = 0;
        private int _kScore = 0;

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

            Logggggg();
        }

        private void HandleSConfrim()
        {
            _sConfirm = true;
            Logggggg();
        }

        private void HandleSLeftDir()
        {
            _shooterDir = Vector3.left * 0.5f;
        }

        private void HandleSRightDir()
        {
            _shooterDir = Vector3.right * 0.5f;
        }

        private void HandleSMiddleDir()
        {
            _shooterDir = Vector3.zero;
        }

        private void HandleKConfrim()
        {
            _kConfirm = true;
            Logggggg();
        }

        private void HandleKLeftDir()
        {
            _keeperDir = new Vector3(-6, _keeperPrefab.transform.position.y, _keeperPrefab.transform.position.z);
        }

        private void HandleKRightDir()
        {
            _keeperDir = new Vector3(6, _keeperPrefab.transform.position.y, _keeperPrefab.transform.position.z);
        }

        private void HandleKMiddleDir()
        {
            _keeperDir = _originPos;
        }

        private void HandleComfirm()
        {
            if (_sConfirm == false || _kConfirm == false)
            {
                Logggggg();
                Debug.Log("누군가가 완료를 안함");
                return;
            }

            // Shooter confirm
            Debug.Log("슈터 확정 - 공 발사");

            if (currentBall != null)
                Destroy(currentBall);

            currentBall = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);

            Vector3 shooterDir = Vector3.forward;
            shooterDir += _shooterDir;
            shooterDir = shooterDir.normalized;

            _rbCompo = currentBall.GetComponent<Rigidbody>();
            if (_rbCompo != null)
            {
                _rbCompo.linearVelocity = Vector3.zero;
                _rbCompo.angularVelocity = Vector3.zero;
                _rbCompo.AddForce(shooterDir * shootForce, ForceMode.VelocityChange);
            }

            // Keeper confirm
            Vector3 keeperDir = _keeperDir;

            _keeperPrefab.transform.position = keeperDir;

            WhoIsWinner();

            StatusInit();
        }

        private void WhoIsWinner()
        {
            // todo: 내일 고칠거. 이거 confirm으로 하지 말고 방향이 일치한지 확인하려고 했던거임!!
            if (_sConfirm != _kConfirm)
                ScoreUpdate(true);
            else
                ScoreUpdate(false);
        }

        private void ScoreUpdate()
        {
            _sScore = 0;
            _kScore = 0;

            _scoreText.text = $"{_sScore} : {_kScore}";
        }

        private void ScoreUpdate(bool isShooterWin)
        {
            if (isShooterWin)
                _sScore += 1;
            else
                _kScore += 1;

            _scoreText.text = $"{_sScore} : {_kScore}";
        }

        private void Logggggg()
        {
            //Debug.Log($"<b>Status Init!</b> <size=17><color=red>Shooter confirm: {_sConfirm}</color>, <color=blue>Keeper confirm: {_kConfirm}</color></size>");
        }
    }
}
