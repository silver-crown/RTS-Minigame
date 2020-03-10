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


    public Dictionary<Resource.Type, int> Resources { get; protected set; }

    /// <summary>
    /// Total number of drones present in the army
    /// </summary>
    public int DroneCount { get; protected set; }
    public const int MAXDRONES = 300;
    //TODO Make separate counters for different types of drones

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
        if(DroneCount != 0)
            DroneCount = 0;
        _behaviorTree = GetComponent<BehaviorTree>();
        // SetUpTreeFromCode();
        //_behaviorTree.SetTimer();
        _actions = new Action[NUMOFACTIONS];

        //Contains the types of resources and the amounts the CI has of them
        Resources = new Dictionary<Resource.Type, int>();
        Resources.Add(Resource.Type.METAL, 100);
        Resources.Add(Resource.Type.CRYSTAL, 100);

        //set up actions
        _actions[0] = new Action(new List<Factor> { new GatherResourceAmount(this, Resource.Type.METAL) }, new CIGatherMetal());
        _actions[1] = new Action(new List<Factor> { new GatherResourceAmount(this, Resource.Type.CRYSTAL) }, new CIGatherCrystal());
        _actions[2] = new Action(new List<Factor> { new WorkerNumber(this) }, new CIBuildWorker());

        //run selectAction
        _selectAction();
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
        if(DroneCount < MAXDRONES)
        {
            Drone drone = Instantiate(_dronePrefab).GetComponent<Drone>();
            _drones.Add(drone);
            Debug.Log("Created drone with ID " + drone.ID);
            DroneCount++;
        }
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
        Resources[Resource.Type.METAL] -= 10;
        Resources[Resource.Type.CRYSTAL] -= 8;
        DroneCount++;
        Debug.Log("Built drone. Metal: " +  Resources[Resource.Type.METAL] + "Crystal: " + Resources[Resource.Type.CRYSTAL]);
    }

    public void TestGatherMetal()
    {
        Resources[Resource.Type.METAL] += 10;
    }

    public void TestGatherCrystal()
    {
        Resources[Resource.Type.CRYSTAL] += 10;
    }

    
}


