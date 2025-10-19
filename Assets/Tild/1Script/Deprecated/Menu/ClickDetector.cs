using System.Collections;
using UnityEngine;
using DG.Tweening;
using Tild.Chest;
using Code.Player;

namespace Tild.Menu
{
    public class ClickDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask clickableLayer;
        [SerializeField] private BaseInputSO playerInputSO;
        [SerializeField] private float holdTime = 2f;
        [SerializeField] private float shakeDuration = 0.2f;
        [SerializeField] private float shakeStrength = 2f;
        [SerializeField] private float followSpeed = 5f;
        [SerializeField] private CanvasGroup chestInfo;
        [SerializeField] private ParticleSystem chargingParticles;
        private Chest currentChest;

        private Camera _mainCamera;
        private Coroutine _holdCoroutine;
        private bool _isHolding;
        private Quaternion startRot;
        private bool _isStarted;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                StartHoldCheck();

            if (Input.GetMouseButtonUp(0))
                StopHoldCheck();
        }

        private void StartHoldCheck()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, clickableLayer))
            {
                Chest chest = hit.collider.GetComponent<Chest>();
                if (chest != null)
                    _holdCoroutine = StartCoroutine(HoldToOpenChest(chest));
            }
        }

        private void StopHoldCheck()
        {
            
            if (_holdCoroutine != null && _isHolding)
            {
                currentChest.ShakingChest(0f);
                currentChest.transform.DOKill();
                currentChest.transform.DORotateQuaternion(startRot, 0.3f).SetEase(Ease.OutSine);
                _isHolding = false; 
                StopCoroutine(_holdCoroutine);
                chestInfo.DOFade(1f, 0.5f);      
                chargingParticles.Stop();
            }
         
     
      
        }

        private IEnumerator HoldToOpenChest(Chest chest)
        {
            currentChest = chest;
            _isHolding = true;
            float timer = 0f;
            chargingParticles.Play();
            Transform chestTransform = chest.transform;
            startRot = chestTransform.rotation;
            chestInfo.DOFade(0f, 0.5f);


            while (_isHolding && timer < holdTime)
            {
                timer += Time.deltaTime;
                float weight = Mathf.Clamp01(timer / holdTime);
                chest.ShakingChest(weight);

                chestTransform.DOShakeRotation(shakeDuration, shakeStrength, 10, 90, false)
                    .SetEase(Ease.Linear);

                yield return null;
            }
            
            chest.OpenChest();
            chargingParticles.Stop();

            chest.ShakingChest(0f);
            chest.transform.DOKill();
            chestTransform.DORotateQuaternion(startRot, 0.3f).SetEase(Ease.OutSine);
            _isHolding = false;
        }
    }
}
