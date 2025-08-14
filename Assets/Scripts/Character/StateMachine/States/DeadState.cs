using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : PlayerStateBase
{
    public DeadState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) { }


    public override void OnStateEnter()
    {
        _playerStateManager.animator.Play("Diying");
        _isRootState = true;
        Debug.Log("Entering Dead State");
    }
    public override void OnStateExit()
    {}
    public override void OnStateUpdate()
    {}

    public override void CheckSwitchState()
    {}
    public override void InitializeSubState()
    {}
}
