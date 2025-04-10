using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

public class GMStateGameplay : StateBase
{
    public override void OnStateEnter()
    {
        Debug.Log("Entering Gameplay State");
        // Initialize menu UI
        // Show main menu
    }

    public override void OnStateStay()
    {
        Debug.Log("In Gameplay State");
        // Handle menu interactions
        // Check for user input to navigate menus
    }

    public override void OnStateExit()
    {
        Debug.Log("Exiting Gameplay State");
        // Clean up menu UI
        // Save any necessary data
    }
}
