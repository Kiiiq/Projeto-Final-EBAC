using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingState : PlayerStateBase
{

    public WalkingState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory) 
    : base(playerStateManager, playerStateFactory){}

    public override void OnStateEnter()
    {
        // Code to execute when entering the walking state
        _playerStateManager.animator.SetBool("Run",true);
        InitializeSubState();
    }
    public override void OnStateExit()
    {
        _playerStateManager.animator.SetBool("Run", false);

    }
    
    public override void OnStateUpdate()
    {
        
        
        Vector3 forward = _playerStateManager.playerCamera.TransformDirection(Vector3.forward);
        Vector3 right = _playerStateManager.playerCamera.TransformDirection(Vector3.right);

        Vector3 ForwardRelative = new Vector3(forward.x, 0, forward.z) * _playerStateManager.moveInput.y;
        Vector3 RightRelative = new Vector3(right.x, 0, right.z) * _playerStateManager.moveInput.x;


        Vector3 moveDirection = ForwardRelative + RightRelative;
        if (moveDirection != Vector3.zero)
        {
            _playerStateManager.LastMoveDir = moveDirection;


            _playerStateManager.characterController.Move(_playerStateManager.Speed * Time.deltaTime * moveDirection);
            if (_playerStateManager.focused)
            {
                _playerStateManager.playerSprite.transform.forward = new Vector3(_playerStateManager.playerCamera.forward.x, 0, _playerStateManager.playerCamera.forward.z); // Keep the player facing the camera direction in combat focus
            }
            else
            {
                _playerStateManager.playerSprite.transform.forward = moveDirection;
            }
        }



        CheckSwitchState();
        UpdateStates();

    }
    public override void CheckSwitchState()
    {
        if (!_playerStateManager.WalkButtonPressed)
        {
            SwitchState(_playerStateFactory.IdleState());
        }
        else if (_playerStateManager.AttackButtonPressed && _playerStateManager.CanAttack && (_currentSuperState is InGroundState))
        {
            SwitchState(_playerStateFactory.AttackingState());
        }
        else if (_playerStateManager.DashButtonPressed && (_currentSuperState is InGroundState))
        {
            SwitchState(_playerStateFactory.DashingState());
        }
    }
    public override void InitializeSubState()
    {
        if (_currentSubState == null)
        {
            SetSubState(_playerStateFactory.WalkingNullState());
        }
    }
}


