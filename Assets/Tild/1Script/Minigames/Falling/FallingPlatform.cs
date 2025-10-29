using System;
using DG.Tweening;
using UnityEngine;

namespace Tild.Minigames.Falling
{
    public class FallingPlatform : MonoBehaviour
    {
        private Rigidbody rigidBody;
        private MeshCollider collider;
        [SerializeField] private float duration = 0.5f;
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            collider = GetComponent<MeshCollider>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnCollisionEnter(Collision other)
        {
            meshRenderer.material.DOColor(Color.red, duration);
            
            transform.DOShakePosition(duration,1,20,2).OnComplete(() =>
            {
             
                rigidBody.useGravity = true;
                rigidBody.isKinematic = false;
                collider.enabled = false;
                rigidBody.AddForce(Vector3.down * 50f, ForceMode.Acceleration);
            });

        }
    }
}