using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    public class FinishLineBillboard : MonoBehaviour
    {
        [SerializeField] private Transform lookTarget;

        void LateUpdate()
        {
            if (lookTarget == null) return;
            Vector3 fwd = (lookTarget.position - transform.position);
            fwd.y = 0f;
            if (fwd.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(fwd.normalized, Vector3.up);
        }
    }
}
