using UnityEngine;

public class SmoothThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float followSmoothness = 10f;

    private float _yaw;
    private float _pitch;

    private float _currentYaw;
    private float _currentPitch;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        _yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        _pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        float targetPitch = Mathf.Clamp(_pitch, -80f, 80f);
        _currentPitch = Mathf.Lerp(_currentPitch, targetPitch, Time.deltaTime * followSmoothness);

        _currentYaw = Mathf.Lerp(_currentYaw, _yaw, Time.deltaTime * followSmoothness);

        Quaternion rotation = Quaternion.Euler(_currentPitch, _currentYaw, 0f);
        Vector3 desiredPosition = target.position - rotation * Vector3.forward * distance + Vector3.up * height;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSmoothness);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
