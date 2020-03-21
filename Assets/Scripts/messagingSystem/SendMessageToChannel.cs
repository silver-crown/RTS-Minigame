using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageToChannel : MonoBehaviour
{
    [SerializeField] private EventManager.MessageChannel _channel;
    private string _message;
    [SerializeField] bool sending;

    void Start()
    {
        if(_message == null)
        {
            _message = "Test message";
        }
    }


    void Update()
    {
        if (sending)
        {
            SendMessage (_message);
        }
    }
    /// <summary>
    /// Send a message to the channel in question
    /// </summary>
    public void Send(string message, EventManager.MessageChannel channel, int ID = -1)
    {
        _channel = channel;
        _message = message;
        EventManager.TriggerEvent(_message, _channel, ID);
    }
}
