using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FSM.States
{
    [CreateAssetMenu(fileName = "IdleState", menuName="CW-Draft/States/Idle", order = 1)]
    public class IdleState : AbstractFSMState
    {
        [SerializeField]
        float _idleDuration = 3f;

        float _totalDuration;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.IDLE;
        }

        public override bool EnterState()
        {
            EnteredState =  base.EnterState();

            if(EnteredState)
            {
                Debug.Log("ENTERED IDLE STATE");
                _totalDuration = 0f;
            }
            
            //EnteredState = true;
            return EnteredState;
        }


        public override void UpdateState()
        {
            if (EnteredState)
            { 
                _totalDuration += Time.deltaTime;
                //Debug.Log("UPDATING IDLE STATE : "+ _totalDuration +" seconds");

                if(_totalDuration >= _idleDuration)
                {
                    _fsm.EnterState(FSMStateType.PATROL);
                } 
                else
                {
                    if (Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position) <= 10f)
                    {
                        _fsm.EnterState(FSMStateType.CHASE);
                    }
                }
            }
        }


        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING IDLE STATE");
            return true;
        }
    }

}
