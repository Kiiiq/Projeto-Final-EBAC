using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField]public WeaponSO weaponData;
    [SerializeField]List<Collider> colliders = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !colliders.Contains(other))
        {
            colliders.Add(other);
            Debug.Log("Hit: " + other.name);
            other.GetComponent<HealthManager>().TakeDamage(weaponData.damage);
        }
    }

    public void ClearColliders()
    {
        colliders.Clear();
    }
}
