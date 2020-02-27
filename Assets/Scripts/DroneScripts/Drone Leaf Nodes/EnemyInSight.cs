using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

/// <summary>
/// This Leaf Nodes is a condition that checks if the drone can currently see an enemy
/// </summary>
public class EnemyInSight : Behavior
{
    RTS.Actor _actor;

    public EnemyInSight()
    {

    }

    protected virtual void OnInitialize()
    {
       
    }

}
