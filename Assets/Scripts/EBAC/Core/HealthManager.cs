using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f;
    [SerializeField] protected float currentHealth;


    // Start is called before the first frame update
    protected abstract void Start();


    public abstract void TakeDamage(float damage);

    //currentHealth -= damage;
    //if (currentHealth <= 0)
    //{
    //    Die();
    //}


    protected abstract void Die();
    
}
