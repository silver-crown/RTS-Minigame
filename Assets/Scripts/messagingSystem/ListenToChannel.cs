using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yeeter;

public class ListenToChannel : EventManager
{
    [SerializeField] private Drone _drone;
    [SerializeField] bool listening;
    [SerializeField] MessageChannel _channel;
    [SerializeField] private string _message;

    private int _lastMessage;

    public Action MessageReceived;

    ///<summary>
    ///List of all the messages the entity will me listening after
    /// </summary>
    private List<string> _messageList = new List<string>();

    /// <summary>
    /// String used for listening to messages contained in the message list
    /// </summary>
    private string[] message;


    /// <summary>
    /// Each channel needs to store their own messages on dictionaries
    /// </summary>
    private Dictionary<string, UnityEvent> _personalChannels;

    public void Init(EventManager.MessageChannel channel)
    {

        _drone = GetComponent<Drone>();
        _channel = channel;
    }

    void Start()
    {
        _personalChannels = new Dictionary<string, UnityEvent>();

        ///<summary>if the entity's channel isn't in the list, add it there</summary>
        if (!_privateChannels.Contains(_personalChannels))
        {
            AddPrivateChannel(_personalChannels);
        }
        if (_message == null)
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
            //if it's a message meant for an individual
            if (MessageList().Contains(_message))
            {
                EventManager.MessageChannel global = EventManager.MessageChannel.globalChannel;
                for (int i = 0; i <= _messageList.Count; i++)
                {
                    //Listen in on the global channel
                    StartListening(_messageList[i], () => { ReceiveMessage(_message); }, global);

                    //listen in on the private channel
                    if (_channel == EventManager.MessageChannel.privateChannel)
                    {
                        StartListening(_messageList[i], () => { ReceiveMessage(_message); }, _channel, _drone.ID);
                    }
                    //Listen without any ID on a channel
                    else
                    {
                        StartListening(_messageList[i], () => { ReceiveMessage(_message); }, _channel);
                    }
                }
            }
            //if it's a message meant for a group
            else if (GroupMessageList().Contains(_message))
            {
                //Check if the drone is a leader
                if (_drone.leaderStatus)
                {
                    ///<summary>For every message the leader can listen to</summary>
                    for (int i = 0; i <= GroupMessageList().Count; i++)
                    {
                        ///<summary>Set the message to be the current message the loop is iterating through</summary>
                        _message = GroupMessageList()[i];
                        ///<summary>Listen to this message</summary>
                        StartListening(_message, () => { _drone.GetComponent<GroupLeader>().groupMessageList.Add(_message); }, EventManager.MessageChannel.groupChannel, _drone.groupID);
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
    /// Gets the most recent message from the message list
    /// </summary>
    /// <returns></returns>
    public string GetLastMessage()
    {
        return _messageList[_messageList.Count - 1];
    }
    /// <summary>
    /// Add message to the message list. 
    /// </summary>
    /// <param name="message">Message that was received</param>
    public void ReceiveMessage(string message)
    {
        //add the received message to the list of messages, for use in other functions later.
        _messageList.Add(message);
        MessageReceived?.Invoke();
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
