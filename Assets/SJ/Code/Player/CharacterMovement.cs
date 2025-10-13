using System;
using System.Runtime.InteropServices;
using Code.Agents;
using Code.Animations;
using UnityEngine;

namespace Code.Players
{
    public class CharacterMovement : MonoBehaviour, IComponent, IMovement
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float gravity;
        [SerializeField] private float rotationSpeed;

        //[SerializeField] private ParamSO xVelocityParam;
        //[SerializeField] private ParamSO zVelocityParam;
        //[SerializeField] private ParamSO isRunningParam;
        //[SerializeField] private float dampTime = 0.1f;

        private Agent _agent;
        private Vector3 _inputMovement;
        private Vector3 _movement;
        private float _yVelocity;
        private Quaternion _targetRotation;

        private CharacterController _characterController;
        private AgentAnimator _agentAnimtor;
        public bool IsGround => _characterController.isGrounded;

        public void Initialize(Agent agent)
        {
            _characterController = agent.GetComponent<CharacterController>();
            _agentAnimtor = agent.GetCompo<AgentAnimator>();
            _agent = agent;
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            _inputMovement = new Vector3(movementInput.x, 0, movementInput.y);
        }

        public void SetRunningStatus(bool isRunning)
        {

        }

        public void SetRunningRotation(Quaternion targetRotation)
        {
            _targetRotation = targetRotation;
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyGravity();
            ApplyRotation();
            MoveCharacter();
        }

        private void CalculateMovement()
        {
            float speed = moveSpeed;
            _movement = _inputMovement.normalized * (speed * Time.deltaTime);
        }

        private void ApplyGravity()
        {
            if (IsGround && _yVelocity < 0)
                _yVelocity = -2f; //땅에 붙어있게 하기 위해서 약간의 음수값을 넣어줘야 합니다.
            else
                _yVelocity += gravity * Time.deltaTime;
            _movement.y = _yVelocity;
        }

        private void ApplyRotation()
        {
            _agent.transform.rotation = Quaternion.Slerp(
                _agent.transform.rotation,
                _targetRotation,
                rotationSpeed * Time.fixedDeltaTime);
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
            float x = Vector3.Dot(_inputMovement, _agent.transform.right);
            float z = Vector3.Dot(_inputMovement, _agent.transform.forward);
            //_agentAnimtor.SetParameter(xVelocityParam, x);
            //_agentAnimtor.SetParameter(zVelocityParam, z);

            //_agentAnimtor.SetParameter(isRunningParam, IsRunning
            //        && !Mathf.Approximately(_inputMovement.magnitude, 0f));
        }

    }
}