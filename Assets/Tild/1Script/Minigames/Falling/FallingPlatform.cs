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
        private bool _isTriggered;

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            collider = GetComponent<MeshCollider>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isTriggered) return;
            
            _isTriggered = true;
                
            meshRenderer.material.DOColor(Color.red, duration);
            SoundManager.Instance.Play("Fall_Tile");
            transform.DOPunchRotation(Vector3.down, duration,20,2).OnComplete(() =>
            {
                
                rigidBody.useGravity = true;
                rigidBody.isKinematic = false;
                collider.enabled = false;
                rigidBody.AddForce(Vector3.down * 50f, ForceMode.Acceleration);
            });

        }
    }
}