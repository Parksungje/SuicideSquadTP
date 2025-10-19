using UnityEngine;

public class BallCollider : MonoBehaviour
{
    private VolleyBallController controller;
    private Rigidbody rb;

    public void Init(VolleyBallController c)
    {
        controller = c;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        controller.ResetBallSpeed(rb);
    }
}
