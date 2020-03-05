using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    /// <summary>
    /// Each channel needs to store their own messages on dictionaries
    /// Personal channels for the drones are located in Drone.cs
    /// </summary>
    private Dictionary<string, UnityEvent> _globalChannelDictionary;
    private Dictionary<string, UnityEvent> _workerChannelDictionary;
    private Dictionary<string, UnityEvent> _scoutChannelDictionary;
    private Dictionary<string, UnityEvent> _tankChannelDictionary;
    private List<Dictionary<string, UnityEvent>> _privateChannelList;
    private static EventManager _eventManager;

    //the channels start at 0, c# hates me and refuses to let me set an int as NULL
    const int noID = -1;
    /// <summary>
    /// The actual channel being listened to 
    /// </summary>
    public enum MessageChannel
    {
        globalChannel,
        privateChannel,
        workerChannel,
        scoutChannel,
        tankChannel
    }

    //Get the EventManager if there's one present in the scene
    private static EventManager instance 
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
        if(_globalChannelDictionary == null)
        {
            _globalChannelDictionary = new Dictionary<string, UnityEvent>();
        }
        if (_workerChannelDictionary == null)
        {
            _workerChannelDictionary = new Dictionary<string, UnityEvent>();
        }
        if (_scoutChannelDictionary == null)
        {
            _scoutChannelDictionary = new Dictionary<string, UnityEvent>();
        }
        if (_tankChannelDictionary == null)
        {
            _tankChannelDictionary = new Dictionary<string, UnityEvent>();
        }
        if(_privateChannelList == null)
        {
            _privateChannelList = new List<Dictionary<string, UnityEvent>>();
        }
    }

    //Make the listener start listening
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    /// <param name="channel"></param>
    /// <param name="droneID"></param>
    public static void StartListening(string eventName, UnityAction listener, MessageChannel channel, int droneID = noID)
    {
        UnityEvent thisEvent = null;
        switch (channel)
        {
            //Listening in on the global channel
            //everyone should do this, but not every message should be sent to the global channel
            case (MessageChannel.globalChannel):
                //If there's an event like this in the dictionary, add the listener
                if (instance._globalChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary.
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    instance._globalChannelDictionary.Add(eventName, thisEvent);
                }
                break;
            //Listening in on the private channel
            //Each drone has a private channel, this case will exit immediately if an id is not present
            case (MessageChannel.privateChannel):
                //the channels start at 0, c# hates me and refuses to let me set an int as NULL
                if (droneID > noID)
                {

                    //If there's an event like this in the dictionary, add the listener
                    if (instance._privateChannelList[droneID].TryGetValue(eventName, out thisEvent))
                    {
                        thisEvent.AddListener(listener);
                    }
                    //Else add the event to the dictionary.
                    else
                    {
                        thisEvent = new UnityEvent();
                        thisEvent.AddListener(listener);
                        instance._privateChannelList[droneID].Add(eventName, thisEvent);
                    }
                }
                else
                {
                    Debug.Log("Trying to listen in on invalid Private Channel ID!");
                }
                break;
            //Listening in on the worker channel
            //This is meant for drones that primarily gather resources, ala AoE villagers
            case (MessageChannel.workerChannel):
                //If there's an event like this in the dictionary, add the listener
                if (instance._workerChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary.
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    instance._workerChannelDictionary.Add(eventName, thisEvent);
                }
                break;
            //Listening in on the scount channel
            //These are for the scouting drones
            case (MessageChannel.scoutChannel):
                //If there's an event like this in the dictionary, add the listener
                if (instance._scoutChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary.
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    instance._scoutChannelDictionary.Add(eventName, thisEvent);
                }
                break;
            //Listening in on the tank channel
            //This is meant for the heavy units
            case (MessageChannel.tankChannel):
                //If there's an event like this in the dictionary, add the listener
                if (instance._tankChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary.
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    instance._tankChannelDictionary.Add(eventName, thisEvent);
                }
                break;
        }
    }

    /// <summary>
    /// Stop listening in on the specified channel event
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    /// <param name="channel"></param>
    /// <param name="droneID"></param>
    public static void StopListening(string eventName, UnityAction listener, MessageChannel channel, int droneID = noID)
    {
        //if there's no manager then just jump out
        if (_eventManager == null)
            return;
        UnityEvent thisEvent = null;
        switch (channel)
        {
            case (MessageChannel.globalChannel):
                //if there's an event like this in the dictionary, remove the listener
                if (instance._globalChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.RemoveListener(listener);
                }
                break;
            case (MessageChannel.privateChannel):
                if(droneID > noID)
                {
                    //if there's an event like this in the dictionary, remove the listener
                    if (instance._privateChannelList[droneID].TryGetValue(eventName, out thisEvent))
                    {
                        thisEvent.RemoveListener(listener);
                    }
                }
                else
                {
                    Debug.Log("Trying to remove Private Channel with invalid ID!");
                }
                break;
            case (MessageChannel.workerChannel):
                //if there's an event like this in the dictionary, remove the listener
                if (instance._workerChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.RemoveListener(listener);
                }
                break;
            case (MessageChannel.scoutChannel):
                //if there's an event like this in the dictionary, remove the listener
                if (instance._scoutChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.RemoveListener(listener);
                }
                break;
            case (MessageChannel.tankChannel):
                //if there's an event like this in the dictionary, remove the listener
                if (instance._tankChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.RemoveListener(listener);
                }
                break;
        }
    }
    /// <summary>
    /// Trigger the event on the specified channel, actually sending the message to the listeners.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="channel"></param>
    public static void TriggerEvent(string eventName, MessageChannel channel, int droneID = noID)
    {
        UnityEvent thisEvent = null;
        //Invoke an event in the specified channel, if that event actually exists.
        switch (channel)
        {
            case (MessageChannel.globalChannel):
                if (instance._globalChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.Invoke();
                }
                break;
            case (MessageChannel.privateChannel):
                if (droneID >= noID)
                {
                    if (instance._privateChannelList[droneID].TryGetValue(eventName, out thisEvent))
                    {
                        thisEvent.Invoke();
                    }
                }
                break;
            case (MessageChannel.workerChannel):
                if (instance._workerChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.Invoke();
                }
                break;
            case (MessageChannel.scoutChannel):
                if (instance._scoutChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.Invoke();
                }
                break;
            case (MessageChannel.tankChannel):
                if (instance._tankChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.Invoke();
                }
                break;
        }
    }
    /// <summary>
    /// Add a private channel to the list of Private Channels
    /// </summary>
    /// <param name="channel"></param>
    public static void AddPrivateChannel(Dictionary<string, UnityEvent> channel)
    {
        Debug.Log("adding a private channel to the list");
        instance._privateChannelList.Add(channel);
    }

    /// <summary>
    /// Remove a private channel from the Private Channel list
    /// </summary>
    /// <param name="channel"></param>
    public static void RemovePrivateChannel(Dictionary<string, UnityEvent> channel)
    {
        Debug.Log("removing a private channel to the list");
        instance._privateChannelList.Remove(channel);
    }
}
