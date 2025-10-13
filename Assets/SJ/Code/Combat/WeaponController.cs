using Code.Agents;
using Code.Animations;
using System;
using UnityEngine;

public class WeaponController : MonoBehaviour, IComponent
{
    [SerializeField] private ParamSO fireParam;
    private Agent _agent;
    private IAgentAnimator _agentAnimator;

    private bool _isAttacking;
        
    public void Initialize(Agent agent)
    {
        _agent = agent;
    }

    public void SetAttacking(bool isAttacking) => _isAttacking = isAttacking;

    private void Update()
    {
        if(_isAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Attack");
        //_agentAnimator.SetTrigger(fireParam);
    }
}
