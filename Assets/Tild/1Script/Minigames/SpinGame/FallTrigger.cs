using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tild.Minigames.SpinGame
{
    public class FallTrigger : MonoBehaviour
    {
        private bool isFalled;
        public Action<Rigidbody> onFall;
        private void OnCollisionEnter(Collision other)
        {
            if (other.rigidbody != null && isFalled == false)
            {
                isFalled = true;
                onFall.Invoke((Rigidbody)other.rigidbody);
            }
        }
    }
}