using Assets.Code.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ExecutionState 
{
    NONE,
    ACTIVE,
    COMPLETED,
    TERMINATED,
};

public enum FSMStateType
{
    IDLE,
    PATROL,
    CHASE,
    ATTACK,
};

public abstract class AbstractFSMState : ScriptableObject
{
    protected NavMeshAgent _navMeshAgent;
    protected FsmEnemy _fsmEnemy;
    protected Transform _player;
    protected FiniteStateMachine _fsm;

    //dont have to be attached to a gameobject
    //should need a gameobjet for each state which is redundant 

    /*
    ExecutionState _executionState;

    public ExecutionState ExecutionState
    {
        get
        {
            return _executionState;
        }
        protected set
        {
            _executionState = value;
        }
    }*/

    public ExecutionState ExecutionState { get; protected set; }
    public FSMStateType StateType { get; protected set; }
    public bool EnteredState { get; protected set; }

    public virtual void OnEnable()
    {
        ExecutionState = ExecutionState.NONE;
    }

    public virtual bool EnterState()
    {
        bool successNavMesh = true;
        bool successNPC = true;
        ExecutionState = ExecutionState.ACTIVE;

        //check for navmesh
        successNavMesh = (_navMeshAgent != null);

        //check for executing agent
        successNPC = (_fsmEnemy != null);

        return successNavMesh & successNPC;
    }

    //virtual can be overwritten if needed
    public virtual bool ExitState()
    {
        ExecutionState = ExecutionState.COMPLETED;
        return true;
    }

    //declared method, but doesnt have to be written
    //when creating a new state class they all have to write their own updateState
    public abstract void UpdateState();

    public virtual void SetNavMeshAgent(NavMeshAgent navMeshAgent)
    {
        if(navMeshAgent != null)
        {
            _navMeshAgent = navMeshAgent;
        }
    }

    public virtual void SetExecutingFsmEnemy(FsmEnemy fsmEnemy)
    {
        if (fsmEnemy != null)
        {
            _fsmEnemy = fsmEnemy;
        }
    }

    public virtual void SetExecutingFSM(FiniteStateMachine fsm)
    {
        if (fsm != null)
        {
            _fsm = fsm;
        }
    }

    public virtual void SetExecutingPlayer(Transform player)
    {
        if (player != null)
        {
            _player = player;
        }
    }

}
