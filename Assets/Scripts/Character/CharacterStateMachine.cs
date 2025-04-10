using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachines;

public class CharacterStateMachine : MonoBehaviour
{
    public enum characterStates
    {
        Walking,
        Jumping,
        idle
    }
    
    public float speed = 5f;

    public StateMachine<characterStates> stateMachine = new StateMachine<characterStates>();

    private void Start()
    {
        stateMachine.RegisterState(characterStates.Walking, new WalkingState());
        stateMachine.RegisterState(characterStates.Jumping, new JumpingState());
        stateMachine.RegisterState(characterStates.idle, new IdleState());
        stateMachine.SwitchState(characterStates.idle);
    }

    private void Update()
    {
        stateMachine.Update();
        if (Input.GetKeyDown(KeyCode.W))
        {
            stateMachine.SwitchState(characterStates.Walking);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.SwitchState(characterStates.Jumping);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            stateMachine.SwitchState(characterStates.idle);
        }
    }

}
