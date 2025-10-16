using UnityEngine;

namespace Code.Agents
{
    public interface IMovement
    {
        bool IsMoving { get; }
        void SetMovementInput(Vector2 movementInput);
        public void SetRunningStatus(bool isRunning);
        void SetRunningRotation(Quaternion targetRot);
    }
}