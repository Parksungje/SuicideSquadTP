using System;
using Code.Agents;
using Code.Animations;
using UnityEngine;

namespace Code.Players
{
    public class CharacterMovement : MonoBehaviour, IComponent, IMovement
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private ParamSO moveParam;

        private Agent _agent;
        private Vector3 _inputMovement;
        private Vector3 _movement;
        private Quaternion _targetRotation;

        private CharacterController _characterController;
        private AgentAnimator _agentAnimator;

        public bool IsGround => _characterController.isGrounded;
        public bool IsRunning { get; private set; }

        public void Initialize(Agent agent)
        {
            _characterController = agent.GetComponent<CharacterController>();
            _agentAnimator = agent.GetCompo<AgentAnimator>();
            _agent = agent;
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            _inputMovement = new Vector3(movementInput.x, 0f, movementInput.y);
        }

        public void SetRunningStatus(bool isRunning)
        {
            IsRunning = isRunning;
        }

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
            float speed = moveSpeed;
            _movement = _inputMovement.normalized * (moveSpeed * Time.deltaTime);
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
            _characterController.Move(_movement);
        }

        private void Update()
        {
            UpdateMovementMotion();
        }

        private void UpdateMovementMotion()
        {
            //float x = _inputMovement.x;
            //float z = _inputMovement.z;
            //_agentAnimator.SetParameter(moveParam, IsRunning
            //        && !Mathf.Approximately(_inputMovement.magnitude, 0f));
        }

    }
}
