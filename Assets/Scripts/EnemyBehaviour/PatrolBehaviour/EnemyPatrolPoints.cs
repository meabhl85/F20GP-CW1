using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolPoints : MonoBehaviour
{
    [SerializeField]
    bool patrolWaiting;

    [SerializeField]
    List<CheckPoint> patrolPoints;

    NavMeshAgent navMeshAgent;
    int currentPatrolIndex;
    bool travelling = true;
    bool waiting = false;
    bool patrolForward = true;
    bool waitTimer;

    bool isAware = false;

    public Transform player;

    public GameObject lookPoint;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(navMeshAgent.remainingDistance);

        
        if (isAware)
        {
            navMeshAgent.SetDestination(player.position);

        } else
        {
            //search 

            Vector3 fwd = lookPoint.transform.TransformDirection(Vector3.forward);


            RaycastHit hitInfo;

            //Debug.DrawRay(lookPoint.transform.position, lookPoint.transform.forward, Color.red);

            Ray ray = new Ray(lookPoint.transform.position, lookPoint.transform.forward);
            Debug.DrawRay(lookPoint.transform.position, lookPoint.transform.forward * 100, Color.red, 2f);


            if (Physics.Raycast(ray, out hitInfo, 100))
            {
                //Debug.Log(hit.transform.name);
                ///Debug.DrawLine(transform.position, hit.point, Color.red);

                if (hitInfo.transform.name == "Player")
                {
                    Debug.Log("playerhit");
                    isAware = true;

                }

            }
                    //check to see if we are close to the destination
            if (travelling && navMeshAgent.remainingDistance <= 1.0f)
            {
                travelling = false;

                ChangePatrolPoint();
                SetDestination();

            }

        }

    }

    public void ChangePatrolPoint()
    {
        if (patrolForward)
        {
            Debug.Log("here");
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        } else
        {
            if (--currentPatrolIndex < 0)
            {
                currentPatrolIndex = patrolPoints.Count - 1;
            }
        }
    }

    public void SetDestination()
    {
        if (patrolPoints != null)
        {
            Vector3 targetVector = patrolPoints[currentPatrolIndex].transform.position;
            navMeshAgent.SetDestination(targetVector);
            travelling = true;
        }
    }

}
