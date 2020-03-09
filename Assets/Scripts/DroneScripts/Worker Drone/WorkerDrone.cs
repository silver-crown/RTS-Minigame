using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;

public class WorkerDrone : Drone
{
    /// <summary>
    /// How much Metall the drone is currecntly carrying
    /// </summary>
    public int AmountMetall { get; protected set; }

    /// <summary>
    /// The max amount of resources a drone can carry
    /// </summary>
    public int MaxResources { get; protected set; }

    /// <summary>
    /// Keeps track if the drone has room for more resrouces or is full.
    /// </summary>
    public bool IsInventoryFull { get; protected set; }

    override
    public void SetDroneType()
    {
        Script script = new Script();

        // Calls Read Stats in parent 
        base.SetDroneType();

        MaxResources = (int)script.Globals.Get("maxResources").Number;
        Debug.Log(MaxResources);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
