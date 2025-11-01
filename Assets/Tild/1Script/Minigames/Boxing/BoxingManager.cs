using System;
using DG.Tweening;
using Tild.Menu;
using TMPro;
using UnityEngine;

namespace Tild.Minigames.Boxing
{
    public class BoxingManager : MonoBehaviour
    {
        [Header("Systems")] [SerializeField] private BoxingInputSO boxingInputSO;
        [SerializeField] private Rigidbody rigid1P, rigid2P;
        [SerializeField] private Animator animator1P, animator2P;

        [Header("Values")] [Space] [SerializeField] private int damage = 10;
        [SerializeField] private int criticalDamage = 20;
        [SerializeField] private float backForce = 5f;
        [SerializeField] private float dashForce = 8f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float knockbackForce = 3f;
        [SerializeField] private int guardBreakThreshold = 3;
        [SerializeField] private float guardBreakStunDuration = 2f;
        [SerializeField] private float parryWindow = 0.2f;
        [SerializeField] private float parryStunDuration = 2f;

    
        [SerializeField] private float hitstunDuration = 0.3f;  
        [SerializeField] private int hitsToKnockback = 3;        
        [SerializeField] private float heavyKnockbackForce = 6f; 
        [SerializeField] private float comboResetTime = 0.3f;   

        private int health1P = 100, health2P = 100;
        private bool debounce1P, debounce2P;
        private bool isDefending1P, isDefending2P;
        private bool backDefend1P, backDefend2P;
        private int guardHits1P, guardHits2P;
        private float guardStartTime1P, guardStartTime2P;
        private float stunUntil1P, stunUntil2P;

       
        private int hitComboOn1P, hitComboOn2P;
        private float lastHitTimeOn1P, lastHitTimeOn2P;

        [Header("UI&Effects")] [Space] [SerializeField] private RectTransform healthBar1P, healthBar2P;
        [SerializeField] private ParticleSystem hitImpact1P, hitImpact2P;
        [SerializeField] private ParticleSystem stunImpact1P, stunImpact2P;
        [SerializeField] private ParticleSystem shieldImpact1P, shieldImpact2P;
        [SerializeField] private ParticleSystem guardBreakImpact1P, guardBreakImpact2P;
        [SerializeField] private TMP_Text infos;

        private void OnEnable()
        {
            boxingInputSO.AKeyPressed += Handle1PBack;
            boxingInputSO.DKeyPressed += Handle1PDashAttack;
            boxingInputSO.SKeyPressed += Handle1PGuardState;
            boxingInputSO.LeftKeyPressed += Handle2PDashAttack;
            boxingInputSO.RightKeyPressed += Handle2PBack;
            boxingInputSO.DownKeyPressed += Handle2PGuardState;
        }

        private void OnDisable()
        {
            boxingInputSO.AKeyPressed -= Handle1PBack;
            boxingInputSO.DKeyPressed -= Handle1PDashAttack;
            boxingInputSO.SKeyPressed -= Handle1PGuardState;
            boxingInputSO.LeftKeyPressed -= Handle2PDashAttack;
            boxingInputSO.RightKeyPressed -= Handle2PBack;
            boxingInputSO.DownKeyPressed -= Handle2PGuardState;
        }

        private void Handle1PGuardState(bool pressed)
        {
            if (Time.time < stunUntil1P) return; 
            isDefending1P = pressed;
            animator1P.SetBool("isGuarding", pressed);
            if (pressed)
            {
                guardStartTime1P = Time.time;
                guardHits1P = 0;
                shieldImpact1P.gameObject.SetActive(true);
            }
            else
            {
                shieldImpact1P.gameObject.SetActive(false);
            }
        }

        private void Handle2PGuardState(bool pressed)
        {
            if (Time.time < stunUntil2P) return;
            isDefending2P = pressed;
            animator2P.SetBool("isGuarding", pressed);
            
            if (pressed)
            {
                guardStartTime2P = Time.time;
                guardHits2P = 0;
                shieldImpact2P.gameObject.SetActive(true);
            }
            else
            {
                shieldImpact2P.gameObject.SetActive(false);
            }
        }

        private void Handle1PBack()
        {
            if (debounce1P || Time.time < stunUntil1P) return; 
            debounce1P = true;
            backDefend1P = true;
            rigid1P.linearVelocity = Vector3.right * backForce;
            animator1P.SetTrigger("BackDash");
            Invoke(nameof(Stop1PMove), 0.25f);
            Invoke(nameof(Reset1PDebounce), 0.3f);
        }

        private void Handle1PDashAttack()
        {
            if (debounce1P || Time.time < stunUntil1P) return; 
            debounce1P = true;
            animator1P.SetTrigger("Punch");
            rigid1P.linearVelocity = Vector3.left * dashForce;
            if (Vector3.Distance(rigid1P.position, rigid2P.position) < attackRange)
            {
                bool guardActive = isDefending2P || backDefend2P;
                bool parry = isDefending2P && Time.time - guardStartTime2P <= parryWindow;
                TakeDamage(ref health2P, hitImpact2P, rigid2P, animator2P, Vector3.right, guardActive, parry,
                    ref guardHits2P, ref stunUntil2P, rigid1P, animator1P, ref stunUntil1P, isTarget1P:false);
            }

            Invoke(nameof(Stop1PMove), 0.25f);
            Invoke(nameof(Reset1PDebounce), 0.4f);
        }

        private void Stop1PMove()
        {
            rigid1P.linearVelocity = Vector3.zero;
            backDefend1P = false;
        }

        private void Reset1PDebounce() => debounce1P = false;

