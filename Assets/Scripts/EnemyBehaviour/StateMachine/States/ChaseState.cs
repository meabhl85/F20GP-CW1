using System.Collections;
using UnityEngine;

namespace Assets.Scripts.EnemyBehaviour
{
    public class ChaseState : State
    {
        [SerializeField]
        float _idleDuration = 3f;

        float _totalDuration;

        public ChaseState(Enemy enemy, StateMachine stateMachine, Renderer rend, Transform player) : base(enemy, stateMachine, rend, player)
        {
        }

        public override bool Enter()
        {
            EnteredState = base.Enter();

            if (EnteredState)
            {
                Debug.Log("ENTERED CHASE STATE");
                _rend.material.SetColor("_Color", Color.yellow);
                _totalDuration = 0f;
            }

            return EnteredState;
        }

        public override void UpdateState()
        {
            //Make sure we have successfully enter the state
            if (EnteredState)
            {
                //Player detection
                _enemy.playerInSight = _enemy.PlayerSeen(_enemy._navMeshAgent.transform, _player, _enemy.maxAngle, _enemy.maxRadius);
                _enemy.playerInDetectionRadius = Vector3.Distance(_enemy._navMeshAgent.transform.position, _player.position) <= _enemy._detectionRadius;
                bool attackDetection = Vector3.Distance(_enemy._navMeshAgent.transform.position, _player.transform.position) <= _enemy._attackRadius;

                if (_enemy.playerInSight || _enemy.playerInDetectionRadius)
                {
                    SetDestination(_player.position);

                    if (attackDetection)
                    {
                        _stateMachine.ChangeState(_enemy.attack);
                    }
                }
                else
                {
                    _stateMachine.ChangeState(_enemy.idle);
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