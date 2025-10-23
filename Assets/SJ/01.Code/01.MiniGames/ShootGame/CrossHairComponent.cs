using UnityEngine;

public class CrossHairComponent : MonoBehaviour
{
    public float raycastDistance = 100f;

    private RaycastHit currentHit;
    private bool isHitting = false;

    void Start()
    {
    }

    void Update()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        rayOrigin += rayDirection * 0.2f;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance))
        {
            isHitting = true;
            currentHit = hit;

            int enemyLayerIndex = LayerMask.NameToLayer("Enemy");

            if (hit.collider.gameObject.layer == enemyLayerIndex)
            {
                Debug.Log($"Enemy 레이어 오브젝트에 닿음: {hit.collider.gameObject.name}");
            }
        }
        else
        {
            isHitting = false;
        }

    }

    private void OnDrawGizmos()
    {
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = transform.forward;

        Gizmos.color = Color.yellow;

        if (isHitting)
        {
            Gizmos.DrawLine(rayOrigin, currentHit.point);

            int enemyLayerIndex = LayerMask.NameToLayer("Enemy");

            if (currentHit.collider.gameObject.layer == enemyLayerIndex)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(currentHit.point, 0.1f);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(currentHit.point, 0.1f);
            }

        }
        else
        {
            Gizmos.DrawLine(rayOrigin, rayOrigin + rayDirection * raycastDistance);
        }
    }
}