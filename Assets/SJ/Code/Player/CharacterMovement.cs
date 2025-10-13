using System;
using Code.Agents;
using Code.Animations;
using UnityEngine;

namespace Code.Players
{
    public class CharacterMovement : MonoBehaviour, IComponent, IMovement
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        private Agent _agent;
        private Vector3 _inputMovement;
        private Quaternion _targetRotation;

        private CharacterController _characterController;
        private AgentAnimator _agentAnimtor;

        public void Initialize(Agent agent)
        {
            _characterController = agent.GetComponent<CharacterController>();
            _agentAnimtor = agent.GetCompo<AgentAnimator>();
            _agent = agent;
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            // Z축을 위/아래로, X축을 좌/우로만 사용
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
            float x = _inputMovement.x;
            float z = _inputMovement.z;
            // _agentAnimtor.SetParameter(xVelocityParam, x);
            // _agentAnimtor.SetParameter(zVelocityParam, z);
        }
    }
}
