using Assets.Code.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent), typeof(FiniteStateMachine))]

public class FsmEnemy : MonoBehaviour
{
    [SerializeField]
    ConnectedWaypoint[] _patrolPoints;

    NavMeshAgent _navMeshAgent;
    FiniteStateMachine _finiteStateMachine;

    public float sightRange, attackRange;

    public void Awake()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _finiteStateMachine = this.GetComponent<FiniteStateMachine>();
    }

    public void Start()
    {

    }

    public void Update() 
    {
                 
    }

    public ConnectedWaypoint[] PatrolPoints
    {
        get
        {
            return _patrolPoints;
        }
    }

}



