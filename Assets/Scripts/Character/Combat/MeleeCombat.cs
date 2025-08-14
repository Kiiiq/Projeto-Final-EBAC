using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeCombat : MonoBehaviour
{
    public InputActionReference AttackAction;

    [Header("Sword Attack")]
    public InputActionReference switchToSword;
    public float swordCooldownTime;
    public float swordStaminaCost=23f;
    public float swordDamage;
    public float swordAttackDuration;

    [Header("Dagger Attack")]
    public InputActionReference switchToDagger;
    public float daggerCooldownTime;
    public float daggerStaminaCost = 12f;
    public float daggerDamage;
    public float daggerAttackDuration;

    [Header("Defense")]
    public InputActionReference DefenseAction;
    public float defenseStaminaCost = 10f;
    public float damageReduction = 0.5f;


    private float actualStaminaCost;
    private bool sword=true;
    private bool Attacking;
    private bool Defending;
    
    public bool CanAttack = true;

    [Header("References")]
    [SerializeField]Animator animator;
    [SerializeField] GroundChecker groundChecker;
    [SerializeField] StaminaScript stamina;
    [SerializeField] GameObject swordPrefab;
    [SerializeField] GameObject daggerPrefab;
    [SerializeField] private Collider weaponHitBox;




    private void Start()
    {
        actualStaminaCost = swordStaminaCost;
        swordPrefab.SetActive(true);
        daggerPrefab.SetActive(false);
        weaponHitBox.enabled = false;
        sword = true;
        animator.SetBool("Sword", true);
    }


    // Update is called once per frame
    void Update()
    {
        if (CanAttack) { 
            if (AttackAction.action.triggered && groundChecker.IsGrounded() && stamina.UseStamina(actualStaminaCost))
            {
                if(sword ) StartCoroutine(swordAttack()); else StartCoroutine(daggerAttack());

            }
        }

        if (groundChecker.IsGrounded()) { 
            defense();
        }

        if (switchToSword.action.triggered)
        {
            actualStaminaCost = swordStaminaCost;
            sword = true;
            animator.SetBool("Sword", true);
            daggerPrefab.SetActive(false);
            swordPrefab.SetActive(true);
            weaponHitBox= swordPrefab.GetComponent<Collider>();
        }

        if (switchToDagger.action.triggered)
        {
            actualStaminaCost = daggerStaminaCost;
            sword = false;
            animator.SetBool("Sword", false);
            daggerPrefab.SetActive(true);
            swordPrefab.SetActive(false);
            weaponHitBox = daggerPrefab.GetComponent<Collider>();
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

    IEnumerator swordAttack()
    {
        
        Attacking = true;
        CanAttack = false;
        animator.SetBool("Attack",true);
        weaponHitBox.enabled = true;

        yield return new WaitForSeconds(swordAttackDuration);
        animator.SetBool("Attack", false);
        weaponHitBox.enabled = false;
        Attacking = false;


        yield return new WaitForSeconds(swordCooldownTime);        
        CanAttack = true;
    }

    IEnumerator daggerAttack()
    {
        Attacking = true;
        CanAttack = false;
        animator.SetBool("Attack", true);
        weaponHitBox.enabled = true;
        
        yield return new WaitForSeconds(daggerAttackDuration);
        animator.SetBool("Attack", false);
        weaponHitBox.enabled = false;
        Attacking = false;

        yield return new WaitForSeconds(daggerCooldownTime);
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
