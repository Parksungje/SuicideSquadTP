using Code.Player;
using Tild._1Script.Minigames.Rope;
using UnityEngine;

public class HuddleComponent : MonoBehaviour
{
    [field: SerializeField] private HuddleGameSO _inputSO;

    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");

    private bool wPressed, upPressed;
    private Rigidbody rb;

    private void Awake()
    {
        rb = FindAnyObjectByType<Rigidbody>();
    }

    private void OnEnable()
    {
        if (_inputSO == null)
        {
            return;
        }

        _inputSO.OnWKeyDown += OnWKey;

        _inputSO.OnUpArrowDown += OnUpKey;
    }

    private void OnDisable()
    {
        if (_inputSO == null) return;

        _inputSO.OnWKeyDown -= OnWKey;

        _inputSO.OnUpArrowDown -= OnUpKey;
    }

    private void OnWKey(bool pressed) => wPressed = pressed;
    private void OnUpKey(bool pressed) => upPressed = pressed;

    private void FixedUpdate()
    {
        rb.AddForce(Vector3.forward * 1f, ForceMode.Impulse);
    }
}
