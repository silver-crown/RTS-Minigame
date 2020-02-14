using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> _eventDictionary;

    private static EventManager _eventManager;



    //Get the EventManager if there's one present in the scene
    public static EventManager instance 
    {
        get 
        {
            if(!_eventManager)
            {
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;
                //If there's no eventmanager send an error, else intialize the dictionary.
                if(!_eventManager)
                {
                    Debug.LogError("put EventManager on a GameObject in your scene first.");
                }
                else
                {
                    _eventManager.Init();
                }
            }
            return _eventManager;
        }
    }

    //Initialize the dictionary for the eventManager
    void Init()
    {
        if(_eventDictionary == null)
        {
            _eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    //Make the listener start listening
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        //If there's an event like this in the dictionary, add the listener
        if(instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        //Else add the event to the dictionary.
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance._eventDictionary.Add(eventName, thisEvent);
        }
    }
    //stop listening to the event
    public static void StopListening(string eventName, UnityAction listener)
    {
        if (_eventManager == null) 
            return;
        UnityEvent thisEvent = null;
        //if there's an event like this in the dictionary, remove the listener
        if(instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }
    //Invoke the damn event
    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if(instance._eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }
}
