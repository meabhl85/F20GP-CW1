using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    [CreateAssetMenu(fileName = "AttackState", menuName = "CW-Draft/States/Attack", order = 4)]
    public class AttackState : AbstractFSMState
    {
        [SerializeField]
        float _idleDuration = 2f;

        float _totalDuration;

        bool attacked = false;

        public override void OnEnable()
        {
            base.OnEnable();
            StateType = FSMStateType.ATTACK;
        }

        public override bool EnterState()
        {
            EnteredState = base.EnterState();

            if (EnteredState)
            {
                Debug.Log("ENTERED ATTACK STATE");
                attacked = false;
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

                if (!attacked)
                {
                    //Do attack
                    Debug.Log("Attack");
                    GameObject.Find("Player").GetComponent<PlayerHealth>().TakeDamage(10);
                    attacked = true;
                }
                
                if (_totalDuration >= _idleDuration)
                {
                    _fsm.EnterState(FSMStateType.CHASE);
                }
            }
        }

        public override bool ExitState()
        {
            base.ExitState();
            Debug.Log("EXITING ATTACK STATE");
            return true;
        }


    }
}
