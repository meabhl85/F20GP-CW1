using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code.FSM
{
    public class FiniteStateMachine : MonoBehaviour
    {

        AbstractFSMState _currentState;

        [SerializeField]
        List<AbstractFSMState> _validStates;
        Dictionary<FSMStateType, AbstractFSMState> _fsmStates;

        public void Awake()
        {
            _currentState = null;

            _fsmStates = new Dictionary<FSMStateType, AbstractFSMState>();

            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            FsmEnemy fsmEnemy = GetComponent<FsmEnemy>();
            Transform player = GameObject.Find("Player").transform;

            foreach (AbstractFSMState state in _validStates)
            {
                state.SetExecutingFSM(this);
                state.SetExecutingFsmEnemy(fsmEnemy);
                state.SetNavMeshAgent(navMeshAgent);
                state.SetExecutingPlayer(player);
                _fsmStates.Add(state.StateType, state);
            }
        }

        public void Start()
        {
            EnterState(FSMStateType.IDLE);
        }

        public void Update()
        {
            if (_currentState != null)
            {
                _currentState.UpdateState();

            }
        }

        #region STATE MANAGEMENT
        public void EnterState(AbstractFSMState nextState)
        {
            if (nextState == null)
            {
                return;
            }
            if (_currentState != null)
            {
                _currentState.ExitState();
            }

            _currentState = nextState;
            _currentState.EnterState();
        }

        public void EnterState(FSMStateType stateType)
        {
            if (_fsmStates.ContainsKey(stateType))
            {
                //get value in the dictionary that corresponds to that key
                AbstractFSMState nextState = _fsmStates[stateType];

                //_currentState.ExitState();

                EnterState(nextState);

            }
        }
        #endregion


    }
}
