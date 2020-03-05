using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListenToChannel : MonoBehaviour
{
    [SerializeField] Drone drone;
    [SerializeField] bool listening;
    [SerializeField] EventManager.MessageChannel _channel;
    private string message;
    //Convert the enum into a proper
    void Start()
    {
        if(message == null)
        {
            message = "Test message";
        }
    }
    void Update()
    {
        if (listening)
        {
            ListenInOnChannel();
        }
    }

    /// <summary>
    /// Listen continually for messages on the channel provided in the enumerator
    /// </summary>
    void ListenInOnChannel()
    {
        for(int i = 0; i<= drone.messageList.Count; i++)
        {
            //set message to be the current message
            message = drone.messageList[i];
            if (_channel == EventManager.MessageChannel.privateChannel)
            {
                EventManager.StartListening(drone.messageList[i], ProxyMessageReceiver, _channel, drone.ID);
            }
            else
            {
                EventManager.StartListening(drone.messageList[i], ProxyMessageReceiver, _channel);
            }
        }
    }
    /// <summary>
    /// A proxy function used to invoke the secondary function in the listener
    /// StartListening does not take parameters on the UnityAction, so a proxy was necessary to avoid complications.
    /// </summary>
    void ProxyMessageReceiver()
    {
        //if the function is invoked, it'll send the message it was currently listening for
        drone.ReceiveMessageOnChannel(message, _channel);
    }
}
