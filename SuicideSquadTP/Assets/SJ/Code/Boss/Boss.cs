using UnityEngine;
using UnityEngine.Serialization;

namespace SJ.Code.Boss
{
    public abstract class Boss : MonoBehaviour
    {
        [SerializeField] private BossDataSO bossDataSO;
    }
}
