using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharachterHealthManager : HealthManager
{
    [SerializeField] PlayerStateManager playerStateManager; // Reference to the PlayerStateManager

    [SerializeField] Image healthBar; // Reference to the health bar UI element
    [SerializeField] Image subHealthBar; // Reference to the sub health bar UI element
    public float timeDiference; // Time difference for UI updates


    protected override void Start()
    {
        
        currentHealth = maxHealth; // Initialize current health to max health
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;

        StartCoroutine(updateUI());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health does not exceed max health
        StartCoroutine(HealUIUpdate());
    }

    private IEnumerator updateUI() {
        healthBar.DOFillAmount(currentHealth / maxHealth, 0.1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timeDiference);
        subHealthBar.DOFillAmount(currentHealth / maxHealth, 0.1f).SetEase(Ease.Linear);
    }

    private IEnumerator HealUIUpdate() {
        subHealthBar.DOFillAmount(currentHealth / maxHealth, 0.1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timeDiference);
        healthBar.DOFillAmount(currentHealth / maxHealth, 0.1f).SetEase(Ease.Linear);
        
    }

    protected override void Die()
    {
        Debug.Log("Character has died.");
        playerStateManager.Die(); // Call the Die method in PlayerStateManager
        
    }

}
