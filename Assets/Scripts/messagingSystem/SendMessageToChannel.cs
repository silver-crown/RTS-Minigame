using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageToChannel : MonoBehaviour
{

    [SerializeField] EventManager.MessageChannel channel;
    private string message;
    [SerializeField] bool sending;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sending)
        {
            SendMessage(message, channel);
        }
    }
    void SendMessage()
    {
        EventManager.TriggerEvent(message, channel);
    }
}
