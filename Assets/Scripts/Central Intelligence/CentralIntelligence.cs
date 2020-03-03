using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ssuai;


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
    private int _droneCount;
    private const int MAXDRONES = 300;

    /// <summary>
    /// Different types of drones CI is capable of building
    /// </summary>
    /// 
    List<Drone> _drones = new List<Drone>();

    private int _droneID = 0;

    enum DroneType
    {
        Worker,
        Scout,
        Tank
    }
    DroneType drone = DroneType.Worker;

    #region UtilityAI

    private Action[] _actions;

    //the action that has been selected
    private Action _selectedAction;

    //The number of actions the AI is capable of doimg.
    private static int NUMOFACTIONS = 2;

    float _timeOfLastAction = 0.0f;

    /// <summary>
    /// Seconds between each time the AI tries to reselect an action
    /// </summary>
    private static int AIDECISIONTIME = 4;

    #endregion UtilityAI


    void Awake()
    {
        if(_droneCount != 0)
            _droneCount = 0;
        _behaviorTree = GetComponent<BehaviorTree>();
        // SetUpTreeFromCode();
        //_behaviorTree.SetTimer();
        _actions = new Action[NUMOFACTIONS];

        //Contains the types of resources and the amounts the CI has of them
        Resources = new Dictionary<string, int>();
        Resources.Add("metal", 100);
        Resources.Add("crystal", 100);
    }

    // Update is called once per frame
    void Update()
    {
        //build a drone
        if (Input.GetKeyDown("b"))
        {
            BuildDrone(drone);
        }
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
    /// Creates a behavior tree from a file.
    /// </summary>
    public bool SetUpBehaviorTreeFromFile()
    {
        return false;
    }

    ///<summary>
    ///CI makes drones and gives them a unique ID
    /// </summary>
    void BuildDrone(DroneType droneType)
    {
        if(_droneCount < MAXDRONES)
        {
            Drone drone = Instantiate(_dronePrefab).GetComponent<Drone>();
            _drones.Add(drone);
            Debug.Log("Created drone with ID " + drone.ID);
            _droneCount++;
        }
    }

    private void _selectAction()
    {
        Action currentAction= _selectedAction;         //the currently best action at this point in the loop
        float currentUtility = 0.0f;   //the utility of the currently best action

        //go through all actions, choose the one with the highest utility
        foreach (Action action in _actions)
        {
            float utility = action.GetUtility();
            if (utility > currentUtility)
            {
                currentAction = action;
                currentUtility = utility;
            }
        }

        _selectedAction = currentAction;
    }

    private void TestUtilityBuildDrone()
    {
        Resources["metal"] -= 10;
        Resources["crystal"] -= 8;
        Debug.Log("Built drone.");
    }
}


