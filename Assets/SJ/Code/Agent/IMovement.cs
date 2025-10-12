using UnityEngine;

namespace Code.Agents
{
    public interface IMovement
    {
        void SetMovementInput(Vector2 movementInput);
        void SetRunningStatus(bool isRunning);
        void SetRunningRotation(Quaternion targetRotation);
    }
}