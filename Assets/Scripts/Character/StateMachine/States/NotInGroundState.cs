using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotInGroundState : PlayerStateBase
{
    public NotInGroundState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) { }


    public override void OnStateEnter()
    {
        _playerStateManager.animator.Play("JumpAir_Normal_InPlace_SwordAndShield");
        InitializeSubState();
        _isRootState = true;
        Debug.Log("Entering Not In Ground State");
    }
    public override void OnStateExit()
    {
        _playerStateManager.animator.SetBool("Falling", false);
        
        // Code to execute when exiting the not in ground state
        Debug.Log("Exiting Not In Ground State");
    }
    public override void OnStateUpdate()
    {
        CheckSwitchState();
        UpdateStates();
        _playerStateManager.VSpeed -= _playerStateManager.gravityForce * Time.deltaTime;
        _playerStateManager.characterController.Move(new Vector3(0, _playerStateManager.VSpeed * Time.deltaTime, 0));
        
        // Code to execute during the not in ground state update
        Debug.Log("Updating Not In Ground State");
    }
    public override void CheckSwitchState()
    {
        if (_playerStateManager.groundCheck.IsGrounded())
        {
            SwitchState(_playerStateFactory.InGroungState());
        }
    }
    public override void InitializeSubState()
    {
        if (_currentSubState == null)
        {
            SetSubState(_playerStateFactory.IdleState());
        }
    }
}

