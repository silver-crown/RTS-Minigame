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

    ListenToChannel()
    {
        string type = drone.Type;
        switch (type)
        {
            case "test":
                {
                    _channel = EventManager.MessageChannel.globalChannel;
                    break;
                }
            default:
                break;
        }
    }

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
        EventManager.MessageChannel global = EventManager.MessageChannel.globalChannel;
        for (int i = 0; i<= drone.messageList.Count; i++)
        {
            //Listen in on the global channel
            EventManager.StartListening(drone.messageList[i], ProxyMessageReceiver, global);
            //set message to be the current message
            message = drone.messageList[i];
            //listen in on the private channel
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
