using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// By attaching the Walkingscript to a GameObject it can move in the gameworld
/// towards a target position or waypoints.  Remember to add a NavMeshAgent to the gameobject this script is 
/// attached to.
/// </summary>
public class Walking : MonoBehaviour
{
    public GameObject[] Waypoints;
    public GameObject TargetDestination;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(TargetDestination.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}