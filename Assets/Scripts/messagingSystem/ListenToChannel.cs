﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListenToChannel : MonoBehaviour
{
    private Drone drone;
    [SerializeField] bool listening;
    [SerializeField] EventManager.MessageChannel _channel;
    [SerializeField] private string _message;
    private int _lastMessage;


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
        if(GetComponent<Drone>() != null)
        {
            drone = GetComponent<Drone>();
        }

        if(_message == null)
        {
            _message = "Test message";
        }
    }

    /// <summary>
    /// Listen continually for messages on the channel provided in the enumerator
    /// </summary>
    public void ListenToMessage(string message)
    {
        _message = message;
        //if it's a message meant for individual drones
        if (MessageList().Contains(_message))
        {
            EventManager.MessageChannel global = EventManager.MessageChannel.globalChannel;
            for (int i = 0; i <= drone.messageList.Count; i++)
            {
                //Listen in on the global channel
                EventManager.StartListening(drone.messageList[i], () => { drone.ReceiveMessage(_message); }, global);

                //listen in on the private channel
                if (_channel == EventManager.MessageChannel.privateChannel)
                {
                    EventManager.StartListening(drone.messageList[i], () => { drone.ReceiveMessage(_message); }, _channel, drone.ID);
                }
                else
                {
                    EventManager.StartListening(drone.messageList[i], () => { drone.ReceiveMessage(_message); }, _channel);
                }
            }
        }
        //if it's a message meant for groups
        else if (GroupMessageList().Contains(_message))
        {
            //Check if the drone is a leader
            if (drone.leaderStatus)
            {
                for (int i = 0; i <= drone.GetComponent<Group>().groupMessageList.Count; i++)
                {
                    //set message to be the current message
                    _message = drone.GetComponent<Group>().groupMessageList[i];
                    EventManager.StartListening(_message, () => { drone.GetComponent<Group>().groupMessageList.Add(_message); }, EventManager.MessageChannel.groupChannel, drone.groupID);
                }
            }
            else {
                InGameDebug.Log("Error: this drone is not a leader, don't send him this message!");
            }
        }
    }

    /// <summary>
    /// The list of messages individual drones can listen to
    /// </summary>
    private List<string> MessageList()
    {
        List<string> messages = new List<string>();
        int i = 0;
        messages[i++] = "Frontal Assault";
        messages[i++] = "Flanking Assault Frontal";
        messages[i++] = "Flanking Assault Behind";
        messages[i++] = "Flanking Assault Right";
        messages[i++] = "Flanking Assault Left";
        return messages;
    }
    /// <summary>
    ///  The list of messages a group can listen to
    /// </summary>
    private List<string> GroupMessageList()
    {
        List<string> messages = new List<string>();
        int i = 0;
        messages[i++] = "Group Frontal Assault";
        messages[i++] = "Group Flanking Assault";
        return messages;
    }
}
