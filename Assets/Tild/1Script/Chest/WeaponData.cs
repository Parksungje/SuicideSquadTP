using System;
using UnityEngine;

namespace Tild.Chest
{
    [CreateAssetMenu(menuName = "WeaponData",fileName = "SO/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public string WeaponName;
        public Rarity WeaponRarity;
        public string WeaponDesc;
        public Sprite WeaponIcon;
        public GameObject WeaponPrefab;
    }
    [Serializable]
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }
}