using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFactoryBase : MonoBehaviour
{
    public EnemyStateManagerBase stateManager;

    public EnemyStateFactoryBase(EnemyStateManagerBase enemyStateManager)
    {
        stateManager = enemyStateManager;
    }

    public EnemyStateBase AliveState()
     {
        return new EnemyAliveState(stateManager, this);
     }

    public EnemyStateBase DeadState()
    {
        return new EnemyDeadState(stateManager, this);
    }
}
