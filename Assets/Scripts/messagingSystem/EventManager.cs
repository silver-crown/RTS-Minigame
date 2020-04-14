using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The class controlling the events for the game's messaging system
/// </summary>
public class EventManager : MonoBehaviour
{
    /// <summary>
    /// Each channel needs to store their own messages on dictionaries
    /// Personal channel dictionaries for the drones are located in Drone.cs
    /// Each group needs to store their messages in a dictionary as well
    /// </summary>
    protected static Dictionary<string, UnityEvent> _globalChannelDictionary;
    protected static Dictionary<string, UnityEvent> _workerChannelDictionary;
    protected static Dictionary<string, UnityEvent> _scoutChannelDictionary;
    protected static Dictionary<string, UnityEvent> _tankChannelDictionary;
    protected static Dictionary<string, UnityEvent> _CIChannelDictionary;
    protected static List<Dictionary<string, UnityEvent>> _privateChannelList;
    protected static List<Dictionary<string, UnityEvent>> _groupChannelList;
    private static EventManager _eventManager;

    /// <summary>
    /// The default value of the IDs, a negative number because numbers can't be NULL in C# 
    /// </summary>
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
        tankChannel,
        CIChannel,
        groupChannel
    }

    //Initialize the dictionaries for the eventManager
    void Start()
    {
        Debug.Log("Initializing Event Manager");
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
    protected static void StartListening(string eventName, UnityAction listener, MessageChannel channel, int ID = noID)
    {

        UnityEvent thisEvent = null;
        switch (channel)
        {
            //Listening in on the global channel
            //everyone should do this, but not every message should be sent to the global channel
            case (MessageChannel.globalChannel):
                //If there's an event like this in the dictionary, add the listener
                if (_globalChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary.
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    _globalChannelDictionary.Add(eventName, thisEvent);
                }
                break;
            //Listening in on the private channel
            //Each drone has a private channel, this case will exit immediately if an id is not present
            case (MessageChannel.privateChannel):
                //the channels start at 0, corresponding to the Drone IDs
                if (ID > noID)
                {

                    //If there's an event like this in the dictionary, add the listener
                    if (_privateChannelList[ID].TryGetValue(eventName, out thisEvent))
                    {
                        thisEvent.AddListener(listener);
                    }
                    //Else add the event to the dictionary.
                    else
                    {
                        thisEvent = new UnityEvent();
                        thisEvent.AddListener(listener);
                        _privateChannelList[ID].Add(eventName, thisEvent);
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
                if (_workerChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary.
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    _workerChannelDictionary.Add(eventName, thisEvent);
                }
                break;
            //Listening in on the scount channel
            case (MessageChannel.scoutChannel):
                //If there's an event like this in the dictionary, add the listener
                if (_scoutChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary. and add the listener
                else 
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    _scoutChannelDictionary.Add(eventName, thisEvent);
                }
                break;
            //Listening in on the tank channel
            //This is meant for the heavy units
            case (MessageChannel.tankChannel):
                //If there's an event like this in the dictionary, add the listener
                if (_tankChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary. and add the listener
                else 
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    _tankChannelDictionary.Add(eventName, thisEvent);
                }
                break;
            //Listening in on the tank channel
            //This is meant for the CI
            case (MessageChannel.CIChannel):
                //If there's an event like this in the dictionary, add the listener
                if (_tankChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.AddListener(listener);
                }
                //Else add the event to the dictionary. and add the listener
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(listener);
                    _tankChannelDictionary.Add(eventName, thisEvent);
                }
                break;
            //Listening in on the group channel
            //Each drone has a private channel, this case will exit immediately if an id is not present
            case (MessageChannel.groupChannel):
                //the channels start at 0, in accordance to the group IDs
                if (ID > noID)
                {
                    //If there's an event like this in the dictionary, add the listener
                    if (_groupChannelList[ID].TryGetValue(eventName, out thisEvent))
                    {
                        thisEvent.AddListener(listener);
                    }
                    //Else add the event to the dictionary.
                    else
                    {
                        thisEvent = new UnityEvent();
                        thisEvent.AddListener(listener);
                        _groupChannelList[ID].Add(eventName, thisEvent);
                    }
                }
                else
                {
                    Debug.Log("Trying to listen in on invalid Group Channel ID!");
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
    protected static void StopListening(string eventName, UnityAction listener, MessageChannel channel, int droneID = noID)
    {
        //if there's no manager then just jump out
        if (_eventManager == null)
            return;
        UnityEvent thisEvent = null;
        switch (channel)
        {
            case (MessageChannel.globalChannel):
                //if there's an event like this in the dictionary, remove the listener
                if (_globalChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.RemoveListener(listener);
                }
                break;
            case (MessageChannel.privateChannel):
                if(droneID > noID)
                {
                    //if there's an event with this id in the dictionary, remove the listener
                    if (_privateChannelList[droneID].TryGetValue(eventName, out thisEvent))
                    {
                        thisEvent.RemoveListener(listener);
                    }
                }
                //else do nothing
                else
                {
                    Debug.Log("Trying to remove Private Channel with invalid ID!");
                }
                break;
            case (MessageChannel.workerChannel):
                //if there's an event like this in the dictionary, remove the listener
                if (_workerChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.RemoveListener(listener);
                }
                break;
            case (MessageChannel.scoutChannel):
                //if there's an event like this in the dictionary, remove the listener
                if (_scoutChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.RemoveListener(listener);
                }
                break;
            case (MessageChannel.tankChannel):
                //if there's an event like this in the dictionary, remove the listener
                if (_tankChannelDictionary.TryGetValue(eventName, out thisEvent))
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
    /// <param name="droneID"></param>
    protected static void TriggerEvent(string eventName, MessageChannel channel, int droneID = noID)
    {
        UnityEvent thisEvent = null;
        //Invoke an event in the specified channel, if that event actually exists.
        switch (channel)
        {
            case (MessageChannel.globalChannel):
                if (_globalChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.Invoke();
                }
                break;
            case (MessageChannel.privateChannel):
                if (droneID >= noID)
                {
                    if (_privateChannelList[droneID].TryGetValue(eventName, out thisEvent))
                    {
                        thisEvent.Invoke();
                    }
                }
                break;
            case (MessageChannel.workerChannel):
                if (_workerChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.Invoke();
                }
                break;
            case (MessageChannel.scoutChannel):
                if (_scoutChannelDictionary.TryGetValue(eventName, out thisEvent))
                {
                    thisEvent.Invoke();
                }
                break;
            case (MessageChannel.tankChannel):
                if (_tankChannelDictionary.TryGetValue(eventName, out thisEvent))
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
    protected static void AddPrivateChannel(Dictionary<string, UnityEvent> channel)
    {
        Debug.Log("adding a private channel to the list");
        _privateChannelList.Add(channel);
    }

    /// <summary>
    /// Remove a private channel from the Private Channel list
    /// </summary>
    /// <param name="channel"></param>
    protected static void RemovePrivateChannel(Dictionary<string, UnityEvent> channel)
    {
        Debug.Log("removing a private channel from the list");
        _privateChannelList.Remove(channel);
    }
}
