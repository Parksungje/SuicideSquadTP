using UnityEngine;
using UnityEngine.Events;

namespace Code.Agents
{
    public class AimComponent : MonoBehaviour, IComponent
    {
        [SerializeField] private Transform aimTrm;

        private Agent _agent;
        private Vector3 _aimPosition;
        private Vector3 _prevLookDirection;
        private IMovement _movement;

        //public UnityEvent<Quaternion> OnAimDirectionChange;

        public void Initialize(Agent agent)
        {
            _agent = agent;
            _movement = agent.GetCompo<IMovement>();
        }

        public void SetAimPosition(Vector3 aimPosition)
        {
            _aimPosition = aimPosition;
            aimTrm.position = _aimPosition;
        }

        private void Update()
        {
            UpdateLookDirection();
        }

        private void UpdateLookDirection()
        {
            Vector3 lookDirection = _aimPosition - transform.position;
            lookDirection.y = 0;
            if (_prevLookDirection != lookDirection)
            {
                _movement.SetRunningRotation(Quaternion.LookRotation(lookDirection.normalized));
                //OnAimDirectionChange?.Invoke(Quaternion.LookRotation(lookDirection.normalized));
                _prevLookDirection = lookDirection;
            }
        }
    }
}