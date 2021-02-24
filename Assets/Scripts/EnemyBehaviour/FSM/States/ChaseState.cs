using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    [CreateAssetMenu(fileName = "ChaseState", menuName = "CW-Draft/States/Chase", order = 3)]
    public class ChaseState : AbstractFSMState
    {
        [SerializeField]
        float _idleDuration = 3f;

        float _totalDuration;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.CHASE;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTERED CHASE STATE");
                _totalDuration = 0f;
            }

            return EnteredState;
        }


        public override void UpdateState()
        {
            //Make sure we have successfully enter the state
            if (EnteredState)
            {
                if (Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) <= 15f)
                {
                    SetDestination(_player.position);

                    if (Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) <= 5f)
                    {
                        _fsm.EnterState(FSMStateType.ATTACK);
                    }
                } else
                {
                    _fsm.EnterState(FSMStateType.IDLE);
                }

            }
        }

        private void SetDestination(Vector3 destination)
        {
            if (_navMeshAgent != null && destination != null)
            {
                _navMeshAgent.SetDestination(destination);
            }
        }

        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING CHASE STATE");
            return true;
        }
    }
}
