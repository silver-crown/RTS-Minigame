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
    BehaviorTree _behaviorTree;

    // Navigation
    NavMeshAgent _navMeshAgent;
    public GameObject _target;

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
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _behaviorTree = GetComponent<BehaviorTree>();
        ReadStatsFromFile();
        //add the channel to the channel list
        EventManager.AddPrivateChannel(_personalChannelDictionary);
        //iterate through the number of other drones, and set the ID number of the drone.
        ID = ++_lastUsedId;
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
       // _navMeshAgent.SetDestination(_target.transform.position);       
    }
    /// <summary>
    /// example on use of message listening
    /// </summary>
    void listenToSHit()
    {
       // EventManager.StartListening("get metal", FunctionThatGetsMetal, EventManager.MessageChannel.workerChannel);
    }
}
