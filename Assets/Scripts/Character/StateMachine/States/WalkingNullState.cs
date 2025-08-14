using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingNullState : PlayerStateBase
{
    public WalkingNullState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
   : base(playerStateManager, playerStateFactory) { }

    
    public override void OnStateEnter()
    {
        Debug.Log("Entering Walking Null State");
        
        
        
    }
    public override void OnStateExit()
    {
        
    }
    public override void OnStateUpdate()
    {
        CheckSwitchState();
    }
    public override void CheckSwitchState()
    {
        if (_playerStateManager.DefenseButtonPressed)
        {
            SwitchState(_playerStateFactory.DefendingState());
        }
        else if (_playerStateManager.SprintButtonPressed && !_playerStateManager.stamina.tired)
        {
            SwitchState(_playerStateFactory.RunningState());
        }
    }
    public override void InitializeSubState()
    {
        
    }
}
