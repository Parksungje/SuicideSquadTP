using Code.Agents;
using UnityEngine;

namespace Code.Player
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float gravity;

        private Agent _agent;
        private Vector3 _inputMovement;
        private Vector3 _movement;

        private CharacterController _characterController;

        public bool IsGround => _characterController.isGrounded;
        public bool IsRunning { get; private set; }

        public void Initialize(Agent agent)
        {
            _characterController = agent.GetComponent<CharacterController>();
            _agent = agent;
        }
    }
}