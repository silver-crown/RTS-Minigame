using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerTest : MonoBehaviour
{
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            EventManager.TriggerEvent("test", EventManager.MessageChannel.globalChannel);
        }
        if (Input.GetKeyDown("o"))
        {
            EventManager.TriggerEvent("Spawn", EventManager.MessageChannel.workerChannel);
        }
        if (Input.GetKeyDown("p"))
        {
            EventManager.TriggerEvent("Destroy", EventManager.MessageChannel.scoutChannel);
        }
        if (Input.GetKeyDown("x"))
        {
            EventManager.TriggerEvent("Junk", EventManager.MessageChannel.tankChannel);
        }
    }
}
