using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour tree task which sends a message to other drones.
/// </summary>
public class SendMessageToDronesBehavior : Behavior
{
    const int noID = -1;
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

    override
    protected Status UpdateBehavior()
    {
        return Status.RUNNING;
    }
    /// <summary>
    ///It sends a message to drones listening to the relevant channel
    /// </summary>
    /// <param name="myMessage"></param>
    /// <param name="channel"></param>
    public void SendMessageToDrones(MessageType m, EventManager.MessageChannel channel)
    {
        //Drones that are listening for the message will do the rest
        switch (channel)
        {
            case (EventManager.MessageChannel.globalChannel):
                break;
            case (EventManager.MessageChannel.workerChannel):
                //Check which resource is most desirable atm, and send them to go get that
                //if an ID is present, send to _privateChannelList[ID]
                if (m == MessageType.GatherMetal)
                {
                    GatherMetal();
                }
                if (m == MessageType.GatherCrystal)
                {
                    GatherCrystal();
                }
                if (m == MessageType.Gather)
                {
                    Gather();
                }
                break;
            case (EventManager.MessageChannel.scoutChannel):
                break;
            case (EventManager.MessageChannel.tankChannel):
                break;
        }
    }
    /// <summary>
    ///Sends a PRIVATE message from to drones listening in
    /// </summary>
    /// <param name="myMessage"></param>
    /// <param name="channel"></param>
    /// <param name="droneID"></param>
    public void SendPrivateMessageToDrones(MessageType m, EventManager.MessageChannel channel, int droneID = noID)
    {
        //Drones that are listening for the message will do the rest
        switch (channel)
        {
            case (EventManager.MessageChannel.privateChannel):
                //Make sure there's a legal ID
                if(droneID >= noID)
                {
                    if (m == MessageType.GatherMetal)
                    {
                        GatherMetal(droneID);
                    }
                    if (m == MessageType.GatherCrystal)
                    {
                        GatherCrystal(droneID);
                    }
                }
                else
                {
                    Debug.Log("Trying to send message to an invalid Drone ID!");
                }
                break;
        }
    }


    /// <summary>
    /// Tells the drone to collect metall on the worker channel
    /// </summary>
    /// <param name="droneID"></param>
    public void GatherMetal(int droneID = noID)
    {
        //A specific order to a specific individual
        if(droneID != noID)
        {
            EventManager.TriggerEvent(MessageType.GatherMetal.ToString(), EventManager.MessageChannel.privateChannel, droneID);
        }
        //the general one
        else
        {
            EventManager.TriggerEvent(MessageType.GatherMetal.ToString(), EventManager.MessageChannel.workerChannel);
        }

    }

    /// <summary>
    /// Tells the drone to collect crystal on the worker channel
    /// </summary>
    /// <param name="droneID"></param>
    public void GatherCrystal(int droneID = noID)
    {
        //a specific order to a specific individual
        if(droneID != noID)
        {
            EventManager.TriggerEvent(MessageType.GatherMetal.ToString(), EventManager.MessageChannel.privateChannel, droneID);
        }
        //An order to everyone listening in on the worker channel
        else
        {
            EventManager.TriggerEvent(MessageType.GatherCrystal.ToString(), EventManager.MessageChannel.workerChannel);
        }
       
    }

    /// <summary>
    ///  Tells the drone to collect resoruces
    /// </summary>
    /// <param name="droneID"></param>
    public void Gather(int droneID = noID)
    {
        //a specific order to a specific individual
        if (droneID != noID)
        {
            EventManager.TriggerEvent(MessageType.Gather.ToString(), EventManager.MessageChannel.privateChannel, droneID);
        }
        //An order to everyone listening in on the worker channel
        else
        {
            EventManager.TriggerEvent(MessageType.Gather.ToString(), EventManager.MessageChannel.workerChannel);
        }
    }
   
    ///<summary>
    ///Check what resources you have the least of, and return which one it is you need.
    /// </summary>
    private void CheckResourceRequirements()
    {
        //MyMessage = (myResources.GetCrystal() > myResources.GetMetals()) ? MessageType.GatherMetal : MessageType.GatherCrystal;
    }
}
