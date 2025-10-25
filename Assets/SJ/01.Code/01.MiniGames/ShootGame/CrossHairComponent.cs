using UnityEngine;

public class CrossHairComponent : MonoBehaviour
{
    public float raycastDistance = 100f;
    public float raycastOffset = 0.2f;
    public float rightOffset = 0.2f;
    public LayerMask hitLayers;
    public string enemyLayerName = "Enemy";

    private RaycastHit _currentHit;
    private bool _isHitting = false;
    private int _enemyLayerIndex;

    private const float GIZMO_SPHERE_RADIUS = 0.1f;

    void Start()
    {
        _enemyLayerIndex = LayerMask.NameToLayer(enemyLayerName);
        if (_enemyLayerIndex == -1)
        {
            Debug.LogError($"Layer named '{enemyLayerName}' was not found. Please create it.");
        }
        if (hitLayers.value == 0)
        {
            hitLayers = ~0;
        }
    }

    void Update()
    {
        Transform t = transform;

        Vector3 rayOrigin = t.position + t.forward * raycastOffset + t.right * rightOffset;
        Vector3 rayDirection = t.forward;

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance, hitLayers))
        {
            _isHitting = true;
            _currentHit = hit;
        }
        else
        {
            _isHitting = false;
        }
    }

    public bool IsAimingAtEnemy()
    {
        return _isHitting &&
               _enemyLayerIndex != -1 &&
               _currentHit.collider != null &&
               _currentHit.collider.gameObject.layer == _enemyLayerIndex;
    }

    private void OnDrawGizmos()
    {
        Transform t = transform;
        Vector3 rayOrigin = t.position + t.forward * raycastOffset + t.right * rightOffset;
        Vector3 rayDirection = t.forward;

        if (_isHitting)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(rayOrigin, _currentHit.point);

            if (_enemyLayerIndex != -1 && _currentHit.collider != null && _currentHit.collider.gameObject.layer == _enemyLayerIndex)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawSphere(_currentHit.point, GIZMO_SPHERE_RADIUS);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * raycastDistance);
        }
    }
}
