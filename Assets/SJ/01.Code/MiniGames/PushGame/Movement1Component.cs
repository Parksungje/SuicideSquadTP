using UnityEngine;

public class Movement1Component : MonoBehaviour
{
    [SerializeField] private PushGameSO pushInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform player2Target;


    private Rigidbody _rigid;
    private Vector3 _moveDir;

    private bool wPressed, aPressed, sPressed, dPressed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }
        
    private void OnEnable()
    {
        if (pushInput == null)
        {
            Debug.LogError("❌ PushGameSO가 연결되지 않았습니다!");
            return;
        }

        pushInput.OnWKeyDown += OnWKey;
        pushInput.OnSKeyDown += OnSKey;
        pushInput.OnAKeyDown += OnAKey;
        pushInput.OnDKeyDown += OnDKey;
    }

    private void OnDisable()
    {
        if (pushInput == null) return;

        pushInput.OnWKeyDown -= OnWKey;
        pushInput.OnSKeyDown -= OnSKey;
        pushInput.OnAKeyDown -= OnAKey;
        pushInput.OnDKeyDown -= OnDKey;
    }

    private void OnWKey(bool pressed) => wPressed = pressed;
    private void OnSKey(bool pressed) => sPressed = pressed;
    private void OnAKey(bool pressed) => aPressed = pressed;
    private void OnDKey(bool pressed) => dPressed = pressed;

    private void FixedUpdate()
    {
        _moveDir = Vector3.zero;
        if (wPressed) _moveDir += Vector3.forward;
        if (sPressed) _moveDir += Vector3.back;
        if (aPressed) _moveDir += Vector3.left;
        if (dPressed) _moveDir += Vector3.right;

        _moveDir = _moveDir.normalized;
        _rigid.linearVelocity = new Vector3(_moveDir.x * moveSpeed, _rigid.linearVelocity.y, _moveDir.z * moveSpeed);


        if (player2Target != null)
        {
            Vector3 lookDir = player2Target.position - transform.position;
            lookDir.y = 0f;
            if (lookDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(lookDir);
        }
    }
}
