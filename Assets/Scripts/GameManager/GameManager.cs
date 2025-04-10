
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReusableScripts;
using StateMachines;

public class GameManager : Singleton<GameManager>
{
    public enum gameStates
    {
        MainMenu,
        GamePlay,
        Pause,
        GameOver
    }

    public StateMachine<gameStates> stateMachine = new StateMachine<gameStates>();

    private void Start()
    {
        init();
    }

    private void init()
    {
        stateMachine.RegisterState(gameStates.MainMenu, new StateBase());
        stateMachine.RegisterState(gameStates.GamePlay, new StateBase());
        stateMachine.RegisterState(gameStates.Pause, new StateBase());
        stateMachine.RegisterState(gameStates.GameOver, new StateBase());
    }
}
