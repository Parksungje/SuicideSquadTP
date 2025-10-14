using Code.Agents;
using System;
using UnityEngine;

public class WeaponController : MonoBehaviour, IComponent
{
    [SerializeField] private Animator animator;
    private Agent _agent;
    private IMovement _movement;

    private bool _isAttacking;
    private readonly int AttackHash = Animator.StringToHash("isAttacking");

    public void Initialize(Agent agent)
    {
        _agent = agent;
        _movement = agent.GetCompo<IMovement>();

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
            if (_movement != null && _movement.IsMoving)
            {
                _isAttacking = false;
                return;
            }

            Attack();
            _isAttacking = false;
        }
    }

    private void Attack()
    {
        animator.SetTrigger(AttackHash);
    }
}
