using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour tree task which sends a message to other drones.
/// </summary>
public class SendMessageToDronesBehavior : BbbtBehaviour
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

    override
    protected Status UpdateBehavior()
    {
        return Status.RUNNING;
    }
    /// <summary>
    ///It sends a message from here to drones listening to the relevant channel
    ///It can also send personal messages to individual drones
    /// </summary>
    /// <param name="myMessage"></param>
    /// <param name="channel"></param>
    /// <param name="droneID"></param>
    public void SendMessageToDrones(MessageType m, EventManager.MessageChannel channel, int[] droneID = null)
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
    /// Tells the drone to collect metall on the worker channel
    /// </summary>
    public void GatherMetal()
    {
        EventManager.TriggerEvent(MessageType.GatherMetal.ToString(), EventManager.MessageChannel.workerChannel);
    }

    /// <summary>
    /// Tells the drone to collect crystal on the worker channel
    /// </summary>
    public void GatherCrystal()
    {
        EventManager.TriggerEvent(MessageType.GatherCrystal.ToString(), EventManager.MessageChannel.workerChannel);
    }

    /// <summary>
    ///  Tells the drone to collect resoruces
    /// </summary>
    public void Gather()
    {
        EventManager.TriggerEvent(MessageType.Gather.ToString(), EventManager.MessageChannel.workerChannel);
    }
   
    ///<summary>
    ///Check what resources you have the least of, and return which one it is you need.
    /// </summary>
    private void CheckResourceRequirements()
    {
        //MyMessage = (myResources.GetCrystal() > myResources.GetMetals()) ? MessageType.GatherMetal : MessageType.GatherCrystal;
    }
}
