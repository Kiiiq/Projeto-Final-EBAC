using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManagerBase : MonoBehaviour
{
    EnemyStateFactoryBase _enemyStateFactory;
    [SerializeField] EnemyStateBase _currentState;
    [SerializeField] public EnemyHealthManager _EnemyHealthManager;
    [SerializeField] public EnemyCorroutineHandler corroutineHandler;
    [SerializeField] public Animator animator;

    float maxHealth = 100f;

    bool isDead = false;


    #region Setters/Getters
    public void SetRootState(EnemyStateBase newState)
   {
        _currentState = newState;
        _currentState.OnStateEnter();
   }
    public bool IsDead { get => isDead; set => isDead = value; }
    #endregion

    private void Awake()
    {
        _enemyStateFactory = new EnemyStateFactoryBase(this);

        _currentState = _enemyStateFactory.AliveState();
        _currentState.OnStateEnter();
    }
    private void Update()
    {
        _currentState.OnStateUpdate();
        
    }


}
