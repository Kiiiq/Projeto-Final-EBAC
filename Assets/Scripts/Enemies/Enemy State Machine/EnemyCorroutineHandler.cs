using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorroutineHandler : MonoBehaviour
{
    public EnemyStateManagerBase _enemyStateManagerBase;

    public EnemyCorroutineHandler(EnemyStateManagerBase enemyStateManagerBase)
    {
        _enemyStateManagerBase = enemyStateManagerBase;
    }

    public IEnumerator Die(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(this.gameObject);
    }
}
