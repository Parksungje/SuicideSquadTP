using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tild.Chest;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Tild.Menu
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private LuckyChanceSO chances;
        [SerializeField] private List<WeaponRarity> weaponRarities;
        [SerializeField] private Volume volumeShaking;
        [SerializeField] private Volume volume2;
        [SerializeField] private ParticleSystem chestParticles;
        [SerializeField] private ParticleSystem itemParticles;
        [SerializeField] private CanvasGroup weaponsGroup;
        [SerializeField] private TextMeshProUGUI weaponName;
        [SerializeField] private Transform ItemParent;
        [SerializeField] private CanvasGroup chestInfo;
        private Vector3 scaleTemp;
        private Quaternion rotTemp;
        
        private void Awake()
        {
            scaleTemp = transform.localScale;
            rotTemp = transform.localRotation;
        }

        public void ShakingChest(float value, float duration = 0.5f)
        {
            volumeShaking.weight = value;   
        }

        public void OpenChest()
        {
            StartCoroutine(OpenChestCoroutine());
        }

        private IEnumerator OpenChestCoroutine()
        {
            transform.DOShakeScale(0.5f, 10f, 10, 90);
            transform.DOShakeRotation(0.5f, 180f, 10, 90);
            chestParticles.Play();
            yield return new WaitForSeconds(0.1f);

            transform.DOScale(3,0.4f).OnComplete(() =>
            {
                transform.DOScale(0f, 0.1f);
            });
            DOTween.To(
                () => volumeShaking.weight,
                x => volumeShaking.weight = x,
                0,
                0.7f
            ).SetEase(Ease.InOutSine);
            
            DOTween.To(
                () => volume2.weight,
                x => volume2.weight = x,
                1,
                0.7f
            ).SetEase(Ease.InOutSine);
            
            yield return new WaitForSeconds(0.9f);
            
            DOTween.To(
                () => volume2.weight,
                x => volume2.weight = x,
                0,
                0.25f
            ).SetEase(Ease.InOutSine);
        
           
            
            yield return new WaitForSeconds(0.8f);
            WeaponData weaponData = GetItem();
            itemParticles.Play();
            weaponsGroup.DOFade(1f, 0.5f);
            weaponName.text = weaponData.WeaponName;
            
            
            GameObject prefab = Instantiate(weaponData.WeaponPrefab);
            Vector3 prevScale = prefab.transform.localScale;
            prefab.transform.SetParent(ItemParent, false);
            prefab.transform.localScale = Vector3.zero;
            prefab.transform.DOScale(prevScale, 0.5f);
          
            yield return new WaitForSeconds(4f);
            prefab.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                Destroy(prefab);
            });
            weaponsGroup.DOFade(0f, 0.5f);
            
            yield return new WaitForSeconds(1f);
            weaponsGroup.DOFade(0f, 0.5f);
            transform.rotation = rotTemp;
            transform.DOScale(scaleTemp, 0.5f).SetEase(Ease.Unset);
            chestInfo.DOFade(1f, 0.5f);
           
        }
        public WeaponData GetItem()
        {
            
            Rarity selectedRarity = chances.GetRandomRarity();
  

            WeaponRarity selectedGroup = weaponRarities.Find(r => r.rarity == selectedRarity);

            if (selectedGroup.weapons == null || selectedGroup.weapons.Count == 0)
            {
              
                return null;
            }

  
            int index = Random.Range(0, selectedGroup.weapons.Count);
            WeaponData chosenWeapon = selectedGroup.weapons[index];

        
            return chosenWeapon;

        }
    }
}