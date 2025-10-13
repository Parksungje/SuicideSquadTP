using System;
using UnityEngine;

namespace Tild.Chest
{
    [CreateAssetMenu(menuName = "WeaponInfo",fileName = "SO/WeaponInfo")]
    public class WeaponInfoSO : ScriptableObject
    {
        public string WeaponName;
        public Rarity WeaponRarity;
        public string WeaponDesc;
        public Sprite WeaponIcon;
    }
    [Serializable]
    public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }
}