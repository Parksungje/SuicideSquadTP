using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Tild.Minigames.PoleGame
{
    public class PoleCoconut : MonoBehaviour
    {
        private Rigidbody rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        public void Shoot(float speed)
        {
            rigid.AddForce(Vector3.down * speed, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            
            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Player"))
            {
                transform.DOScale(Vector3.zero, 0.3f);
                transform.DOShakePosition(0.6f, 5, 3, 3).OnComplete(
                    () =>
                    {
                        Destroy(other.gameObject);
                    });
                if (other.rigidbody != null)
                {
                    
                    other.rigidbody.useGravity = true;
                    other.rigidbody.linearVelocity = Vector3.down * 50;
                    PoleGameManager.instance.GetFall(other.rigidbody);
                    StartCoroutine(FallDelay(other.rigidbody));
                  
                }
            }
        }

        private IEnumerator FallDelay(Rigidbody rigidbody)
        {
            yield return new WaitForSeconds(0.5f);
            rigidbody.linearVelocity = Vector3.zero;
            rigidbody.useGravity = false;
        }
    }
}