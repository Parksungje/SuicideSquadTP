using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform targetA;
        [SerializeField] private Transform targetB;
        [SerializeField] private Vector3 offset = new Vector3(0f, 6f, -10f);
        [SerializeField] private float smoothTime = 0.2f;

        private Vector3 _vel;

        void LateUpdate()
        {
            if (targetA == null || targetB == null) return;

            Vector3 mid = (targetA.position + targetB.position) * 0.5f;
            Vector3 desired = mid + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _vel, smoothTime);
            transform.rotation = Quaternion.LookRotation((mid - transform.position).normalized, Vector3.up);
        }
    }
}
