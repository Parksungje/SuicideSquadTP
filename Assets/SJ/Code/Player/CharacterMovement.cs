using Code.Agents;
using UnityEngine;

namespace Code.Player
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float gravity;

        private Agent _agent;
        private Vector3 _inputMovement;
        private float _yVelocity;
        private Vector3 _movement;

        [SerializeField]private CharacterController _characterController;

        public bool IsGround => _characterController.isGrounded;
        public bool IsRunning { get; private set; }

        public void Initialize(Agent agent)
        {
            _characterController = agent.GetComponent<CharacterController>();
            _agent = agent;
        }
        public void SetMovementInput(Vector2 movementInput)
        {
            _movement = new Vector3(movementInput.x, 0, movementInput.y);
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
            float speed = _moveSpeed;
            _movement = _movement.normalized * (speed * Time.fixedDeltaTime);
        }

        private void ApplyGravity()
        {
            if (IsGround && _yVelocity < 0)
                _yVelocity = -2f;
            else
                _yVelocity += gravity * Time.fixedDeltaTime;

            _movement.y = _yVelocity;
        }

        private void ApplyRotation()
        {

        }

        private void MoveCharacter()
        {
            _characterController.Move(_movement);
        }


    }
}