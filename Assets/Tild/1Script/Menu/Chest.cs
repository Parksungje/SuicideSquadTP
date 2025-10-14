using System.Collections.Generic;
using Tild.Chest;
using UnityEngine;

namespace Tild.Menu
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private LuckyChanceSO chances;
        [SerializeField] private List<WeaponRarity> weaponRarities;

        public void OpenChest()
        {
      
            Rarity selectedRarity = chances.GetRandomRarity();
            Debug.Log($"선택된 희귀도: {selectedRarity}");

            WeaponRarity selectedGroup = weaponRarities.Find(r => r.rarity == selectedRarity);

            if (selectedGroup.weapons == null || selectedGroup.weapons.Count == 0)
            {
                Debug.LogWarning($"{selectedRarity} 희귀도의 무기가 없습니다!");
                return;
            }

  
            int index = Random.Range(0, selectedGroup.weapons.Count);
            WeaponData chosenWeapon = selectedGroup.weapons[index];

            Debug.Log($"뽑힌 무기: {chosenWeapon.WeaponName}");
            
        }
    }
}