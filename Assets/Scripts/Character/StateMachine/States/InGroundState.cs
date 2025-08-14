using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGroundState : PlayerStateBase
{
    public InGroundState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) { }

    
     
    public override void OnStateEnter()
    {
        // Code to execute when entering the in ground state
        _isRootState = true;
        InitializeSubState();
        _playerStateManager.VSpeed = -_playerStateManager.gravityForce; // Reset vertical speed when entering the ground state
        _playerStateManager.animator.SetBool("Land", true);
        Debug.Log("Entering In Ground State");
    }
    public override void OnStateExit()
    {
        _playerStateManager.animator.SetBool("Land", false);
        // Code to execute when exiting the in ground state
        Debug.Log("Exiting In Ground State");
    }
    public override void OnStateUpdate()
    {
        CheckSwitchState();
        UpdateStates();
        _playerStateManager.characterController.Move(new Vector3(0, _playerStateManager.VSpeed * Time.deltaTime, 0));
        
    }
    public override void CheckSwitchState()
    {   
        if (_playerStateManager.JumpButtonPressed)
        {
            SwitchState(_playerStateFactory.JumpingState());
        }
        else if (!_playerStateManager.groundCheck.IsGrounded())
        {
            SwitchState(_playerStateFactory.NotInGroundState());
        }
    }
    public override void InitializeSubState()
    {
        if (_currentSubState==null)
        {
            SetSubState(_playerStateFactory.IdleState());
        }
    }
}
