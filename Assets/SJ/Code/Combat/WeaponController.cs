using Code.Agents;
using System;
using UnityEngine;

public class WeaponController : MonoBehaviour, IComponent
{
    [SerializeField] private Animator animator;
    private Agent _agent;

    private bool _isAttacking;
    private readonly int AttackHash = Animator.StringToHash("isAttacking");

    public void Initialize(Agent agent)
    {
        _agent = agent;

        if (animator == null)
            animator = agent.GetComponentInChildren<Animator>();
    }

    public void SetAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }

    private void Update()
    {
        if (_isAttacking)
        {
            Attack();
            _isAttacking = false;
        }
    }

    private void Attack()
    {
        animator.SetTrigger(AttackHash);
    }
}
