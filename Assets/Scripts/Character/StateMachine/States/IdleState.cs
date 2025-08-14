using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerStateBase
{
    public IdleState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) { }

    public override void OnStateEnter()
    {
        _playerStateManager.animator.SetBool("Run", false);
        _playerStateManager.stamina.AbleRegen();
        // Code to execute when entering the idle state
        Debug.Log("Entering Idle State");
    }

    public override void OnStateExit()
    {
        // Code to execute when exiting the idle state
        Debug.Log("Exiting Idle State");
    }

    public override void OnStateUpdate()
    {
        CheckSwitchState();
        // Code to execute during the idle state update
        Debug.Log("Updating Idle State");
    }

    public override void CheckSwitchState()
    {
        if (_playerStateManager.WalkButtonPressed)
        {
            SwitchState(_playerStateFactory.WalkingState());
        }
        else if (_playerStateManager.AttackButtonPressed && _playerStateManager.CanAttack && (_currentSuperState is InGroundState))
        {
            SwitchState(_playerStateFactory.AttackingState());
        } else if (_playerStateManager.DashButtonPressed && (_currentSuperState is InGroundState))
        {
            SwitchState(_playerStateFactory.DashingState());
        }
    }

    public override void InitializeSubState()
    {
        // Initialize any sub-states if needed
    }
}

