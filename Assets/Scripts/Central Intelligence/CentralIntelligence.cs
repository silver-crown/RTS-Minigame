using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ssuai;
using RTS.Test;
using MoonSharp.Interpreter;

[RequireComponent(typeof(BehaviorTree))]
public class CentralIntelligence : MonoBehaviour
{
    /// <summary>
    /// The prefab to use when instatiating new drones.
    /// </summary>
    [SerializeField] GameObject _dronePrefab = null;

    /// <summary>
    /// CentralIntelligene's behavior tree 
    /// </summary>
    private BehaviorTree _behaviorTree;


    public Dictionary<string, int> Resources { get; protected set; }

    /// <summary>
    /// Total number of drones present in the army
    /// </summary>
    public int DroneCount { get => _drones.Count; }
    public Dictionary<string, int> DroneTypeCount { get; protected set; }
    public const int MAXDRONES = 300;
    //TODO Make separate counters for different types of drones

    /// <summary>
    /// Different types of drones CI is capable of building
    /// </summary>
    private List<Drone> _drones;

    #region UtilityAI

    private Action[] _actions;

    //the action that has been selected
    private Action _selectedAction;

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
        _drones = new List<Drone>();
        _behaviorTree = GetComponent<BehaviorTree>();
        // SetUpTreeFromCode();
        //_behaviorTree.SetTimer();
        _actions = new Action[NUMOFACTIONS];

        //Contains the types of resources and the amounts the CI has of them
        Resources = new Dictionary<string, int>();


        //set up actions
        _actions[0] = new Action(new List<Factor> { new GatherResourceAmount(this, "Metal") }, new CIGatherMetal());
        _actions[1] = new Action(new List<Factor> { new GatherResourceAmount(this, "Crystal") }, new CIGatherCrystal());
        _actions[2] = new Action(new List<Factor> { new WorkerNumber(this) }, new CIBuildWorker());

        //run selectAction
        _selectAction();
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

        //if enough time has passed since last time do AI decision making
        if (Time.time >= _timeOfLastAction+AIDECISIONTIME)
        {
            _selectAction();

            //tick selected action
            _selectedAction.Behaviour.Tick(gameObject);

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

    private void _selectAction()
    {
        Action chosenAction= _selectedAction;         //the currently best action at this point in the loop
        float chosenUtility = 0.0f;   //the utility of the currently best action

        //debug values to get information on chosen action
        int chosenIndex = 0;
        int debugIndex = 0;

        //go through all actions, choose the one with the highest utility
        foreach (Action action in _actions)
        {
            float utility = action.GetUtility();
            Debug.Log("Utility of action " + debugIndex + ": " + utility);
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
}


