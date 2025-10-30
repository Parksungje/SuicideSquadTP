using System.Collections;
using Tild.Menu;
using UnityEngine;

namespace Tild.Minigames.SpinGame
{
    public class SpinGameManager : MonoBehaviour
    {
        public static SpinGameManager instance;
        
        [Header("Input")]
        [SerializeField] private FallingInputSO fallingInputSO;

        [Header("Players")]
        [SerializeField] private Rigidbody rigid1P, rigid2P;
        [SerializeField] private Animator animator1P, animator2P;
        [SerializeField] private Transform jumpObstacle, HeadObstacle;
        [SerializeField] private Camera mainCamera;

        [Header("Speed Settings")]
        [SerializeField] private float baseSpeed = 30f;  
        [SerializeField] private float acceleration = 1f; 
        [SerializeField] private float maxSpeed = 360f; 

        [Header("Jump Settings")]
        [SerializeField] private float jumpPower = 6f;
        [SerializeField] private float movePower = 8f;
        [SerializeField] private float jumpCooldown = 1f;

        private float currentSpeed = 0f;
        private bool canJump1P = true;
        private bool canJump2P = true;

        private bool wPressed, aPressed, sPressed, dPressed;
        private bool upPressed, downPressed, leftPressed, rightPressed;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }

        private void Start()
        {
            currentSpeed = baseSpeed;
        }

        private void OnEnable()
        {
            fallingInputSO.OnWKeyDown += (pressed) => wPressed = pressed;
            fallingInputSO.OnAKeyDown += (pressed) => aPressed = pressed;
            fallingInputSO.OnSKeyDown += (pressed) => sPressed = pressed;
            fallingInputSO.OnDKeyDown += (pressed) => dPressed = pressed;

            fallingInputSO.OnUpArrowDown += (pressed) => upPressed = pressed;
            fallingInputSO.OnDownArrowDown += (pressed) => downPressed = pressed;
            fallingInputSO.OnLeftArrowDown += (pressed) => leftPressed = pressed;
            fallingInputSO.OnRightArrowDown += (pressed) => rightPressed = pressed;
        }

        private void OnDisable()
        {
            fallingInputSO.OnWKeyDown -= (pressed) => wPressed = pressed;
            fallingInputSO.OnAKeyDown -= (pressed) => aPressed = pressed;
            fallingInputSO.OnSKeyDown -= (pressed) => sPressed = pressed;
            fallingInputSO.OnDKeyDown -= (pressed) => dPressed = pressed;

            fallingInputSO.OnUpArrowDown -= (pressed) => upPressed = pressed;
            fallingInputSO.OnDownArrowDown -= (pressed) => downPressed = pressed;
            fallingInputSO.OnLeftArrowDown -= (pressed) => leftPressed = pressed;
            fallingInputSO.OnRightArrowDown -= (pressed) => rightPressed = pressed;
        }

        private void Update()
        {
            if (jumpObstacle == null || HeadObstacle == null) return;

            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            jumpObstacle.Rotate(Vector3.up, currentSpeed * Time.deltaTime, Space.World);
            HeadObstacle.Rotate(Vector3.up, -currentSpeed * Time.deltaTime, Space.World);

            Vector3 dir1P = GetInputDirection(wPressed, sPressed, aPressed, dPressed);
            if (dir1P != Vector3.zero && canJump1P)
                TryJump(rigid1P, animator1P, ref canJump1P, dir1P);

            Vector3 dir2P = GetInputDirection(upPressed, downPressed, leftPressed, rightPressed);
            if (dir2P != Vector3.zero && canJump2P)
                TryJump(rigid2P, animator2P, ref canJump2P, dir2P);
        }

        private Vector3 GetInputDirection(bool forward, bool back, bool left, bool right)
        {
            Vector3 dir = Vector3.zero;
            if (forward) dir += Vector3.forward;
            if (back) dir += Vector3.back;
            if (left) dir += Vector3.left;
            if (right) dir += Vector3.right;
            return dir.normalized;
        }

        private void TryJump(Rigidbody rigid, Animator animator, ref bool canJump, Vector3 dir)
        {
            if (!canJump) return;
            canJump = false;

            rigid.linearVelocity = Vector3.zero;

            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;
            camForward.y = 0f; 
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (dir.z * camForward + dir.x * camRight).normalized;

            if (moveDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                rigid.transform.rotation = targetRotation;
            }

            animator.SetTrigger("JUMP");
            
            Vector3 jumpVector = (Vector3.up * jumpPower) + (moveDir * movePower);
            rigid.AddForce(jumpVector, ForceMode.Impulse);
            
            StartCoroutine(ExtraGravity(rigid));
        }

        private IEnumerator ExtraGravity(Rigidbody rigid)
        {
            float duration = 0.3f; 
            float timer = 0f;
            while (timer < duration)
            {
                rigid.AddForce(Vector3.down * 50f, ForceMode.Acceleration);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        { 
            Debug.Log(collision.rigidbody + " " + collision.gameObject.name);
            
            if (collision.rigidbody == rigid1P)
                canJump1P = true;

            if (collision.rigidbody == rigid2P)
                canJump2P = true;
        }

        public void Falled(Rigidbody rigid)
        {
            if (rigid == rigid1P)
            {
                MinigameManager.instance.Finish(false);
            }
            if (rigid == rigid2P)
            {
                MinigameManager.instance.Finish(true);
            }
        }
    }
}
