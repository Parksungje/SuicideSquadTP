using Code.Agents;
using System;
using Tild.Chest;
using UnityEngine;

namespace Code.Players
{
    public class CharacterMovement : MonoBehaviour, IComponent, IMovement
    {
        [SerializeField] private float moveSpeed = 5f;
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
            MoveCharacter();
        }

        private void CalculateMovement()
        {
            _inputMovement = _inputMovement.normalized;
        }

        private void MoveCharacter()
        {
            Vector3 move = _inputMovement * moveSpeed * Time.fixedDeltaTime;
            _characterController.Move(move);

            Vector3 pos = _agent.transform.position;
            pos.y = 0f;
            _agent.transform.position = pos;

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
