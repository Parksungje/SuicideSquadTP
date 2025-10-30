using System.Collections;
using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class HurdlePlayerController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private HurdleGameManager gameManager;
        //[SerializeField] private Animator animator;
        [SerializeField] private Transform gfxRoot;

        [Header("Movement")]
        [SerializeField] private float jumpPower = 6.5f;
        [SerializeField] private float jumpCooldown = 0.2f;
        [SerializeField] private float airControlMultiplier = 0.2f;

        [Header("Stumble")]
        [SerializeField] private float stumbleDuration = 0.5f;
        [SerializeField] private float stumbleSlowMul = 0.35f;
        [SerializeField] private float stumbleUpKick = 2.5f;

        [Header("Ground Check")]
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float groundCheckRadius = 0.15f;
        [SerializeField] private float groundCheckOffset = 0.05f;

        [Header("Start")]
        [SerializeField] private Transform startPoint;

        public float StartZ => startPoint ? startPoint.position.z : _startZ;

        Rigidbody _rb;
        CapsuleCollider _col;
        bool _canControl = false;
        bool _canJump = true;
        bool _isStumbling = false;
        float _stumbleTimer = 0f;
        float _startZ;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            _col = GetComponent<CapsuleCollider>();
            _startZ = transform.position.z;
        }

        public void EnableControl(bool enabled)
        {
            _canControl = enabled;
            if (!enabled) _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }

        public void ResetToStart()
        {
            Vector3 pos = startPoint ? startPoint.position : new Vector3(transform.position.x, transform.position.y, _startZ);
            transform.position = pos;
            _rb.linearVelocity = Vector3.zero;
            _canJump = true;
            _isStumbling = false;
            _stumbleTimer = 0f;
            //animator?.SetBool("Run", false);
            //animator?.SetBool("Stumble", false);
        }

        public void DashForward(float speed)
        {
            float mul = (_isStumbling) ? stumbleSlowMul : 1f;
            float targetZVel = speed * mul;

            Vector3 vel = _rb.linearVelocity;
            vel.z = targetZVel;

            if (!IsGrounded())
                vel.z = Mathf.Lerp(_rb.linearVelocity.z, targetZVel, Time.deltaTime * airControlMultiplier);

            _rb.linearVelocity = vel;

            //animator?.SetBool("Run", _canControl && !_isStumbling);
        }

        public void TryJump()
        {
            if (!_canControl || !_canJump) return;
            if (!IsGrounded()) return;

            _canJump = false;
            Vector3 v = _rb.linearVelocity;
            v.y = 0f;
            _rb.linearVelocity = v;
            _rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);

            //animator?.SetTrigger("Jump");
            StartCoroutine(Co_JumpCooldown());
        }

        IEnumerator Co_JumpCooldown()
        {
            yield return new WaitForSeconds(jumpCooldown);
            _canJump = true;
        }

        bool IsGrounded()
        {
            Vector3 center = transform.position + Vector3.down * ((_col.height * 0.5f) - _col.radius + groundCheckOffset);
            return Physics.CheckSphere(center, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isStumbling) return;
            if (!other.collider.CompareTag("Hurdle")) return;

            StartCoroutine(Co_Stumble());
        }

        IEnumerator Co_Stumble()
        {
            _isStumbling = true;
            _stumbleTimer = 0f;
            //animator?.SetBool("Stumble", true);

            _rb.AddForce(Vector3.up * stumbleUpKick, ForceMode.VelocityChange);

            while (_stumbleTimer < stumbleDuration)
            {
                _stumbleTimer += Time.deltaTime;
                yield return null;
            }

            //animator?.SetBool("Stumble", false);
            _isStumbling = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (_col == null) return;
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position + Vector3.down * ((_col.height * 0.5f) - _col.radius + groundCheckOffset);
            Gizmos.DrawWireSphere(center, groundCheckRadius);
        }
    }
}
