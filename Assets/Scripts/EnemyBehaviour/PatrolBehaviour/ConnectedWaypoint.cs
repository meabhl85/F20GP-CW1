using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class ConnectedWaypoint : CheckPoint
{
    [SerializeField]
    protected float _connectivityRadius = 50f;

    List<ConnectedWaypoint> _connections;

    public void Start()
    {
        //Grab all waypoints in the scene
        GameObject[] allWayPoints = GameObject.FindGameObjectsWithTag("Waypoint");

        //Create a list of waypoints
        _connections = new List<ConnectedWaypoint>();

        //check if the waypoints are connected
        for (int i = 0; i < allWayPoints.Length; i++)
        {
            ConnectedWaypoint nextWayPoint = allWayPoints[i].GetComponent<ConnectedWaypoint>();

            if (nextWayPoint != null)
            {
                if (Vector3.Distance(this.transform.position, nextWayPoint.transform.position) <= _connectivityRadius && nextWayPoint != this)
                {
                    _connections.Add(nextWayPoint);
                }
            }
        }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _connectivityRadius);
    }

    public ConnectedWaypoint NextWaypoint(ConnectedWaypoint previousWaypoint)
    {
        if(_connections.Count == 0)
        {
            Debug.LogError("Insufficient waypoint count");
            return null;
        }
        else if (_connections.Count == 1 && _connections.Contains(previousWaypoint))
        {
            return previousWaypoint;
        }
        else
        {
            ConnectedWaypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, _connections.Count);
                nextWaypoint = _connections[nextIndex];


            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}
