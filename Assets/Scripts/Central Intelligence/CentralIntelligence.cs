using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ssuai;
using RTS.Test;
using MoonSharp.Interpreter;
using Bbbt;
using System;

[RequireComponent(typeof(BehaviorTree))]
public class CentralIntelligence : MonoBehaviour
{
    /// <summary>
    /// List of groups(which is a list of drones)
    /// </summary>
   private List<List<Drone>> _group = new List<List<Drone>>();
    int lastGroupID = 0;
    /// <summary>
    /// The prefab to use when instatiating new drones.
    /// </summary>
    [SerializeField] GameObject _dronePrefab = null;


    [Tooltip("file path for the Lua file that gives the utility function for gatherResourceAmount")]
    [SerializeField] string gatherAmountScriptPath;
    [Tooltip("equivalent for buildWorkerNumber")]
    [SerializeField] string buildDroneNumberPath;

    /// <summary>
    /// CentralIntelligene's behavior tree 
    /// </summary>
    private BehaviorTree _behaviorTree;

    /// <summary>
    /// All drones under CI's control.
    /// </summary>
    private List<Drone> _drones = new List<Drone>();

    /// <summary>
    /// Resources in the CI's possession.
    /// </summary>
    public Dictionary<string, int> Resources { get; protected set; }

    /// <summary>
    /// Drones of each type under the CI's control.
    /// </summary>
    public Dictionary<string, int> DroneTypeCount { get; protected set; }

    /// <summary>
    /// Maps chunks to the last time the chunk was scouted.
    /// </summary>
    public Dictionary<Vector2Int, float> LastTimeChunkWasScouted { get; protected set; }

    /// <summary>
    /// Total number of drones present in the army
    /// </summary>
    public int DroneCount { get => _drones.Count; }

    public const int MAXDRONES = 300;

    enum DroneType
    {
        Worker,
        Scout,
        Tank
    }
    DroneType drone = DroneType.Worker;


    #region UtilityAI

    private UtilityAction[] _actions;

    //the action that has been selected
    private UtilityAction _selectedAction;

    //The number of actions the AI is capable of doing in total
    private const int NUMOFACTIONS = 3;

    float _timeOfLastAction = 0.0f;

    /// <summary>
    /// Seconds between each time the AI tries to reselect an action
    /// </summary>
    private const int AIDECISIONTIME = 4;

    #endregion UtilityAI


    void Awake()
    {
        DroneTypeCount = new Dictionary<string, int>();
        LastTimeChunkWasScouted = new Dictionary<Vector2Int, float>();
        _behaviorTree = GetComponent<BehaviorTree>();
        // SetUpTreeFromCode();
        //_behaviorTree.SetTimer();
        _actions = new UtilityAction[NUMOFACTIONS];
        //Contains the types of resources and the amounts the CI has of them
        Resources = new Dictionary<string, int>();

        var gatherMetal = ScriptableObject.CreateInstance<CIGatherMetal>();
        var gatherCrystal = ScriptableObject.CreateInstance<CIGatherCrystal>();
        var buildDrone = ScriptableObject.CreateInstance<CIBuildDrone>();

        //set up actions
        _actions[0] = new UtilityAction(
            new List<Factor> { new ResourceAmount(this, "Metal", readUtilityFunctionFromFile(gatherAmountScriptPath)) },
            () => { gatherMetal.Tick(gameObject); });
        _actions[1] = new UtilityAction(
            new List<Factor> { new ResourceAmount(this, "Crystal", readUtilityFunctionFromFile(gatherAmountScriptPath)) },
            () => { gatherCrystal.Tick(gameObject); });
        _actions[2] = new UtilityAction(
            new List<Factor> { new DroneNumber(this, readUtilityFunctionFromFile(buildDroneNumberPath)) },
            () => { buildDrone.Tick(gameObject); });
    }

    private void Start()
    {
        // Populate the DroneTypeCount dictionary.
        foreach (string type in WorldInfo.DroneTypes)
        {
            if (!DroneTypeCount.ContainsKey(type))
            {
                DroneTypeCount.Add(type, 0);
            }
        }

        // Populate the LastTimeChunkWasScouted dictionary.
        foreach (var chunk in WorldInfo.Chunks)
        {
            LastTimeChunkWasScouted.Add(chunk, 0.0f);
        }

        // Build the starting drones and give starting resources.
        Debug.Log("Central Intelligence: Adding starting drones...", this);
        Script dronesStartDefault = new Script();
        var dronesStart = dronesStartDefault.DoFile("Setup/DronesStartDefault").Table;
        foreach (var type in dronesStart.Get("_drones").Table.Pairs)
        {
            int count = (int)type.Value.Number;
            for (int i = 0; i < count; i++)
            {
                GetComponent<DroneTestFactory>().BuildDroneForFree(type.Key.String);
            }
        }
        Debug.Log("Central Intelligence: Adding starting resources...", this);
        foreach (var type in dronesStart.Get("_resources").Table.Pairs)
        {
            Debug.Log("\t" + type.Key.String + " : " + (int)type.Value.Number, this);
            AddResource(type.Key.String, (int)type.Value.Number);
        }

        SelectAction();
    }

