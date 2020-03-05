using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageToChannel : MonoBehaviour
{
    [SerializeField] static EventManager eventManager; 
    [SerializeField] EventManager.MessageChannel channel;
    private string message;
    [SerializeField] bool sending;
    // Start is called before the first frame update
    void Start()
    {
        if(message == null)
        {
            message = "Test message";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sending)
        {
            SendMessage(message, channel);
        }
    }
    /// <summary>
    /// Send a message to the channel in question
    /// </summary>
    void SendMessage()
    {
        EventManager.TriggerEvent(message, channel);
    }
}
