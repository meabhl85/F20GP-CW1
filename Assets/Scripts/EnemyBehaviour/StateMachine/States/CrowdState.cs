using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.EnemyBehaviour
{
    public class Crowd : State
    {
        [SerializeField]
        float _idleDuration = 2f;

        float _totalDuration;

        //Variables for crowd behaviour
        float desiredSeparation = 10f;
        float neighborDist = 5f;

        List<GameObject> children;

        SquadParent squadParent;

        bool attacked = false;

        public Crowd(Enemy enemy, StateMachine stateMachine, Renderer rend, Transform player) : base(enemy, stateMachine, rend, player)
        {
        }

        public override bool Enter()
        {
            EnteredState = base.Enter();

            if (EnteredState)
            {
                Debug.Log("ENTERED CROWD STATE");
                _rend.material.SetColor("_Color", Color.green);
                attacked = false;
                _totalDuration = 0f;
                squadParent = GameObject.Find("Target").GetComponent<SquadParent>();
                children = _enemy.children;
            }
            
            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState)
            {
                //Player detection
                _enemy.playerInSight = _enemy.PlayerSeen(_enemy._navMeshAgent.transform, _player, _enemy.maxAngle, _enemy.maxRadius);
                _enemy.playerInDetectionRadius = Vector3.Distance(_enemy._navMeshAgent.transform.position, _player.position) <= _enemy._detectionRadius;

                //Check for enemies arriving at crowd target
                if (Vector3.Distance(_enemy._navMeshAgent.transform.position, _enemy.crowdTarget.transform.position) <= 5f)
                {
                    squadParent.numOfChildrenAtTarget++;

                    if (squadParent.numOfChildrenAtTarget > 5)
                    {
                        //Pick a new random target position
                        squadParent.ChangeTargetPosition();
                        squadParent.numOfChildrenAtTarget = 0;
                    }
                }

                SetDestination(_enemy.crowdTarget.transform.position);

                Separation();
                Cohesion();

                //Chase if player is seen           
                if (_enemy.playerInSight || _enemy.playerInDetectionRadius)
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

        private void Separation()
        {

            foreach (GameObject child in children)
            {
                if (child != null)
                {
                    float d = (_enemy._navMeshAgent.transform.position - child.transform.position).magnitude;

                    if ((d > 0) && (d < desiredSeparation))
                    {
                        //Calculate vector pointing away from other crowd enemies
                        Vector3 diff = _enemy._navMeshAgent.transform.position - child.transform.position;
                        diff.Normalize();
                        diff /= d;

                        Vector3 temp = _enemy._navMeshAgent.transform.position += diff;

                        _enemy._navMeshAgent.nextPosition = temp;
                        
                    }
                }
            }
        }

        private void Cohesion()
        {
            int count = 0;

            foreach (GameObject child in children)
            {
                if (child != null)
                {
                    float d = (_enemy._navMeshAgent.transform.position - child.transform.position).magnitude;
                    if ((d > 0) && (d < neighborDist))
                    {
                       Vector3 diff = child.transform.position;
                        count++;

                        Vector3 temp = _enemy.crowdTarget.transform.position + diff;
                        _enemy._navMeshAgent.nextPosition = temp;
                    }
                }
            }

            if (count > 0)
            {
                
                //_enemy._navMeshAgent.nextPosition = _enemy.crowdTarget.transform.position / count;
                //_enemy._navMeshAgent.nextPosition = _enemy._navMeshAgent.transform.position - _enemy.crowdTarget.transform.position;

            }
        }
    }
}