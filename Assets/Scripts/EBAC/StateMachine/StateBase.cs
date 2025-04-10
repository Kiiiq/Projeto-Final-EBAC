using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a base class for state machine states.
namespace StateMachines
{
    public class StateBase
    {
        public virtual void OnStateEnter()
        {
            Debug.Log("OnStateEnter");
        }

        public virtual void OnStateStay()
        {
            Debug.Log("OnStateStay");
        }

        public virtual void OnStateExit()
        {
            Debug.Log("OnStateExit");
        }
    }
}