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
    ///<summary>
    ///List of all the messages the drone will me listening after
    /// </summary>
    public List<string> messageList = new List<string>();

    /// <summary>
    /// Unique ID of the drone
    /// </summary>
    public int ID { get; protected set; }

    //************************************************************************************
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
    //******************************************************************************

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


    public override void Awake()
    {
        base.Awake();

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

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        listenToChannels();
    }

    public void ReceiveMessageOnChannel(string message, EventManager.MessageChannel channel)
    {
        //a switch for the channel
        switch (channel)
        {
            case (EventManager.MessageChannel.globalChannel):
                {
                   //and a nested one for the message itself
                    switch (message)
                    {
                        //a test message
                        case ("Test message"):
                            {
                                break;
                            }
                    }
                    break;
                }
        }
    }

}
