using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerStateBase
{

    public JumpingState(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    : base(playerStateManager, playerStateFactory) { }
    public override void OnStateEnter()
    {
        _playerStateManager.VSpeed = _playerStateManager.jumpForce;
        _playerStateManager.animator.SetBool("Jump", true);
        _isRootState = true;
        InitializeSubState();
        _playerStateManager.corroutineHandler.StartCoroutine(_playerStateManager.corroutineHandler.JumpDuration());
    }
    public override void OnStateExit()
    {
        _playerStateManager.IsJumping = false;
        _playerStateManager.animator.SetBool("Jump", false);
        _playerStateManager.JumpButtonPressed = false;

    }
    public override void OnStateUpdate()
    {
        CheckSwitchState();
        _playerStateManager.characterController.Move(new Vector3(0, _playerStateManager.VSpeed, 0)*Time.deltaTime);
    }
    public override void CheckSwitchState()
    {
        if (!_playerStateManager.IsJumping || !_playerStateManager.JumpButtonPressed)
        {
            SwitchState(_playerStateFactory.NotInGroundState());
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
