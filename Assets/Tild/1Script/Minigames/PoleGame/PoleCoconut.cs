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

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Player"))
            {
                transform.DOScale(Vector3.zero, 0.3f);
                transform.DOShakePosition(0.6f, 5, 3, 3).OnComplete(
                    () =>
                    {
                        Destroy(gameObject);                                      
                    });
                if (other.attachedRigidbody != null)
                {
                    
                    other.attachedRigidbody.useGravity = true;
                    other.attachedRigidbody.linearVelocity = Vector3.down * 50;
                    PoleGameManager.instance.GetFall(other.attachedRigidbody);
                    StartCoroutine(FallDelay(other.attachedRigidbody));
                  
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