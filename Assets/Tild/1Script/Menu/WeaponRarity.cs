using System;
using System.Collections.Generic;
using Tild.Chest;
using UnityEngine;

namespace Tild.Menu
{
    [Serializable]
    public struct WeaponRarity
    {
        public Rarity rarity;
        public List<WeaponData> weapons;
    }
}