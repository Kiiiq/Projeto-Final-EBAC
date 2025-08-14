using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyStateBase
{
    public EnemyDeadState(EnemyStateManagerBase stateManager, EnemyStateFactoryBase stateFactory)
        : base(stateManager, stateFactory)
    {
        _isRootState = true;
    }

    public override void OnStateEnter() {
        Debug.Log("Enemy Dead State");
        stateManager.animator.SetBool("isDead", true);
        stateManager.corroutineHandler.StartCoroutine(stateManager.corroutineHandler.Die(3f));
    }
    public override void OnStateExit() { }

    public override void OnStateUpdate() { }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }
}