        private void Handle2PBack()
        {
            if (debounce2P || Time.time < stunUntil2P) return;
            debounce2P = true;
            backDefend2P = true;
            rigid2P.linearVelocity = Vector3.left * backForce;
            animator2P.SetTrigger("BackDash");
            Invoke(nameof(Stop2PMove), 0.25f);
            Invoke(nameof(Reset2PDebounce), 0.3f);
        }

        private void Handle2PDashAttack()
        {
            if (debounce2P || Time.time < stunUntil2P) return; 
            debounce2P = true;
            animator2P.SetTrigger("Punch");
            rigid2P.linearVelocity = Vector3.right * dashForce;
            if (Vector3.Distance(rigid2P.position, rigid1P.position) < attackRange)
            {
                bool guardActive = isDefending1P || backDefend1P;
                bool parry = isDefending1P && Time.time - guardStartTime1P <= parryWindow;
                TakeDamage(ref health1P, hitImpact1P, rigid1P, animator1P, Vector3.left, guardActive, parry,
                    ref guardHits1P, ref stunUntil1P, rigid2P, animator2P, ref stunUntil2P, isTarget1P:true);
            }

            Invoke(nameof(Stop2PMove), 0.25f);
            Invoke(nameof(Reset2PDebounce), 0.4f);
        }

        private void Stop2PMove()
        {
            rigid2P.linearVelocity = Vector3.zero;
            backDefend2P = false;
        }

        private void Reset2PDebounce() => debounce2P = false;

        private void TakeDamage(
            ref int health,
            ParticleSystem impact,
            Rigidbody targetRigid,
            Animator targetAnimator,
            Vector3 knockDir,
            bool defenderGuardActive,
            bool parry,
            ref int guardHits,
            ref float defenderStunUntil,
            Rigidbody attackerRigid,
            Animator attackerAnimator,
            ref float attackerStunUntil,
            bool isTarget1P)
        {
            if (parry)
            {
                attackerStunUntil = Time.time + parryStunDuration;
                attackerRigid.linearVelocity = Vector3.zero;
                targetRigid.linearVelocity = Vector3.zero;
                
                ResetHitCombo(isTarget1P);
                SetText(isTarget1P,"PARRY 피해! 몇 초동안 경직 상태.");
                if (isTarget1P) stunImpact1P.Play();
                else stunImpact2P.Play();
                return;
            }

            if (defenderGuardActive)
            {
                guardHits++;
                targetAnimator.SetTrigger("GuardHit");
                if (guardHits >= guardBreakThreshold)
                {
                    defenderStunUntil = Time.time + guardBreakStunDuration;
                    if (targetAnimator == animator1P) { isDefending1P = false; animator1P.SetBool("isGuarding", false); }
                    if (targetAnimator == animator2P) { isDefending2P = false; animator2P.SetBool("isGuarding", false); }
                    // 연타 초기화
                    if (isTarget1P) guardBreakImpact1P.Play();
                    else guardBreakImpact1P.Play();
                    
                    ResetHitCombo(isTarget1P);
                    SetText(isTarget1P,"가드 브레이크! 몇 초동안 움직이지 못합니다.");
                }
                return;
            }

            int dealtDamage = UnityEngine.Random.value < 0.2f ? criticalDamage : damage;
            health -= dealtDamage;
            impact.Play();
            targetAnimator.SetTrigger("Damaged");
            UpdateUI();

   
            int combo = GetAndBumpHitCombo(isTarget1P);

      

            if (combo >= hitsToKnockback)
            {
             
                defenderStunUntil = Time.time; 
                targetRigid.linearVelocity = -knockDir * heavyKnockbackForce;
                ResetHitCombo(isTarget1P);
                SetText(isTarget1P,"3연타! 강한 넉백 발생, 반격 기회!");
            }
            else
            {
            
                defenderStunUntil = Time.time + hitstunDuration;
                targetRigid.linearVelocity = knockDir * knockbackForce;
            }

            Invoke(nameof(StopKnockback), 0.4f);
        }

        private void StopKnockback()
        {
            rigid1P.linearVelocity = Vector3.zero;
            rigid2P.linearVelocity = Vector3.zero;
        }

        private void UpdateUI()
        {
            healthBar1P.localScale = new Vector3(Mathf.Max(health1P / 100f, 0f), 1, 1);
            healthBar2P.localScale = new Vector3(Mathf.Max(health2P / 100f, 0f), 1, 1);

            if (health1P == 0 || health2P == 0)
            {
                MinigameManager.instance.Finish(health1P > health2P);
            }
        }

  
        private int GetAndBumpHitCombo(bool isTarget1P)
        {
            if (isTarget1P)
            {
         
                if (Time.time - lastHitTimeOn1P > comboResetTime) hitComboOn1P = 0;
                hitComboOn1P++;
                lastHitTimeOn1P = Time.time;
                return hitComboOn1P;
            }
            else
            {
                if (Time.time - lastHitTimeOn2P > comboResetTime) hitComboOn2P = 0;
                hitComboOn2P++;
                lastHitTimeOn2P = Time.time;
                return hitComboOn2P;
            }
        }

        private void SetText(bool isTarget1P, string text)
        {
            
            infos.color = !isTarget1P ? Color.red : Color.blue;
            infos.text = (!isTarget1P ? "1P의 " : "2P의 " )+ text;
            infos.transform.DOPunchPosition(Vector3.one,0.5f,10,1);
        }
        private void ResetHitCombo(bool isTarget1P)
        {
            if (isTarget1P) { hitComboOn1P = 0; lastHitTimeOn1P = 0f; }
            else { hitComboOn2P = 0; lastHitTimeOn2P = 0f; }
        }
    }
}
