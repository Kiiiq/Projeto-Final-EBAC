using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAliveState : EnemyStateBase
{
    public EnemyAliveState(EnemyStateManagerBase enemyStateManager, EnemyStateFactoryBase enemyStateFactory)
        : base(enemyStateManager, enemyStateFactory)
    {
        _isRootState = true;
    }

    public override void OnStateEnter() {
        stateManager.animator.SetBool("isDead", false);
        Debug.Log("Enemy Alive State");
    }
    public override void OnStateExit() { }

    public override void OnStateUpdate() { 
        CheckSwitchState();
    }

    public override void CheckSwitchState() { 
        if (stateManager.IsDead)
        {
            SwitchState(stateFactory.DeadState());
        }
    }

    public override void InitializeSubState() { }
}
