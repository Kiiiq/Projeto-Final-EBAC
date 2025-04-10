using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

public class FSMExample : MonoBehaviour
{
    public enum states
    {
        stateOne,
        stateTwo,
        stateThree,
        stateFour
    }

    public StateMachine<states> stateMachine = new StateMachine<states>();

    private void Start()
    {
        stateMachine.RegisterState(states.stateOne, new StateBase());
        stateMachine.RegisterState(states.stateTwo, new StateBase());
        stateMachine.RegisterState(states.stateThree, new StateBase());
        stateMachine.RegisterState(states.stateFour, new StateBase());
        stateMachine.SwitchState(states.stateOne);
    }
}
