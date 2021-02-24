using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EnemyBehaviour
{
    public class IdleState : State
    {
        [SerializeField]
        float _idleDuration = 3f;
        float _totalDuration;

        public IdleState(Enemy enemy, StateMachine stateMachine, Renderer rend, Transform player) : base(enemy, stateMachine, rend, player)
        {
        }

        public override bool Enter()
        {
            EnteredState = base.Enter();
            if(EnteredState && _enemy.alive)
            {
                Debug.Log("ENTERED IDLE STATE");
                _rend.material.SetColor("_Color", Color.white);
                _totalDuration = 0f;
            } else
            {
                _enemy._navMeshAgent.SetDestination(_enemy._navMeshAgent.transform.position);
            }
            
            return EnteredState;
        }

        public override void UpdateState()
        {
            if (EnteredState && _enemy.alive)
            {
                _totalDuration += Time.deltaTime;

                //Player detection
                _enemy.playerInSight = _enemy.PlayerSeen(_enemy._navMeshAgent.transform, _player, _enemy.maxAngle, _enemy.maxRadius);
                _enemy.playerInDetectionRadius = Vector3.Distance(_enemy._navMeshAgent.transform.position, _player.position) <= _enemy._detectionRadius;

                
                if (_totalDuration >= _idleDuration)
                {
                    //Change to crowd state if enemy is a crowd member
                    if (_enemy.crowdEnemy)
                    {
                        _stateMachine.ChangeState(_enemy.crowd);
                    }
                    else if (_enemy.patrollingEnabled)
                    {
                        _stateMachine.ChangeState(_enemy.patrol);
                    }
                        
                }
                //Change to chase state if the player is seen or is close to the enemy
                else if (_enemy.playerInSight || _enemy.playerInDetectionRadius)
                { 
                    _stateMachine.ChangeState(_enemy.chase);
                }
                
                
            }
            
        }

        public static bool PlayerSeen(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
        {
            Collider[] overlaps = new Collider[100];
            int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

            for (int i = 0; i < count + 1; i++)
            {

                if (overlaps[i] != null)
                {
                    //Check that the player is in the view of the enemy
                    if (overlaps[i].transform == target)
                    {
                        Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                        directionBetween.y *= 0;

                        float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                        
                        if (angle <= maxAngle)
                        {
                            Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, maxRadius))
                            {
                                if (hit.transform == target)
                                    return true;
                            }
                        }

                    }
                }
            }

            return false;
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
 
