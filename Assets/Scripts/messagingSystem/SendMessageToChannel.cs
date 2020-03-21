﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageToChannel : MonoBehaviour
{
    [SerializeField] private EventManager.MessageChannel _channel;
    private string _message;
    [SerializeField] bool sending;
    // Start is called before the first frame update
    void Start()
    {
        if(_message == null)
        {
            _message = "Test message";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sending)
        {
            SendMessage (_message);
        }
    }
    /// <summary>
    /// Send a message to the channel specified, ID is optional
    /// </summary>
    /// <param name="message"></param>
    /// <param name="channel"></param>
    /// <param name="ID"></param>
    public void Send(string message, EventManager.MessageChannel channel, int ID = -1)
    {
        _channel = channel;
        _message = message;
        EventManager.TriggerEvent(_message, _channel, ID);
    }
}
