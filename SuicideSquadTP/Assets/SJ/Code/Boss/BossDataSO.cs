using UnityEngine;

namespace SJ.Code.Boss
{
    [CreateAssetMenu(fileName = "BossData", menuName = "SO/Boss/BossData", order = 0)]
    public class BossDataSO : ScriptableObject
    {
        public int maxHealth;
        public float attackTime;
    }
}