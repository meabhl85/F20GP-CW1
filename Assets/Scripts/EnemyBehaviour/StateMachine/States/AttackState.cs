using System.Collections;
using UnityEngine;

namespace Assets.Scripts.EnemyBehaviour
{
    public class AttackState : State
    {
        [SerializeField]
        float _idleDuration = 2f;

        float _totalDuration;

        bool attacked = false;

        public AttackState(Enemy enemy, StateMachine stateMachine, Renderer rend, Transform player) : base(enemy, stateMachine, rend, player)
        {
        }

        public override bool Enter()
        {
            EnteredState = base.Enter();

            if (EnteredState)
            {
                Debug.Log("ENTERED ATTACK STATE");
                _rend.material.SetColor("_Color", Color.red);   
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
                    //Damage player
                    GameObject.Find("Player").GetComponent<PlayerHealth>().TakeDamage(10);
                    attacked = true;
                }

                if (_totalDuration >= _idleDuration)
                {
                    _stateMachine.ChangeState(_enemy.chase);
                }
            }
        }

        private void SetDestination(Vector3 destination)
        {
            if (_enemy._navMeshAgent != null && destination != null)
            {
                _enemy._navMeshAgent.SetDestination(destination);
            }
        }
    }
}