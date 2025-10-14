using Code.Agents;
using System;
using Tild.Chest;
using UnityEngine;

namespace Code.Players
{
    public class CharacterMovement : MonoBehaviour, IComponent, IMovement
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float jumpHeight = 2f;
        [SerializeField] private float gravity = -2;
        [SerializeField] private Animator animator;

        private Agent _agent;
        private Vector3 _inputMovement;
        private Quaternion _targetRotation;
        private Vector3 _velocity;
        private CharacterController _characterController;
        private bool _isMoving;

        private readonly int IsMovingHash = Animator.StringToHash("isRunning");
        private readonly int IsJumpingHash = Animator.StringToHash("isJumping");

        public bool IsMoving => _isMoving;

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
            Jump();
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
            Vector3 move = _inputMovement * moveSpeed * Time.fixedDeltaTime;
            _characterController.Move(move);

            Vector3 pos = _agent.transform.position;
            pos.y = 0f;
            _agent.transform.position = pos;

        }

        public void Jump()
        {
            if (_characterController.isGrounded)
            {
                if (_velocity.y < 0f)
                    _velocity.y = -2f;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    animator.SetBool(IsJumpingHash, true);
                }
                else
                {
                    animator.SetBool(IsJumpingHash, false);
                }
            }
            else
            {
                _velocity.y += gravity * Time.fixedDeltaTime;
            }

            _characterController.Move(_velocity * Time.fixedDeltaTime);
        }

        private void Update()
        {
            UpdateMovementMotion();
        }

        private void UpdateMovementMotion()
        {
            _isMoving = _inputMovement.sqrMagnitude > 0.001f;

            animator.SetBool(IsMovingHash, _isMoving);
        }
    }
}
