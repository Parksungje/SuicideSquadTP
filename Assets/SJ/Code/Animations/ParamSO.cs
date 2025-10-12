using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Animations
{
    [CreateAssetMenu(fileName = "Animation param", menuName = "SO/Animator/Param", order = 0)]
    public class ParamSO : ScriptableObject
    {
        [field: SerializeField] public string ParamName { get; private set; }
        [field: SerializeField] public int HashValue { get; private set; }

        private void OnValidate()
        {
            HashValue = Animator.StringToHash(ParamName);
        }
    }
}