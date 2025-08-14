using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : PlayerStateBase
{

    public RunningState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) { }
    public override void OnStateEnter()
    {
        Debug.Log("Entering Running State");
        // Code to execute when entering the running state
        _playerStateManager.animator.SetBool("Sprint", true);
        _playerStateManager.Speed = _playerStateManager.DefaultSpeed * _playerStateManager.sprintMultiplier;
    }
    public override void OnStateExit()
    {
        _playerStateManager.animator.SetBool("Sprint", false);
        _playerStateManager.Speed = _playerStateManager.DefaultSpeed;
        Debug.Log("Exiting Running State");
        _playerStateManager.stamina.AbleRegen();
    }
    public override void OnStateUpdate()
    {
        
        _playerStateManager.stamina.UseStamina(_playerStateManager.sprintStaminaCost * Time.deltaTime);
        Debug.Log("Updating Running State");
        
        
        CheckSwitchState();
    }
    public override void CheckSwitchState()
    {
        if (_playerStateManager.stamina.currentStamina <= 0.3 || !_playerStateManager.SprintButtonPressed)
        {
            if (_playerStateManager.stamina.currentStamina <= 0.3)
            {
                _playerStateManager.stamina.tired = true;
            }
            SwitchState(_playerStateFactory.WalkingNullState());
        }
        else if (_playerStateManager.DefenseButtonPressed)
        {
            SwitchState(_playerStateFactory.DefendingState());
        }
    }
    public override void InitializeSubState()
    {
        // Initialize any sub-states if needed
    }
}

