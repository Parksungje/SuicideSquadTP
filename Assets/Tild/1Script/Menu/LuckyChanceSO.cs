using Tild.Chest;
using UnityEngine;

namespace Tild.Menu
{
    [CreateAssetMenu(fileName = "LuckyChanceSO", menuName = "Tild/LuckyChanceSO")]
    public class LuckyChanceSO : ScriptableObject
    {
        [Header("희귀도별 확률 (총합 100)")]
        public float commonChance = 40f;
        public float uncommonChance = 30f;
        public float rareChance = 15f;
        public float epicChance = 10f;
        public float legendaryChance = 5f;

        public Rarity GetRandomRarity()
        {
            float randomValue = Random.Range(0f, 100f);
            float cumulative = 0f;

            cumulative += commonChance;
            if (randomValue < cumulative) return Rarity.Common;

            cumulative += uncommonChance;
            if (randomValue < cumulative) return Rarity.Uncommon;

            cumulative += rareChance;
            if (randomValue < cumulative) return Rarity.Rare;

            cumulative += epicChance;
            if (randomValue < cumulative) return Rarity.Epic;

            return Rarity.Legendary;
        }
    }

    
}