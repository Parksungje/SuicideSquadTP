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
                collision.rigidbody.freezeRotation = false;
                Vector3 hitNormal = collision.contacts[0].normal;

              
                Vector3 knockbackDir = -hitNormal.normalized;

         
                collision.rigidbody.linearVelocity = Vector3.zero;
                collision.rigidbody.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
            }
        }
    }
}