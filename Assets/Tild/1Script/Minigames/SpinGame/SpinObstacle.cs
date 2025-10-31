using System;
using UnityEngine;

namespace Tild.Minigames.SpinGame
{
    public class SpinObstacle : MonoBehaviour
    {
        [SerializeField] private float knockbackForce = 15f;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody != null)
            {
                Vector3 hitNormal = collision.contacts[0].normal;
                collision.transform.Find("Hitted").GetComponent<ParticleSystem>().Play();

                Vector3 knockbackDir = -hitNormal.normalized;
         
                collision.rigidbody.linearVelocity = Vector3.zero;
                collision.rigidbody.AddForce((knockbackDir * knockbackForce) + Vector3.down * 10, ForceMode.Impulse);
            }
        }
    }
}