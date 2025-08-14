using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : HealthManager
{
    [SerializeField] EnemyStateManagerBase stateManager;
    public EnemyHealthManager(EnemyStateManagerBase enemyManager,float maxHealth)
    {
        stateManager = enemyManager;
        this.maxHealth = maxHealth;
    }
    protected override void Start()
    {
        currentHealth = maxHealth;
    }
    
    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    protected override void Die()
    {
        stateManager.IsDead = true;
    }

}
