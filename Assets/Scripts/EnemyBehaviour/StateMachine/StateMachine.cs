using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EnemyBehaviour
{
    public class StateMachine
    {
        public State currentState { get; private set; }

        public void Initialize(State startingState)
        {
            currentState = startingState;
            startingState.Enter();
        }

        public void ChangeState(State newState)
        {
            Debug.Log("EXITING " + currentState + " STATE");
            currentState.Exit();

            currentState = newState;
            newState.Enter();
        }

      


    }
}
