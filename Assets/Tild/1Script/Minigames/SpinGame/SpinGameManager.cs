using System.Collections;
using Tild.Minigames.BalanceGame;
using UnityEngine;

namespace Tild.Minigames.SpinGame
{
    public class SpinGameManager : MonoBehaviour
    {
        [SerializeField] private SpinInputSO spinInputSO;

        [SerializeField] private Rigidbody rigid1P,  rigid2P;
        [SerializeField] private Animator animator1P, animator2P;
        [SerializeField] private Transform jumpObstacle, HeadObstacle;
        [SerializeField] private float baseSpeed = 30f;  
        [SerializeField] private float acceleration = 1f; 
        [SerializeField] private float maxSpeed = 360f; 

        private float currentSpeed = 0f;
        
        [SerializeField] private float jumpPower = 6f;
        [SerializeField] private float movePower = 8f;
        [SerializeField] private float jumpCooldown = 1f;

        [SerializeField] private Camera mainCamera;

        private bool canJump1P = true;
        private bool canJump2P = true;

        private void Start()
        {
            currentSpeed = baseSpeed;
        }

        private void Update()
        {
            if (jumpObstacle == null || HeadObstacle == null) return;
            
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        
            jumpObstacle.Rotate(Vector3.up, currentSpeed * Time.deltaTime, Space.World);
            HeadObstacle.Rotate(Vector3.up, -currentSpeed * Time.deltaTime, Space.World);
        }

        
        private void OnEnable()
        {
            // --- 1P ---
            spinInputSO.AKeyPressed += () => TryJump(rigid1P, animator1P, ref canJump1P, Vector3.left);
            spinInputSO.DKeyPressed += () => TryJump(rigid1P, animator1P,ref canJump1P, Vector3.right);
            spinInputSO.SKeyPressed += () => TryJump(rigid1P, animator1P, ref canJump1P, Vector3.back);
            spinInputSO.WKeyPressed += () => TryJump(rigid1P, animator1P, ref canJump1P, Vector3.forward);

            // --- 2P ---
            spinInputSO.LeftKeyPressed += () => TryJump(rigid2P, animator2P,ref canJump2P, Vector3.left);
            spinInputSO.RightKeyPressed += () => TryJump(rigid2P,animator2P, ref canJump2P, Vector3.right);
            spinInputSO.DownKeyPressed += () => TryJump(rigid2P, animator2P,ref canJump2P, Vector3.back);
            spinInputSO.UpKeyPressed += () => TryJump(rigid2P,animator2P, ref canJump2P, Vector3.forward);
        }

        private void OnDisable()
        {
            spinInputSO.AKeyPressed -= () => TryJump(rigid1P, animator1P, ref canJump1P, Vector3.left);
            spinInputSO.DKeyPressed -= () => TryJump(rigid1P, animator1P, ref canJump1P, Vector3.right);
            spinInputSO.SKeyPressed -= () => TryJump(rigid1P, animator1P, ref canJump1P, Vector3.back);
            spinInputSO.WKeyPressed -= () => TryJump(rigid1P, animator1P, ref canJump1P, Vector3.forward);

            spinInputSO.LeftKeyPressed -= () => TryJump(rigid2P, animator2P,ref canJump2P, Vector3.left);
            spinInputSO.RightKeyPressed -= () => TryJump(rigid2P,animator2P, ref canJump2P, Vector3.right);
            spinInputSO.DownKeyPressed -= () => TryJump(rigid2P, animator2P,ref canJump2P, Vector3.back);
            spinInputSO.UpKeyPressed -= () => TryJump(rigid2P,animator2P, ref canJump2P, Vector3.forward);
        }

        private void TryJump(Rigidbody rigid, Animator animator, ref bool canJump, Vector3 dir)
        {
            if (!canJump) return;
            canJump = false;

            rigid.linearVelocity = Vector3.zero;

            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;
            camForward.y = 0f; camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = (dir.z * camForward + dir.x * camRight).normalized;

            if (moveDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
                float rotationSpeed = 10f; 
                rigid.transform.rotation = Quaternion.Slerp(
                    rigid.transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            animator.SetTrigger("JUMP");
            
            Vector3 jumpVector = (Vector3.up * jumpPower) + (moveDir * movePower);
            rigid.AddForce(jumpVector, ForceMode.Impulse);
            
            StartCoroutine(ExtraGravity(rigid));
            StartCoroutine(JumpCooldown(rigid == rigid1P));
        }


        private IEnumerator ExtraGravity(Rigidbody rigid)
        {
            float duration = 0.3f; 
            float timer = 0f;
            while(timer < duration)
            {
                rigid.AddForce(Vector3.down * 20f, ForceMode.Acceleration);
                timer += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator JumpCooldown(bool is1P)
        {
            yield return new WaitForSeconds(jumpCooldown);
            if (is1P) canJump1P = true;
            else canJump2P = true;
        }
    }
}
