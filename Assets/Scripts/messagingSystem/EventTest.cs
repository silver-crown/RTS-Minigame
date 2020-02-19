using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTest : MonoBehaviour
{

    private UnityAction someListener;
    // Start is called before the first frame update
    void Awake()
    {
        someListener = new UnityAction(SomeFunction);
    }

    private void OnEnable()
    {
        EventManager.StartListening("test", someListener, EventManager.MessageChannel.globalChannel);
        EventManager.StartListening("Spawn", SomeOtherFunction, EventManager.MessageChannel.scoutChannel);
        EventManager.StartListening("Destroy", SomeThirdFunction, EventManager.MessageChannel.tankChannel);
    }

    private void OnDisable()
    {
        EventManager.StopListening("test", someListener, EventManager.MessageChannel.globalChannel);
        EventManager.StopListening("Spawn", SomeOtherFunction, EventManager.MessageChannel.scoutChannel);
        EventManager.StopListening("Destroy", SomeThirdFunction, EventManager.MessageChannel.tankChannel);
    }

    void SomeFunction()
    {
        Debug.Log("Some Function was called!");
    }
    void SomeOtherFunction()
    {
        Debug.Log("Some Other Function was called!");
    }
    void SomeThirdFunction()
    {
        Debug.Log("Some Third Function was called!");
    }
}
