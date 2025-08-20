using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] PlayerStateManager playerStateManager; // Reference to the PlayerStateManager script
    [SerializeField] public List<GameObject> enemies; // List to hold enemy GameObjects
    private int index = 0; // Index to track the current enemy


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Add the enemy GameObject to the list if it has the "Enemy" tag
            enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject == enemies[index])
            {
                enemies.Remove(other.gameObject);
                if (enemies.Count > 0)
                {
                    index = Mathf.Clamp(index, 0, enemies.Count - 1); // Ensure index is within bounds
                    playerStateManager.combatCamera.LookAt = enemies[index].transform; // Update camera focus to the next enemy
                }
                else
                {
                    StopCombatFocus(); // Stop combat focus if no enemies are left
                }
            }
            else
            {
                enemies.Remove(other.gameObject); // Remove the enemy from the list
            }

        }
    }

    public void StartCombatFocus()
    {
        if (enemies.Count==0)
        {
            StopCombatFocus();
            return;
        }

        playerStateManager.focused = true;
        playerStateManager.combatCamera.Priority = 11;
        playerStateManager.explorationCamera.Priority = 9;

        index = 0; // Reset index to the first enemy
        playerStateManager.combatCamera.LookAt = enemies[index].transform; // Set the camera to focus on the first enemy
    }

    public void StopCombatFocus()
    {
        playerStateManager.focused = false;
        playerStateManager.combatCamera.Priority = 9;
        playerStateManager.explorationCamera.Priority = 11;
    }

    public void NextEnemy()
    {
        index = (index + 1) % enemies.Count; // Increment index and wrap around if necessary
        playerStateManager.combatCamera.LookAt = enemies[index].transform; // Update camera focus to the next enemy
    }

    public void PreviousEnemy()
    {
        index = (index - 1 + enemies.Count) % enemies.Count; // Decrement index and wrap around if necessary
        playerStateManager.combatCamera.LookAt = enemies[index].transform; // Update camera focus to the previous enemy
    }



}
