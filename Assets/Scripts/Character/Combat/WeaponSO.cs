using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponSO : ScriptableObject { 
    public GameObject weaponPrefab;
    public GameObject weapon;
    public Collider hitbox;

    public float damage;
    public float cooldown;
    public float attackDuration;
    public float staminaCost;

    public string animationName;

}
    
