using UnityEngine;

namespace Tild.FSM
{
    public class Boss : MonoBehaviour
    {
        protected State _currentState;

        [Header("Components")] protected Rigidbody _rb;
        protected Animator _anim;

        [Header("Settings")] 
        public float moveSpeed = 3f;
        public float attackRange = 2f;
        public Transform player;
        public bool isFacingForward = true;

        protected virtual void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();
        }

        protected virtual void Start()
        {
            ChangeState(new IdleState(this));
        }

        protected virtual void Update()
        {
            _currentState?.OnUpdate();
        }

        public void ChangeState(State newState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState?.OnEnter();
        }

        public virtual void Move(Vector3 dir)
        {
            Vector3 move = dir.normalized * moveSpeed * Time.deltaTime;
            _rb.MovePosition(_rb.position + move);

            if (move != Vector3.zero)
                RotateTowards(dir);


        }

        public virtual void StopMoving()
        {
        }

        protected void RotateTowards(Vector3 dir)
        {
            if (dir == Vector3.zero) return;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f * Time.deltaTime);
        }

    }
}

