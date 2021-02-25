using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Assets.Code.NPCCodes;
using Assets.Scripts.FSM;
using UnityEngine.AI;

namespace Assets.Scripts.EnemyBehaviour
{
    public class SquadParent : MonoBehaviour
    {

        public GameObject target;
        public GameObject childPrefab;
        public List<GameObject> children;
        public List<GameObject> targetPositions;

        public float numOfChildrenAtTarget = 0;
        public int noOfChildren = 5;

        // Start is called before the first frame update
        void Start()
        {
            //List of crowd members
            children = new List<GameObject>();
            
            //Random Speed
            System.Random random = new System.Random();
            int randomInt = random.Next(3, 7);

         
            //Spawn Crowd Enemies
            for (int i = 0; i < noOfChildren; i++)
            {
                Vector3 relativeSpawn = new Vector3(i % 3, 0.33f, i / 2);
                GameObject temp = Instantiate(childPrefab, transform.position + (relativeSpawn * 6.0f), transform.rotation);

                temp.GetComponent<Enemy>().patrollingEnabled = false;
                temp.GetComponent<Enemy>().crowdEnemy = true;
                temp.GetComponent<Enemy>().crowdTarget = gameObject;
                temp.GetComponent<NavMeshAgent>().speed = randomInt;

                children.Add(temp);
            }

            //Set each enemy with the same crowd target
            foreach (GameObject child in children)
            {
                child.GetComponent<Enemy>().children = children;
            }
        }

        void Update()
        {
            transform.position += (target.transform.position - transform.position).normalized * Time.deltaTime * 5.0f;
        }

        public void ChangeTargetPosition()
        {
            int randomIndex = Random.Range(0, targetPositions.Count);
            transform.position = targetPositions[randomIndex].transform.position;
        }

        

    }
}
   
