using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeCombat : MonoBehaviour
{

    [Header("Basic Attack")]
    public InputActionReference AttackAction;   
    public float basicCooldownTime;
    public float basicStaminaCost=23f;
    public float basicDamage;
    public float basicAttackDuration;

    [Header("Defense")]
    public InputActionReference DefenseAction;
    public float defenseStaminaCost = 10f;
    public float damageReduction = 0.5f;
    



    private bool Attacking;
    private bool Defending;
    
    public bool CanAttack = true;

    [Header("References")]
    [SerializeField]Animator animator;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] StaminaScript stamina;
    [SerializeField] private Collider weaponHitBox;






    // Update is called once per frame
    void Update()
    {
        if (CanAttack) { 
            if (AttackAction.action.triggered && groundChecker.IsGrounded() && stamina.UseStamina(basicStaminaCost))
            {
                StartCoroutine(Attack());
            }
        }

        if (groundChecker.IsGrounded()) { 
            defense();
        }
    }

    private void defense()
    {
        DefenseAction.action.performed += ctx =>
        {
            if (stamina.UseStamina(defenseStaminaCost))
            {
                Defending = true;
                animator.SetBool("Defend", true);
            }
        };

        DefenseAction.action.canceled += ctx =>
        {
            Defending = false;
            animator.SetBool("Defend", false);
        };
    }

    IEnumerator Attack()
    {
        
        Attacking = true;
        CanAttack = false;
        animator.SetBool("Attack",true);
        weaponHitBox.enabled = true;

        yield return new WaitForSeconds(basicAttackDuration);
        animator.SetBool("Attack", false);
        weaponHitBox.enabled = false;


        yield return new WaitForSeconds(basicCooldownTime);
        Attacking = false;
        CanAttack = true;
    }

    public bool IsAttacking()
    {
        return Attacking;
    }

    public bool IsDefending()
    {
        return Defending;
    }
}
