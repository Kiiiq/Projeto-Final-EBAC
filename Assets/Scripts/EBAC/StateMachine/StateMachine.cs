using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StateMachines
{
    public class StateMachine<T> where T : System.Enum
    {
        [SerializeField]
        public Dictionary<T, StateBase> stateDictionary = new Dictionary<T, StateBase>();

        private StateBase currentState;


        public StateBase getCurrentState()
        {
            return currentState;
        }

        public void RegisterState(T enumType, StateBase state)
        {
            stateDictionary.Add(enumType, state);
        }

        public void SwitchState(T state)
        {
            if (currentState != null)
            {
                currentState.OnStateExit();
            }

            currentState = stateDictionary[state];

            currentState.OnStateEnter();
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.OnStateStay();
            }
        }
    }
}