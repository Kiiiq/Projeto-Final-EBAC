using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    [SerializeField]GameObject objectToSpawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectToSpawn != null)
            {
                objectToSpawn.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Object to spawn is not assigned.");
            }
        }
    }
}
