using UnityEngine;

namespace Code.Agents
{
    public interface IMovement
    {
        void SetMovementInput(Vector2 movementInput);
        void SetRunningRotation(Quaternion targetRot);
    }
}