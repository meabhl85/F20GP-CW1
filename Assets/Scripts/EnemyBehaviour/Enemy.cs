using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.EnemyBehaviour
{
    public class Enemy : MonoBehaviour
    {
        //References to components needed for enemy behaviours
        public NavMeshAgent _navMeshAgent;
        public Transform _player;
        public StateMachine _stateMachine;
        Renderer rend;
        
        //List of patrol points
        [SerializeField]
        ConnectedWaypoint[] _patrolPoints;

        //States
        public IdleState idle;
        public PatrolState patrol;
        public ChaseState chase;
        public AttackState attack;
        public Crowd crowd;

        //Crowd Behaviour
        public bool crowdEnemy = false;
        public GameObject crowdTarget;

        [SerializeField]
        public List<GameObject> children;


        //Bool checks
        public bool alive = true;
        public bool patrollingEnabled;

        //Enemy Sight
        public float maxAngle;
        public float maxRadius;
        public bool isInFov = false;
        public bool playerInSight = false;
        public bool playerInDetectionRadius = false;
        public float _detectionRadius = 10f;
        public float _attackRadius = 5f;

        private void Start()
        {
            _stateMachine = new StateMachine();

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _player = GameObject.Find("Player").transform;
            rend = GetComponent<Renderer>();

            idle = new IdleState(this, _stateMachine, rend, _player);
            patrol = new PatrolState(this, _stateMachine, rend, _player);
            chase = new ChaseState(this, _stateMachine, rend, _player);
            attack = new AttackState(this, _stateMachine, rend, _player);
            crowd = new Crowd(this, _stateMachine, rend, _player);

            _stateMachine.Initialize(idle);
        }

        private void Update()
        {
            if (alive)
            {
                _stateMachine.currentState.UpdateState();
            }
        }

        public ConnectedWaypoint[] PatrolPoints
        {
            get
            {
                return _patrolPoints;
            }
        }

        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxRadius);

            Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);

            if (!isInFov)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }


            Gizmos.DrawRay(transform.position, (_player.position - transform.position).normalized * maxRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
        }

        public bool PlayerSeen(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
        {
            Collider[] overlaps = new Collider[1000];
            int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

            for (int i = 0; i < count + 1; i++)
            {

                if (overlaps[i] != null)
                {
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

       
    }
}
