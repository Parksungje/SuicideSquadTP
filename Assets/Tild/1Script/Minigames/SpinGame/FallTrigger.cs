using System;
using UnityEngine;

namespace Tild.Minigames.SpinGame
{
    public class FallTrigger : MonoBehaviour
    {
        private bool isFalled;
        private void OnCollisionEnter(Collision other)
        {
            if (other.rigidbody != null && isFalled == false)
            {
                isFalled = true;
                SpinGameManager.instance.Falled(other.rigidbody);
            }
        }
    }
}