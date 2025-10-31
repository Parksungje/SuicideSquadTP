using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tild.Minigames.PushGame
{
    public class PushFallTrigger : MonoBehaviour
    {
        private bool isFalled;
        public Action<Rigidbody> onFall;
        private void OnCollisionEnter(Collision other)
        {
            if (other.rigidbody != null && isFalled == false && other.gameObject.CompareTag("Player"))
            {
                isFalled = true;
                PushGameManager.instance.Falled(other.rigidbody);
            }
        }
    }
}