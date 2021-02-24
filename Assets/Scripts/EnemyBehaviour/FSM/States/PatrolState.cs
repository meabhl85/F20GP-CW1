using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    [CreateAssetMenu(fileName = "PatrolState", menuName = "CW-Draft/States/Patrol", order = 2)]
    public class PatrolState : AbstractFSMState
    {
        ConnectedWaypoint[] _patrolPoints;
        int _patrolPointIndex;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.PATROL;
            _patrolPointIndex = -1;
        }

        public override bool EnterState()
        {
            EnteredState = false;

            if (base.EnterState())
            {
                //grab and store patrol points
                _patrolPoints = _fsmEnemy.PatrolPoints;

                if(_patrolPoints == null || _patrolPoints.Length == 0)
                {
                    Debug.Log("PatrolState: failed to grap patrol points from the npc");
                    
                }
                else
                {
                    if (_patrolPointIndex < 0)
                    {
                        _patrolPointIndex = UnityEngine.Random.Range(0, _patrolPoints.Length);
                    }
                    else
                    {
                        //stops going off the edge of the array
                        _patrolPointIndex = (_patrolPointIndex + 1) % _patrolPoints.Length;
                    }

                    SetDestination(_patrolPoints[_patrolPointIndex]);
                    EnteredState = true;
                    Debug.Log("ENTERED PATROL STATE");

                }
            }

            return EnteredState;
            
        }

        private void SetDestination(ConnectedWaypoint destination)
        {
            if (_navMeshAgent != null && destination != null)
            {
                _navMeshAgent.SetDestination(destination.transform.position);
            }
        }

        public override void UpdateState()
        {
            //Make sure we have successfully enter the state
            if (EnteredState)
            {
                //logic
                //compare navmesh to partol points
                //Debug.Log(Vector3.Distance(_navMeshAgent.transform.position, _patrolPoints[_patrolPointIndex].transform.position));
                
                if(Vector3.Distance(_navMeshAgent.transform.position, _patrolPoints[_patrolPointIndex].transform.position) <= 3f)
                {
                    _fsm.EnterState(FSMStateType.IDLE);
                }

                if (Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) <= 10f)
                //if (PlayerSeen())
                {
            
                    _fsm.EnterState(FSMStateType.CHASE);
                }

            }
        }

        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING PATROL STATE");
            return true;
        }
    }
}
