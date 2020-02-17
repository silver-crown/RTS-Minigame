using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResources : Behavior
{
    enum MessageForDrones
    {
        GATHERMETAL,
        GATHERCRYSTAL,
    }
    MessageForDrones MyMessage;
    // Start is called before the first frame update
    void Start()
    {
        SendMessageToDrones(WhatResourceDoINeed());
    }

    override
    protected Status UpdateBehavior()
    {
        return Status.RUNNING;
    }
    /// <summary>
    ///It sends a message from here to IDLE drones. The drone then goes and gathers the resource ordered.
    /// </summary>
    /// <param name="myMessage"></param>
    void SendMessageToDrones(MessageForDrones m)
    {
        //for every Worker Drone
        //Check if it's idle
        EventManager.TriggerEvent(m.ToString());
        //Drones that are listening for the message will do the rest
    }
    ///<summary>
    ///Check what resources you have the least of, and return which one it is you need.
    /// </summary>
    private MessageForDrones WhatResourceDoINeed()
    {
        ResourcesAvailable myResources = GetComponent<ResourcesAvailable>();
        MyMessage = (myResources.GetCrystal() > myResources.GetMetals()) ? MessageForDrones.GATHERMETAL : MessageForDrones.GATHERCRYSTAL;
        return MyMessage;
    }
}
