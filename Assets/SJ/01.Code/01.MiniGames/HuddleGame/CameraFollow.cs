using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform targetA;
        [SerializeField] private Transform targetB;
        [SerializeField] private Vector3 offsetDir = new Vector3(0f, 6f, -10f);
        [SerializeField] private float smoothTime = 0.2f;
        [SerializeField] private float minDistance = 10f;   // 최소 거리
        [SerializeField] private float maxDistance = 30f;   // 최대 거리
        [SerializeField] private float zoomFactor = 1.5f;   // 타깃 간 거리 대비 카메라 거리 배수

        private Vector3 _vel;

        void LateUpdate()
        {
            if (targetA == null || targetB == null) return;

            Vector3 mid = (targetA.position + targetB.position) * 0.5f;

            float distance = Vector3.Distance(targetA.position, targetB.position);

            float camDistance = Mathf.Clamp(distance * zoomFactor, minDistance, maxDistance);

            Vector3 desired = mid + offsetDir.normalized * camDistance;

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _vel, smoothTime);

            transform.rotation = Quaternion.LookRotation((mid - transform.position).normalized, Vector3.up);
        }
    }
}
