using System;
using UnityEngine;

namespace Tild.Minigames.Boxing
{
    public class BoxingManager : MonoBehaviour
    {
        [Header("Systems")]
        
        [SerializeField] private BoxingInputSO boxingInputSO;
        [SerializeField] private Rigidbody rigid1P, rigid2P;
        [SerializeField] private Animator animator1P, animator2P;

        [Header("Values")]
        [Space]
        [SerializeField] private int damage = 10;
        [SerializeField] private int criticalDamage = 20;
        [SerializeField] private float backForce = 5f;
        [SerializeField] private float dashForce = 8f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float knockbackForce = 3f;
        private int health1P = 100, health2P = 100;
        private bool debounce1P, debounce2P;
        private bool isDefending1P, isDefending2P;
        
        [Header("UI&Effects")]
        [Space]
        [SerializeField] private RectTransform healthBar1P, healthBar2P;
        [SerializeField] private ParticleSystem hitImpact1P, hitImpact2P;

        private void OnEnable()
        {
            // Player 1
            boxingInputSO.AKeyPressed += Handle1PBack;
            boxingInputSO.DKeyPressed += Handle1PDashAttack;
            boxingInputSO.SKeyPressed += Handle1PDefend;

            // Player 2
            boxingInputSO.LeftKeyPressed += Handle2PDashAttack;
            boxingInputSO.RightKeyPressed += Handle2PBack;
            boxingInputSO.DownKeyPressed += Handle2PDefend;
        }

        private void OnDisable()
        {
            boxingInputSO.AKeyPressed -= Handle1PBack;
            boxingInputSO.DKeyPressed -= Handle1PDashAttack;
            boxingInputSO.SKeyPressed -= Handle1PDefend;

            boxingInputSO.LeftKeyPressed += Handle2PDashAttack;
            boxingInputSO.RightKeyPressed += Handle2PBack;
            boxingInputSO.DownKeyPressed -= Handle2PDefend;
        }

        #region Player1

        private void Handle1PBack()
        {
            if (debounce1P) return;
            debounce1P = true;
            rigid1P.linearVelocity = Vector3.right * backForce;
            animator1P.SetTrigger("Defense");
            Invoke(nameof(Stop1PMove), 0.25f);
            Invoke(nameof(Reset1PDebounce), 0.3f);
        }

        private void Handle1PDashAttack()
        {
            if (debounce1P) return;
            debounce1P = true;
            animator1P.SetTrigger("Punch");
            rigid1P.linearVelocity = Vector3.left * dashForce;

           
            if (Vector3.Distance(rigid1P.position, rigid2P.position) < attackRange)
            {
                if (!isDefending2P)
                    TakeDamage(ref health2P, hitImpact2P, rigid2P,animator2P, Vector3.right);
            }

            Invoke(nameof(Stop1PMove), 0.25f);
            Invoke(nameof(Reset1PDebounce), 0.4f);
        }

        private void Stop1PMove() => rigid1P.linearVelocity = Vector3.zero;

        private void Handle1PDefend()
        {
            isDefending1P = true;
            CancelInvoke(nameof(Stop1PDefend));
            Invoke(nameof(Stop1PDefend), 1.5f);
        }

        private void Stop1PDefend()
        {
            isDefending1P = false;
       
        }

        private void Reset1PDebounce() => debounce1P = false;

        #endregion


        #region Player2

        private void Handle2PBack()
        {
            if (debounce2P) return;
            debounce2P = true;
            rigid2P.linearVelocity = Vector3.left * backForce;
            animator2P.SetTrigger("Defense");
            Invoke(nameof(Stop2PMove), 0.25f);
            Invoke(nameof(Reset2PDebounce), 0.3f);
        }

        private void Handle2PDashAttack()
        {
            if (debounce2P) return;
            debounce2P = true;
            animator2P.SetTrigger("Punch");
            rigid2P.linearVelocity = Vector3.right * dashForce;

            if (Vector3.Distance(rigid2P.position, rigid1P.position) < attackRange)
            {
                if (!isDefending1P)
                    TakeDamage(ref health1P, hitImpact1P, rigid1P,animator1P, Vector3.left);
            }

            Invoke(nameof(Stop2PMove), 0.25f);
            Invoke(nameof(Reset2PDebounce), 0.4f);
        }

        private void Stop2PMove() => rigid2P.linearVelocity = Vector3.zero;

        private void Handle2PDefend()
        {
            isDefending2P = true;
           
            CancelInvoke(nameof(Stop2PDefend));
            Invoke(nameof(Stop2PDefend), 1.5f);
        }

        private void Stop2PDefend()
        {
            isDefending2P = false;
           
        }

        private void Reset2PDebounce() => debounce2P = false;

        #endregion
        
        private void TakeDamage(ref int health, ParticleSystem impact, Rigidbody targetRigid,Animator animator, Vector3 knockDir)
        {
            int dealtDamage = UnityEngine.Random.value < 0.2f ? criticalDamage : damage;
            health -= dealtDamage;
            impact.Play();
            animator.SetTrigger("Damaged");
            UpdateUI();

           
            targetRigid.linearVelocity = knockDir * knockbackForce;
            Invoke(nameof(StopKnockback), 0.4f);
        }

        private void StopKnockback()
        {
            rigid1P.linearVelocity = Vector3.zero;
            rigid2P.linearVelocity = Vector3.zero;
        }

        private void UpdateUI()
        {
            healthBar1P.localScale = new Vector3(health1P / 100f, 1, 1);
            healthBar2P.localScale = new Vector3(health2P / 100f, 1, 1);
        }
    }
}
