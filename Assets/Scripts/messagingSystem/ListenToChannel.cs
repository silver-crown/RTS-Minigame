using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yeeter;

public class ListenToChannel : EventManager
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
        //if the drone's channel isn't in the list, add it there
        if (!_privateChannelList.Contains(drone.personalChannelDictionary)) {
            AddPrivateChannel(drone.personalChannelDictionary);
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
        ///<summary>If you haven't gone through this method once, thus not listening for anything</summary>
        if (!listening) 
        {
            _message = message;
            //if it's a message meant for individual drones
            if (MessageList().Contains(_message)) 
            {
                EventManager.MessageChannel global = EventManager.MessageChannel.globalChannel;
                for (int i = 0; i <= drone.messageList.Count; i++)
                {
                    //Listen in on the global channel
                    StartListening(drone.messageList[i], () => { drone.ReceiveMessage(_message); }, global);

                    //listen in on the private channel
                    if (_channel == EventManager.MessageChannel.privateChannel) 
                    {
                        StartListening(drone.messageList[i], () => { drone.ReceiveMessage(_message); }, _channel, drone.ID);
                    }
                    //Listen without any ID on a channel
                    else 
                    {
                        StartListening(drone.messageList[i], () => { drone.ReceiveMessage(_message); }, _channel);
                    }
                }
            }
            //if it's a message meant for groups
            else if (GroupMessageList().Contains(_message)) 
            {
                //Check if the drone is a leader
                if (drone.leaderStatus) 
                {
                    ///<summary>For every message the leader can listen to</summary>
                    for (int i = 0; i <= GroupMessageList().Count; i++)
                    {
                        ///<summary>Set the message to be the current message the loop is iterating through</summary>
                        _message = GroupMessageList()[i];
                        ///<summary>Listen to this message</summary>
                        StartListening(_message, () => { drone.GetComponent<GroupLeader>().groupMessageList.Add(_message); }, EventManager.MessageChannel.groupChannel, drone.groupID);
                    }
                }
                else 
                {
                    InGameDebug.Log("Error: this drone is not a leader, don't send him this message!");
                }
            }
        }
        listening = true;
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