    // Update is called once per frame
    void Update()
    {
        //test the worker channel
        if (Input.GetKeyDown("w"))
        {
            EventManager.TriggerEvent("Testing Worker Channel", EventManager.MessageChannel.workerChannel);
        }
        //test the private channel
        if (Input.GetKeyDown("p"))
        {
            EventManager.TriggerEvent("Testing Private Channel", EventManager.MessageChannel.privateChannel, 0);
        }
        // Test building combat drone
        if (Input.GetKeyDown(KeyCode.C))
        {
            GetComponent<DroneTestFactory>().BuildDroneForFree("CombatDrone");
        }

        //test the group channels
        if (Input.GetKeyDown("k"))
        {
            string m = "this is a test message to group 0";
            //add the command to the group's list of messages
            GetComponent<Group>().groupMessageList.Add(m);
            //Fire off the event
            //!!!NOTE!!!
            //The group gets the message into it's list and then does a bunch of stuff, but there's no guarantee
            //They're able to listen for the message before the event is triggered if it's done like this. 
            //Sending and triggering should happen seperately
            EventManager.TriggerEvent(m, EventManager.MessageChannel.groupChannel, 0);
        }

        //if enough time has passed since last time do AI decision making
        if (Time.time >= _timeOfLastAction+AIDECISIONTIME)
        {
            SelectAction();

            //tick selected action
            _selectedAction.Behaviour.Invoke();

            _timeOfLastAction = Time.time;
        }
    }

    /// <summary>
    /// Creates a behavior tree for the Central Intelligence base on a predefined code
    /// </summary>
    public void SetUpTreeFromCode()
    {
        Selector rootNode = new Selector();


        GatherResources gatherResources = new GatherResources();
        

        // SendMessageToDronesBehavior collectResources = new SendMessageToDronesBehavior();
        // rootNode.AddChild(collectResources);
        
        _behaviorTree.SetRootNode(rootNode); // Creating the root node of the tree 
    }

    /// <summary>
    /// Adds a drone to the CI's hivemind.
    /// </summary>
    /// <param name="drone"></param>
    public void AddDrone(Drone drone)
    {
        _drones.Add(drone);
        drone.CentralIntelligence = this;
        if (DroneTypeCount.ContainsKey(drone.Type))
        {
            DroneTypeCount[drone.Type]++;
        }
        else
        {
            DroneTypeCount[drone.Type] = 1;
        }
    }

    /// <summary>
    /// Adds a resource to the CI's resource pool.
    /// </summary>
    /// <param name="type">The type of the resource to add.</param>
    /// <param name="count">The amount to add of the resource.</param>
    public void AddResource(string type, int count)
    {
        if (Resources.ContainsKey(type))
        {
            Resources[type] += count;
        }
        else
        {
            Resources[type] = count;
        }
    }

    /// <summary>
    /// Creates a behavior tree from a file.
    /// </summary>
    public bool SetUpBehaviorTreeFromFile()
    {
        return false;
    }

    /// <summary>
    /// Updates the last time a chunk was scouted.
    /// </summary>
    /// <param name="chunk">The chunk to update.</param>
    /// <param name="time">The time it was last scouted.</param>
    public void SetLastTimeScouted(Vector2Int chunk, float time)
    {
        if (time > LastTimeChunkWasScouted[chunk])
        {
            LastTimeChunkWasScouted[chunk] = time;
        }
    }

    private void SelectAction()
    {
        UtilityAction chosenAction= _selectedAction;         //the currently best action at this point in the loop
        float chosenUtility = 0.0f;   //the utility of the currently best action

        //debug values to get information on chosen action
        int chosenIndex = 0;
        int debugIndex = 0;

        //go through all actions, choose the one with the highest utility
        foreach (UtilityAction action in _actions)
        {
            float utility = action.GetUtility();
            //Debug.Log("Utility of action " + debugIndex + ": " + utility);
            if (utility > chosenUtility)
            {
                chosenAction = action;
                chosenUtility = utility;
                chosenIndex = debugIndex;
            }
            debugIndex++;
        }

        _selectedAction = chosenAction;
    }

    public void TestBuildDrone()
    {
        AddResource("Metal", -10);
        AddResource("Crystal", -8);
    }

    public void TestGatherMetal()
    {
        AddResource("Metal", 10);
    }

    public void TestGatherCrystal()
    {
        AddResource("Crystal", 10);
    }

    enum GroupType
    {
        Assault,
        Mining,
        Mixed,
        Scouting,
        Defense,
    }
    void CreateDroneGroup(GroupType type)
    {
        //pick out some dumbass drones depending on what type of group I want to make
        //Give them a unique group ID and a leader
        //Group script does the rest
        Drone[] groupMember = FindObjectsOfType(typeof(Drone)) as Drone[];
        int leader = 0;
        switch (type)
        {
            //Assault group, for combat scenarios
            case GroupType.Assault:
                {
                    for (int i = 0; i <= groupMember.Length; i++)
                    {
                        switch (groupMember[i].Type)
                        {
                            //get the fighters
                            case ("FighterDrone"):
                                {
                                    //something something utility AI
                                    //*********************************************************
                                    //give the group member a unique group number
                                    groupMember[i].groupID = lastGroupID+1;
                                    //assign a leader based on killcount
                                    if (groupMember[i].killCount > groupMember[leader].killCount)
                                    {
                                        leader = i;
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    groupMember[leader].leaderStatus = true;
                }
                break;
            case GroupType.Mining:
                {
                    for (int i = 0; i <= groupMember.Length; i++)
                    {
                        switch (groupMember[i].Type)
                        {
                            //get the workers
                            case ("WorkerDrone"):
                                {
                                    //something something utility AI
                                    //*********************************************************
                                    //give the group member a unique group number
                                    groupMember[i].groupID = lastGroupID + 1;
                                    //assign a leader based on something
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
                break;
            case GroupType.Mixed:
                {

                }
                break;
            case GroupType.Scouting:
                {

                }
                break;
            case GroupType.Defense:
                {

                }
                break;
            default:
                break;
        }
        lastGroupID++;
    }

    /// <summary>
    /// Reads a utility function from the provided lua filepath.
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private string readUtilityFunctionFromFile(string filepath)
    {
        Script script = new Script();
        var table = script.DoFile(filepath).Table;
        return (string)table.Get("_utilityFunction").String;
    }

}


