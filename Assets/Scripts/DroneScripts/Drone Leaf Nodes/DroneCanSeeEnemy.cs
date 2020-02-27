using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;
using Bbbt;

/// <summary>
/// This Leaf Nodes is a condition that checks if the drone can currently see an enemy
/// </summary>
public class DroneCanSeeEnemy : Bbbt.BbbtBehaviour
{
    private RTS.Actor _actor;

    public DroneCanSeeEnemy()
    {

    }

    public override void AddChild(BbbtBehaviour child)
    {
        throw new System.NotImplementedException();
    }

    public override void RemoveChildren()
    {
        throw new System.NotImplementedException();
    }

    public override BbbtBehaviourSaveData ToSaveData()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnInitialize(GameObject gameObject)
    {
        _actor = gameObject.GetComponent<Actor>();
    }

    protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
    {
        throw new System.NotImplementedException();
    }

    protected override BbbtBehaviourStatus UpdateBehavior(GameObject gameObject)
    {
        Debug.Log("Updating Sight CanSeeEnemy Behavior");

        // Loop over Marine Locations
        for (int i = 0; i < EntityLocations.MarineLocations.Count; i++)
        {
            if (Vector3.Distance(_actor.transform.position, EntityLocations.MarineLocations[i].transform.position) <= _actor.LineOfSight)
            {
                // Inn here we have spotted a player
                Debug.Log("Spotted Enemy Marine!");

                // Records last sighting of the player
                _actor.LastSighting = EntityLocations.MarineLocations[i].transform.position;

                return BbbtBehaviourStatus.Success;
            }
        }
        
        return BbbtBehaviourStatus.Failure;        
    }
}
