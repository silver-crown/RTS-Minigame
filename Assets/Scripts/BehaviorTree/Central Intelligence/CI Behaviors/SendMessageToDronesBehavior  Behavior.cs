using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour tree task which sends a message to other drones.
/// </summary>
public class SendMessageToDronesBehavior : Behavior
{
    /// <summary>
    /// Types of messages that the drone needs
    /// </summary>
    public enum MessageType
    {
        Gather,
        GatherMetal,
        GatherCrystal,
        Attack,
        Scout,
        Defend,
        ambush,
    }

    MessageType MyMessage = MessageType.GatherCrystal;

    // Start is called before the first frame update
    void Start()
    {
        SendMessageToDrones(CheckResourceRequirements());
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
    public virtual void SendMessageToDrones(MessageType m)
    {
        //for every Worker Drone
        EventManager.TriggerEvent(m.ToString());
        //Drones that are listening for the message will do the rest

        ////tanks
        /////scounts
        ///
    }

    /// <summary>
    /// Tells the drone to collect metall
    /// </summary>
    public void CollectMetall()
    {
        EventManager.TriggerEvent(MessageType.GatherMetal.ToString());
    }

    /// <summary>
    /// Tells the drone to collect crystal
    /// </summary>
    public void CollectCrystal()
    {
        EventManager.TriggerEvent(MessageType.GatherCrystal.ToString());
    }

    /// <summary>
    ///  Telles the drone to collect resoruces
    /// </summary>
    public void Gather()
    {
        EventManager.TriggerEvent(MessageType.Gather.ToString());
    }
   

    ///<summary>
    ///Check what resources you have the least of, and return which one it is you need.
    /// </summary>
    private MessageType CheckResourceRequirements()
    {
        //MyMessage = (myResources.GetCrystal() > myResources.GetMetals()) ? MessageType.GatherMetal : MessageType.GatherCrystal;
        return MyMessage;
    }
}
