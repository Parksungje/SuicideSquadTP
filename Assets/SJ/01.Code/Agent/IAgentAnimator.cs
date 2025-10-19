using Code.Animations;
using UnityEngine;

namespace Code.Agents
{
    public interface IAgentAnimator
    {
        Animator Animator { get; }
        void SetParameter(ParamSO param, float value);
        void SetParameter(ParamSO param, float value, float dampTime);
        void SetParameter(ParamSO param, int value);
        void SetParameter(ParamSO param, bool value);
        void SetTrigger(ParamSO param);
        void ReSetTrigger(ParamSO param);
    }
}