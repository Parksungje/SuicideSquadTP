using System.Collections;
using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class HurdlePlayerController : MonoBehaviour
    {
        [SerializeField] private HurdleGameManager gameManager;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform gfxRoot;

        [SerializeField] private float jumpPower = 8.5f;
        [SerializeField] private float jumpCooldown = 0.18f;

        [SerializeField] private float stumbleDuration = 0.5f;
        [SerializeField] private float stumbleSlowMul = 0.35f;
        [SerializeField] private float stumbleUpKick = 2.5f;

        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float groundCheckRadius = 0.15f;
        [SerializeField] private float groundCheckOffset = 0.05f;

        [SerializeField] private float perfectMin = 0.55f;
        [SerializeField] private float perfectMax = 0.85f;
        [SerializeField] private float timingLeniency = 0.3f;
        [SerializeField] private float edgeSnap = 0.1f;

        [SerializeField] private float speedMulPerfect = 1.25f;
        [SerializeField] private float speedMulEarly = 0.8f;
        [SerializeField] private float speedMulLate = 0.7f;
        [SerializeField] private float timingEffectDuration = 0.6f;

        [SerializeField] private float detectRange = 4.5f;
        [SerializeField] private float detectWidth = 1.4f;
        [SerializeField] private float detectHeight = 2.4f;
        [SerializeField] private LayerMask hurdleMask = ~0;

        [SerializeField] private float burstZ_Perfect = 2f;
        [SerializeField] private float burstZ_Early = -1f;
        [SerializeField] private float burstZ_Late = -1.5f;
        [SerializeField] private float burstDurationPerfect = 0.25f;
        [SerializeField] private float burstDurationEarly = 0.2f;
        [SerializeField] private float burstDurationLate = 0.25f;

        [SerializeField] private float baseSpeedMul = 1.35f;
        [SerializeField] private float fallMultiplier = 2.5f;
            
        Rigidbody _rb;
        CapsuleCollider _col;
        bool _canControl = false;
        bool _canJump = true;
        bool _isStumbling = false;
        float _stumbleTimer = 0f;

        float _timingMul = 1f;
        Coroutine _timingCo;

        float _burstAddZ = 0f;
        Coroutine _burstCo;

        Collider _lastBest;

        static readonly int HashRun = Animator.StringToHash("isRunning");
        static readonly int HashJump = Animator.StringToHash("isJumping");

        Vector3 _startPos;
        Quaternion _startRot;
        public float StartZ => _startPos.z;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            _col = GetComponent<CapsuleCollider>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
        }

        public void InitStartPosition()
        {
            _startPos = transform.position;
            _startRot = transform.rotation;
        }

        void FixedUpdate()
        {
            bool grounded = IsGrounded();
            animator?.SetBool(HashRun, _canControl && !_isStumbling && grounded);

            if (!grounded && _rb.linearVelocity.y < 0f)
                _rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        public void EnableControl(bool enabled)
        {
            _canControl = enabled;
            if (!enabled) _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }

        public void ResetToStart()
        {
            transform.position = _startPos;
            transform.rotation = _startRot;
            _rb.linearVelocity = Vector3.zero;
            _canJump = true;
            _isStumbling = false;
            _stumbleTimer = 0f;
            _timingMul = 1f;
            if (_timingCo != null) { StopCoroutine(_timingCo); _timingCo = null; }
            _burstAddZ = 0f;
            if (_burstCo != null) { StopCoroutine(_burstCo); _burstCo = null; }
            if (animator) { animator.ResetTrigger(HashJump); animator.SetBool(HashRun, false); }
        }

        public void DashForward(float speed)
        {
            float mul = _isStumbling ? stumbleSlowMul : 1f;
            float targetZVel = speed * baseSpeedMul * mul * _timingMul + _burstAddZ;

            Vector3 vel = _rb.linearVelocity;
            if (IsGrounded())
                vel.z = targetZVel;
            else
                vel.z = Mathf.Max(_rb.linearVelocity.z, targetZVel);

            _rb.linearVelocity = vel;
        }

        public void TryJump()
        {
            if (!_canControl || !_canJump || !IsGrounded()) return;
            EvaluateJumpTiming();
            _canJump = false;
            Vector3 v = _rb.linearVelocity; v.y = 0f; _rb.linearVelocity = v;
            _rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            animator?.SetTrigger(HashJump);
            StartCoroutine(Co_JumpCooldown());
        }

        void EvaluateJumpTiming()
        {
            Vector3 fwd = transform.forward.normalized;
            Vector3 up = Vector3.up;
            Vector3 right = Vector3.Cross(up, fwd).normalized;
            Vector3 center = transform.position + fwd * (detectRange * 0.5f) + up * (detectHeight * 0.5f);
            Vector3 halfExt = new Vector3(detectWidth * 0.5f, detectHeight * 0.5f, detectRange * 0.5f);
            Quaternion rot = Quaternion.LookRotation(fwd, up);
            Collider[] buf = new Collider[16];
            int count = Physics.OverlapBoxNonAlloc(center, halfExt, buf, rot, hurdleMask, QueryTriggerInteraction.Collide);
            if (count == 0) count = Physics.OverlapBoxNonAlloc(center, halfExt, buf, rot, ~0, QueryTriggerInteraction.Collide);

            Collider best = null;
            float bestForwardDist = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                var c = buf[i];
                if (c == null) continue;
                bool isHurdle = c.CompareTag("Hurdle");
                if (!isHurdle && !(c.transform.parent != null && c.transform.parent.CompareTag("Hurdle"))) continue;
                Vector3 to = c.bounds.center - transform.position;
                float forwardDist = Vector3.Dot(to, fwd);
                if (forwardDist < 0f) continue;
                float lateral = Mathf.Abs(Vector3.Dot(to, right));
                if (lateral > detectWidth * 0.9f) continue;
                if (forwardDist < bestForwardDist) { bestForwardDist = forwardDist; best = c; }
            }
            _lastBest = best;
            if (best == null) { ApplyTimingEffect(TimingGrade.None); return; }

            float pMin = Mathf.Max(0f, perfectMin - timingLeniency);
            float pMax = perfectMax + timingLeniency;
            bool snapPerfect = Mathf.Abs(bestForwardDist - perfectMin) <= edgeSnap || Mathf.Abs(bestForwardDist - perfectMax) <= edgeSnap;
            TimingGrade grade = (snapPerfect || (bestForwardDist >= pMin && bestForwardDist <= pMax)) ? TimingGrade.Perfect :
                                (bestForwardDist > pMax) ? TimingGrade.Early : TimingGrade.Late;
            ApplyTimingEffect(grade);
        }

        enum TimingGrade { None, Early, Perfect, Late }

        void ApplyTimingEffect(TimingGrade grade)
        {
            float mul = 1f, burst = 0f, burstDur = 0f;
            switch (grade)
            {
                case TimingGrade.Perfect: mul = speedMulPerfect; burst = burstZ_Perfect; burstDur = burstDurationPerfect; break;
                case TimingGrade.Early: mul = speedMulEarly; burst = burstZ_Early; burstDur = burstDurationEarly; break;
                case TimingGrade.Late: mul = speedMulLate; burst = burstZ_Late; burstDur = burstDurationLate; break;
            }
            SetTimingMul(mul);
            if (_burstCo != null) StopCoroutine(_burstCo);
            _burstCo = StartCoroutine(Co_BurstZ(burst, burstDur));
        }

        IEnumerator Co_BurstZ(float addZ, float dur)
        {
            _burstAddZ = addZ;
            float t = 0f;
            while (t < dur) { t += Time.deltaTime; _burstAddZ = addZ * (1f - Mathf.Clamp01(t / dur)); yield return null; }
            _burstAddZ = 0f; _burstCo = null;
        }

        void SetTimingMul(float mul)
        {
            if (_timingCo != null) StopCoroutine(_timingCo);
            _timingCo = StartCoroutine(Co_TimingMul(mul, timingEffectDuration));
        }

        IEnumerator Co_TimingMul(float mul, float duration)
        {
            _timingMul = mul;
            float t = 0f;
            while (t < duration) { t += Time.deltaTime; yield return null; }
            _timingMul = 1f; _timingCo = null;
        }

        IEnumerator Co_JumpCooldown() { yield return new WaitForSeconds(jumpCooldown); _canJump = true; }

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

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Hurdle") || _rb.linearVelocity.y > 0.1f) return;
            if (!_isStumbling) StartCoroutine(Co_Stumble());
            Vector3 vel = _rb.linearVelocity; vel.z *= stumbleSlowMul; _rb.linearVelocity = vel;
        }

        IEnumerator Co_Stumble()
        {
            _isStumbling = true; _stumbleTimer = 0f;
            _rb.AddForce(Vector3.up * stumbleUpKick, ForceMode.VelocityChange);
            while (_stumbleTimer < stumbleDuration) { _stumbleTimer += Time.deltaTime; yield return null; }
            _isStumbling = false;
        }
    }
}
