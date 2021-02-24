using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.EnemyBehaviour
{
    public abstract class State
    {
        protected Enemy _enemy;
        protected StateMachine _stateMachine;
        protected NavMeshAgent _navMeshAgent;
        protected Renderer _rend;
        protected Transform _player;

        public bool EnteredState { get; protected set; }

        protected State(Enemy enemy, StateMachine stateMachine, Renderer rend, Transform player)
        {
            this._enemy = enemy;
            this._stateMachine = stateMachine;
            this._rend = rend;
            this._player = player;

        }

        public virtual bool Enter()
        {
            return true;
        }

        public virtual void UpdateState()
        {

        }

        public virtual bool Exit()
        {
            return true;
        }

       
    }
}
