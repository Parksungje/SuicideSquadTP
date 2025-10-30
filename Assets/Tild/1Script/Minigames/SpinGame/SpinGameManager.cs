using System.Collections;
using Tild.Menu;
using UnityEngine;

namespace Tild.Minigames.SpinGame
{
    public class SpinGameManager : MonoBehaviour
    {
        public static SpinGameManager instance;
        
        [SerializeField] private Rigidbody rigid1P, rigid2P;
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
        void Awake()
        {
           
            if (instance == null) 
                instance = this; 
            
            else if (instance != this) 
                Destroy(gameObject);
        }

        private void Update()
        {
            if (jumpObstacle == null || HeadObstacle == null) return;

        
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            jumpObstacle.Rotate(Vector3.up, currentSpeed * Time.deltaTime, Space.World);
            HeadObstacle.Rotate(Vector3.up, -currentSpeed * Time.deltaTime, Space.World);

         
            Vector3 dir1P = GetInputDirection(
                KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D
            );
            if (dir1P != Vector3.zero && canJump1P)
                TryJump(rigid1P, animator1P, ref canJump1P, dir1P);

         
            Vector3 dir2P = GetInputDirection(
                KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow
            );
            if (dir2P != Vector3.zero && canJump2P)
                TryJump(rigid2P, animator2P, ref canJump2P, dir2P);
        }

        private Vector3 GetInputDirection(KeyCode forward, KeyCode back, KeyCode left, KeyCode right)
        {
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(forward)) dir += Vector3.forward;
            if (Input.GetKey(back)) dir += Vector3.back;
            if (Input.GetKey(left)) dir += Vector3.left;
            if (Input.GetKey(right)) dir += Vector3.right;
            return dir.normalized; 
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
            while(timer < duration)
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
