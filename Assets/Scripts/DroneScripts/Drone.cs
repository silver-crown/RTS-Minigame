using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;
using RTS;
/// <summary>
/// Drones are used by the enemy AI/CI to interact in the world
/// </summary>
public class Drone : RTS.Actor 
{
    /// <summary>
    /// The last used drone id.
    /// </summary>
    private static int _lastUsedId = 0;

    /// <summary>
    /// Each channel needs to store their own messages on dictionaries
    /// </summary>
    private Dictionary<string, UnityEvent> _personalChannelDictionary;

    /// <summary>
    /// Unique ID of the drone
    /// </summary>
    public int ID { get; protected set; }


    // we should probbly init drones with LUA or Scriptable objects
    private void Awake()
    {
        if (_personalChannelDictionary == null)
        {
           _personalChannelDictionary = new Dictionary<string, UnityEvent>();
        }

        ReadStatsFromFile();
        //add the channel to the private channel list, it's connected to the ID number of the drone
        //Private channel 0 corresponds to Drone ID 0
        EventManager.AddPrivateChannel(_personalChannelDictionary); 
        ID = _lastUsedId++;
    }


    /// <summary>
    /// Reads the drone's stats from lua.
    /// </summary>
    override
    public void ReadStatsFromFile()
    {
        Script script = new Script();
        script.DoFile("drone.lua");
        Health = (int)script.Globals.Get("health").Number;
        Debug.Log(Health);
    }

    // Update is called once per frame
    void Update()
    {
        listenToChannels();
    }
    /// <summary>
    /// example on use of message listening 
    /// </summary>
    void listenToChannels()
    {
        //listening on a public channel
         EventManager.StartListening("Testing Worker Channel", WorkerChannelTest, EventManager.MessageChannel.workerChannel);
        
        //Listening on a private channel requires an id number, the Drone's own id should be provided here
        EventManager.StartListening("Testing Private Channel", PrivateChannelTest, EventManager.MessageChannel.privateChannel, ID);
    }
    void WorkerChannelTest()
    {
        Debug.Log("Drone " + ID + " received a message in the Worker Channel!");
    }
    void PrivateChannelTest()
    {
        Debug.Log("Drone " + ID + " received a message in the Private Channel!");
    }
}
