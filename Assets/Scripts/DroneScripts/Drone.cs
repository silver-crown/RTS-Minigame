using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;

/// <summary>
/// Drones are used by the enemy AI to interact in the world
/// </summary>
public class Drone : MonoBehaviour
{
    /// <summary>
    /// The behaviour to use for this drone.
    /// </summary>
    [SerializeField] BbbtBehaviourTree _behavior = null;

    /// <summary>
    /// The last used drone id.
    /// </summary>
    private static int _lastUsedId = 0;

    /// <summary>
    /// Each channel needs to store their own messages on dictionaries
    /// </summary>
    private Dictionary<string, UnityEvent> _personalChannelDictionary;


    /// <summary>
    /// Behavior Tree used by the drone for micro world behaviors
    /// </summary>
    protected BehaviorTree _behaviorTree;

    /// <summary>
    /// How far the drone can look
    /// </summary>
    public float LineOfSight = 1000.0f;

    /// <summary>
    /// Unique ID of the drone
    /// </summary>
    public int ID { get; protected set; }

    /// <summary>
    /// Current Health Points the drone has
    /// </summary>
    public int Health { get; protected set; }

    /// <summary>
    /// How much Metall the drone is currecntly carrying
    /// </summary>
    public int AmountMetall{ get; protected set; }
    
    /// <summary>
    /// The max amount of resources a drone can carry
    /// </summary>
    public int MaxResources{ get; protected set; }

    /// <summary>
    /// Keeps track if the drone has room for more resrouces or is full.
    /// </summary>
    public bool IsInventoryFull { get; protected set; }


    // we should probbly init drones with LUA or Scriptable objects
    private void Awake()
    {
        //_navMeshAgent = GetComponent<NavMeshAgent>();
        _behaviorTree = GetComponent<BehaviorTree>();

        if(_personalChannelDictionary == null)
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
    public void ReadStatsFromFile()
    {
        Script script = new Script();
        script.DoFile("drone.lua");
        Health = (int)script.Globals.Get("health").Number;
        Debug.Log(Health);

        MaxResources = (int)script.Globals.Get("maxResources").Number;
        Debug.Log(MaxResources);

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
