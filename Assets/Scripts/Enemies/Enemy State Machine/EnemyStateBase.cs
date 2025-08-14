using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase
{
    protected EnemyStateManagerBase stateManager;
    protected EnemyStateFactoryBase stateFactory;
    
    protected bool _isRootState = false;
    
    protected EnemyStateBase _currentSubState;
    protected EnemyStateBase _currentSuperState;

    public EnemyStateBase(EnemyStateManagerBase enemyStateManager, EnemyStateFactoryBase enemyStateFactory)
    {
        stateManager = enemyStateManager;
        stateFactory = enemyStateFactory;
    }
    
    public virtual void OnStateEnter() { }
    
    public virtual void OnStateExit() { }
    
    public virtual void OnStateUpdate() { }
    
    public virtual void CheckSwitchState() { }
    
    public virtual void InitializeSubState() { }

    protected void SwitchState(EnemyStateBase newState)
    {
        ExitStates();

        newState.OnStateEnter();

        if (!_isRootState)
        {
            _currentSuperState.SetSubState(newState);
        }
        else
        {
            stateManager.SetRootState(newState);
        }
    }
    
    protected void SetSubState(EnemyStateBase newSubState)
    {
        _currentSubState = newSubState;
        _currentSubState.SetSuperState(this);
        _currentSubState.OnStateEnter();
    }

    protected void ExitStates()
    {
        OnStateExit();
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
    }

    protected void SetSuperState(EnemyStateBase superState)
    {
        _currentSuperState = superState;
    }

    public void UpdateStates()
    {
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
            _currentSubState.OnStateUpdate();
        }
    }


}
