using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : PlayerStateBase
{
    public DashingState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) { }

    public override void OnStateEnter()
    {
        if (!_playerStateManager.IsDashing)
        {
        _playerStateManager.animator.SetBool("Dash", true);
        _playerStateManager.corroutineHandler.StartCoroutine(_playerStateManager.corroutineHandler.Dash(this));
        _playerStateManager.DashButtonPressed = false;
            
        }
    }
    public override void OnStateExit()
    {
        Debug.Log("Exiting Dashing State");
        _playerStateManager.animator.SetBool("Dash", false);
        _playerStateManager.IsDashing = false;
        _playerStateManager.stamina.AbleRegen();
    }
    public override void OnStateUpdate()
    {
        Debug.Log("Updating Rolling State");
    }
    public override void CheckSwitchState()
    {
        // Logic to check if we should switch to another state
    }
    public override void InitializeSubState()
    {
        // Initialize any sub-states if needed
    }

    public void SwitchToIdle()
    {
        SwitchState(_playerStateFactory.IdleState());
        Debug.Log("Switching to Idle State from Dashing State");
    }
}


