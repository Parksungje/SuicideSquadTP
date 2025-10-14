using System;
using Code.Agents;
using UnityEngine;

namespace Code.Players
{
    public class CharacterMovement : MonoBehaviour, IComponent, IMovement
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private Animator animator;

        private Agent _agent;
        private Vector3 _inputMovement;
        private Vector3 _velocity;
        private Quaternion _targetRotation;
        private CharacterController _characterController;

        private readonly int IsRunningHash = Animator.StringToHash("isRunning");
        private readonly int IsJumpingHash = Animator.StringToHash("isJumping");

        public void Initialize(Agent agent)
        {
            _characterController = agent.GetComponent<CharacterController>();
            _agent = agent;

            if (animator == null)
                animator = agent.GetComponentInChildren<Animator>();
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            _inputMovement = new Vector3(movementInput.x, 0f, movementInput.y);
        }

        public void SetRunningStatus(bool isRunning) { }

        public void SetRunningRotation(Quaternion targetRotation)
        {
            _targetRotation = targetRotation;
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyRotation();
            MoveCharacter();
        }

        private void CalculateMovement()
        {
            _inputMovement = _inputMovement.normalized;
        }

        private void ApplyRotation()
        {
            if (_inputMovement.sqrMagnitude > 0.001f)
            {
                Vector3 direction = new Vector3(_inputMovement.x, 0f, _inputMovement.z);
                Quaternion targetRot = Quaternion.LookRotation(direction);
                Vector3 euler = targetRot.eulerAngles;
                euler.x = 0f;
                euler.z = 0f;
                targetRot = Quaternion.Euler(euler);

                _agent.transform.rotation = Quaternion.Slerp(
                    _agent.transform.rotation,
                    targetRot,
                    rotationSpeed * Time.fixedDeltaTime
                );
            }
        }

        private void MoveCharacter()
        {
            Vector3 move = _inputMovement * moveSpeed;
            _characterController.Move(move * Time.fixedDeltaTime);

            Vector3 pos = _agent.transform.position;
            pos.y = _characterController.transform.position.y;
            _agent.transform.position = pos;
        }

        public void Jump()
        {
            if (_characterController.isGrounded)
            {
                if (_velocity.y < 0f)
                    _velocity.y = -2f;

                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetBool(IsJumpingHash, true);
            }
            else
            {
                _velocity.y += gravity * Time.fixedDeltaTime;
                animator.SetBool(IsJumpingHash, false);
            }

            _characterController.Move(_velocity * Time.fixedDeltaTime);
        }

        private void Update()
        {
            UpdateMovementMotion();
        }

        private void UpdateMovementMotion()
        {
            bool isMoving = _inputMovement.sqrMagnitude > 0.001f;
            animator.SetBool(IsRunningHash, isMoving);
        }
    }
}
