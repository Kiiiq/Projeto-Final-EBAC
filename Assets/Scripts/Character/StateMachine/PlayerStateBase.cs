using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateBase 
{
    protected PlayerStateManager _playerStateManager;
    protected PlayerStateFactory _playerStateFactory;

    protected PlayerStateBase _currentSubState;
    protected PlayerStateBase _currentSuperState;

    protected bool _isRootState = false;

    public PlayerStateBase(PlayerStateManager playerStateManager, PlayerStateFactory playerStateFactory)
    {
        _playerStateManager = playerStateManager;
        _playerStateFactory = playerStateFactory;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateExit();

    public abstract void OnStateUpdate();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubState();

    protected void SwitchState(PlayerStateBase newState) { 
        ExitStates();

        newState.OnStateEnter();

        if (_isRootState)
        {
            _playerStateManager.CurrentState = newState;
        }
        else
        {
            _currentSuperState.SetSubState(newState);
        }

    }

    public void ExitStates()
    {
        OnStateExit();
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
    }

    public void UpdateStates() { 
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
            _currentSubState.OnStateUpdate();
        }
    }
    protected void SetSubState(PlayerStateBase SubState){
        _currentSubState = SubState;
        _currentSubState.SetSuperState(this);
        _currentSubState.OnStateEnter();
    }

    protected void SetSuperState(PlayerStateBase SuperState) { 
        _currentSuperState = SuperState;
    }
}
