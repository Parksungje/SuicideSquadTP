using System;
using TMPro;
using UnityEngine;

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


    private Vector3 _p1HairDir;
    private Vector3 _p2HairDir;

    private bool _wPressed, _sPressed, _aPressed, _dPressed,
        _upArrowPressed, _downArrowPressed, _leftArrowPressed, _rightArrowPressed;

    private float _p1LastShootTime;
    private float _p2LastShootTime;

    private int _p1Score;
    private int _p2Score;

    private void Awake()
    {
        _p1HairDir = Vector3.zero;
        _p2HairDir = Vector3.zero;

        _p1LastShootTime = -_shootCooldown;
        _p2LastShootTime = -_shootCooldown;

        _p1Score = 0;
        _p2Score = 0;
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

    private void SetP1W(bool isPressed) => _wPressed = isPressed;
    private void SetP1S(bool isPressed) => _sPressed = isPressed;
    private void SetP1A(bool isPressed) => _aPressed = isPressed;
    private void SetP1D(bool isPressed) => _dPressed = isPressed;
    private void SetP2UpArrow(bool isPressed) => _upArrowPressed = isPressed;
    private void SetP2DownArrow(bool isPressed) => _downArrowPressed = isPressed;
    private void SetP2LeftArrow(bool isPressed) => _leftArrowPressed = isPressed;
    private void SetP2RightArrow(bool isPressed) => _rightArrowPressed = isPressed;

    private void SetP1Shoot(bool isPressed)
    {
        if (isPressed && Time.time - _p1LastShootTime >= _shootCooldown)
        {
            _p1LastShootTime = Time.time;
            AttemptShoot(_p1CrossHairComponent, _p1Animator, 1);
        }
    }

    private void SetP2Shoot(bool isPressed)
    {
        if (isPressed && Time.time - _p2LastShootTime >= _shootCooldown)
        {
            _p2LastShootTime = Time.time;
            AttemptShoot(_p2CrossHairComponent, _p2Animator, 2);
        }
    }

    private void FixedUpdate()
    {
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
        if (crossHair == null) return;

        if (animator != null)
        {
            animator.SetBool("isFire", true);
            Invoke(nameof(ResetFireFlags), 0.1f);
        }

        TargetComponent target = crossHair.GetCurrentTarget();
        if (target != null)
        {
            target.OnHit();

            if (playerNumber == 1)
            {
                _p1Score += 20;
                _p1ScoreText.text = _p1Score.ToString();
                ShowScorePopup(crossHair.transform.position, "+20", Color.red);
            }
            else
            {
                _p2Score += 20;
                _p2ScoreText.text = _p2Score.ToString();
                ShowScorePopup(crossHair.transform.position, "+20", Color.blue);
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
        if (popupComp != null)
            popupComp.Show(message, color);
    }

}
