using Code.Animations;
using UnityEngine;

namespace Code.Agents
{
    public class AgentAnimator : MonoBehaviour, IComponent, IAgentAnimator
    {
        private Agent _agent;
        private Animator _animator;
        public Animator Animator => _animator;

        public void Initialize(Agent agent)
        {
            _agent = agent;
            _animator = GetComponent<Animator>();
        }

        public void SetParameter(ParamSO param, float value)
            => _animator.SetFloat(param.HashValue, value);
        public void SetParameter(ParamSO param, float value, float dampTime)
            => _animator.SetFloat(param.HashValue, value, dampTime, Time.deltaTime);
        public void SetParameter(ParamSO param, int value)
            => _animator.SetInteger(param.HashValue, value);
        public void SetParameter(ParamSO param, bool value)
            => _animator.SetBool(param.HashValue, value);
        public void SetTrigger(ParamSO param)
            => _animator.SetTrigger(param.HashValue);
        public void ReSetTrigger(ParamSO param)
            => _animator.ResetTrigger(param.HashValue);
    }
}