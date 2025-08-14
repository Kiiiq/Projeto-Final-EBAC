using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendingState : PlayerStateBase
{
    public DefendingState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) { }

    public override void OnStateEnter()
    {
        _playerStateManager.animator.SetBool("Defend", true);
        _playerStateManager.Defending = true;
        _playerStateManager.stamina.UNAbleRegen();
        _playerStateManager.Speed = _playerStateManager.DefaultSpeed * _playerStateManager.DefenseSlow; // Reduce speed while defending
        Debug.Log("Entering Defending State");
    }
    public override void OnStateExit()
    {
        _playerStateManager.animator.SetBool("Defend", false);
        _playerStateManager.Defending = false;
        _playerStateManager.stamina.InstaAbleRegen();
        _playerStateManager.Speed = _playerStateManager.DefaultSpeed;
        Debug.Log("Exiting Defending State");
    }
    public override void OnStateUpdate()
    {
        CheckSwitchState();
        // Code to execute during the defending state update
        Debug.Log("Updating Defending State");
    }
    public override void CheckSwitchState()
    {
        if (!_playerStateManager.DefenseButtonPressed)
        {
            SwitchState(_playerStateFactory.WalkingNullState());
        }
    }
    public override void InitializeSubState()
    {
        
    }
}

