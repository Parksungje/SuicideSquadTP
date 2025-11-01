using UnityEngine;

public class TextFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f);
    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position + offset;

        transform.rotation = initialRotation;
    }
}
