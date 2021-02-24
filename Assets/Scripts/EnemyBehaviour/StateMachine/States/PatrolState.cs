using System.Collections;
using UnityEngine;


namespace Assets.Scripts.EnemyBehaviour
{
    public class PatrolState : State
    {
        ConnectedWaypoint[] _patrolPoints;
        int _patrolPointIndex;

        public PatrolState(Enemy enemy, StateMachine stateMachine, Renderer rend, Transform player) : base(enemy, stateMachine, rend, player)
        {
            _patrolPointIndex = -1;
        }

        public override bool Enter()
        {
            EnteredState = false;

            if (base.Enter())
            {
                //Grab and store patrol points
                _patrolPoints = _enemy.PatrolPoints;

                if (_patrolPoints == null || _patrolPoints.Length == 0)
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
                    _rend.material.SetColor("_Color", Color.blue);

                }
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

                //Change to idle state when enemy reaches patrol point
                if (Vector3.Distance(_enemy._navMeshAgent.transform.position, _patrolPoints[_patrolPointIndex].transform.position) <= 3f)
                {
                    _stateMachine.ChangeState(_enemy.idle);
                }

                if (_enemy.playerInSight || _enemy.playerInDetectionRadius)
                {
                    _stateMachine.ChangeState(_enemy.chase);
                }

            }
        }

        private void SetDestination(ConnectedWaypoint destination)
        {
            if (_enemy._navMeshAgent != null && destination != null)
            {
                _enemy._navMeshAgent.SetDestination(destination.transform.position);
            }
        }
    }
}