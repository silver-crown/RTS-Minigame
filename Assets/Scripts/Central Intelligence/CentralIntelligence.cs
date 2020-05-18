using MoonSharp.Interpreter;
using RTS.Test;
using ssuai;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central Intelligence system that controls the drone actions
/// </summary>
[RequireComponent(typeof(Inventory))]
public class CentralIntelligence : MonoBehaviour
{
    /// <summary>
    /// List of groups(which is a list of drones)
    /// </summary>
    public List<GroupLeader> groups = new List<GroupLeader>();
    int lastGroupID = 0;
    public int maxGroups = 20;

    /// <summary>
    /// The prefab to use when instatiating new drones.
    /// </summary>
    [SerializeField] GameObject _dronePrefab = null;

    [Tooltip("file path for the Lua file that gives the utility function for gatherResourceAmount")]
    [SerializeField] string _gatherAmountPath;
    [Tooltip("equivalent for buildWorkerNumber")]
    [SerializeField] string _buildDroneNumberPath;
    [SerializeField] string _constructGroupPath;
    [SerializeField] string _buildFactoryNumber;

    [SerializeField] public GameObject FactoryPrefab;

    /// <summary>
    /// All drones under CI's control.
    /// </summary>
    public List<Drone> Drones { get; protected set; } = new List<Drone>();

    /// <summary>
    /// Maximum number of group members one group can have
    /// </summary>
    private const int MaxGroupSize = 20;

    /// <summary>
    /// Drones of each type under the CI's control.
    /// </summary>
    public Dictionary<string, int> DroneTypeCount { get; protected set; }

    /// <summary>
    /// Maps chunks to the last time the chunk was scouted.
    /// </summary>
    public Dictionary<Vector2Int, float> LastTimeChunkWasScouted { get; protected set; }

    /// <summary>
    /// Stores the resources the central intellegence has access to
    /// </summary>
    public Inventory Inventory { get; protected set; }

    /// <summary>
    /// Total number of drones present in the army
    /// </summary>
    public int DroneCount { get => Drones.Count; }

    /// <summary>
    /// Max amount of drones CI can create
    /// </summary>
    public const int MaxDrones = 300;

    #region UtilityAI

    private List<UtilityAction> _actions;

    //the action that has been selected
    private UtilityAction _selectedAction;

    /// <summary>
    /// The number of actions the AI is capable of doing in total. NOTE, when you add more actions this needs to be increased.
    /// </summary>
    private const int NUMOFACTIONS = 6;

    float _timeOfLastAction = 0.0f;

    /// <summary>
    /// Seconds between each time the AI tries to reselect an action
    /// </summary>
    private const int AIDECISIONTIME = 4;

    #endregion UtilityAI

    void Awake() {
        DroneTypeCount = new Dictionary<string, int>();
        LastTimeChunkWasScouted = new Dictionary<Vector2Int, float>();

        //Contains the types of resources and the amounts the CI has of them
        Inventory = GetComponent<Inventory>();
        var gatherMetal = ScriptableObject.CreateInstance<CIGatherResource>();
        gatherMetal.Init("Metal");
        var gatherCrystal = ScriptableObject.CreateInstance<CIGatherResource>();
        gatherCrystal.Init("Crystal");
        var buildDrone = ScriptableObject.CreateInstance<CIBuildWorker>();
        var buildFactory = ScriptableObject.CreateInstance<CIBuildFactory>();

        _actions = new List<UtilityAction>();

        //set up actions
        ///<summary>Gathering Metal</summary>
        _actions.Add(new UtilityAction(
            new List<Factor> { new ResourceAmount(this, "Metal", ReadUtilityFunctionFromFile(_gatherAmountPath)) },
            () => { gatherMetal.Tick(gameObject); }, "GatherMetal"));
        ///<summary>Gathering Crystal</summary>
        _actions.Add(new UtilityAction(
            new List<Factor> { new ResourceAmount(this, "Crystal", ReadUtilityFunctionFromFile(_gatherAmountPath)) },
            () => { gatherCrystal.Tick(gameObject); }, "GatherCrystal"));
        ///<summary>Drone building </summary>
        _actions.Add(new UtilityAction(
            new List<Factor> { new DroneNumber(this, ReadUtilityFunctionFromFile(_buildDroneNumberPath)) },
            () => { buildDrone.Tick(gameObject); },"BuildDrone"));
        ///<summary>Need Factory</summary>
        _actions.Add(new UtilityAction(
            new List<Factor> { new FactoryNumber(this, ReadUtilityFunctionFromFile(_buildFactoryNumber)) },
            () => { buildFactory.Tick(gameObject); }, "BuildFactory"));
    }

    private void Start()
    {
        BBInput.AddOnAxisPressed("TestButton", AddTestGroup);
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
        var dronesStart = dronesStartDefault.DoFile("Setup.DronesStartDefault").Table;
        
        Debug.Log("Central Intelligence: Adding starting resources...", this);
        foreach (var type in dronesStart.Get("_resources").Table.Pairs)
        {
            Debug.Log("\t" + type.Key.String + " : " + (int)type.Value.Number, this);
            Inventory.Deposit(type.Key.String, (int)type.Value.Number);
        }

        SelectAction();
    }

    // Update is called once per frame
    void Update()
    {
        //if enough time has passed since last time do AI decision making
        if (Time.time >= _timeOfLastAction+AIDECISIONTIME)
        {
            SelectAction();

            //tick selected action
            _selectedAction.MyAction.Invoke();
            Debug.Log("CI Does: " + _selectedAction.Name + ", Utility was " + _selectedAction.GetUtility());

            _timeOfLastAction = Time.time;
        }
    }

    /// <summary>
    /// Adds a drone to the CI's hivemind.
    /// </summary>
    /// <param name="drone">The drone to add.</param>
    public void AddDrone(Drone drone)
    {
        Drones.Add(drone);
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

    /// <summary>
    /// Finds the action with the highest utility
    /// </summary>
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

    /// <summary>
    /// Creates a group of drones with a group leader
    /// </summary>
    /// <param name="type">Type of group to create</param>
    public void CreateDroneGroup(GroupLeader.GroupType type)
    {
        // pick out some drones depending on what type of group you want to make
        // Give them a unique group ID and a leader
        // Group script does the rest
        Drone[] groupMember = FindObjectsOfType(typeof(Drone)) as Drone[];
        int leader = 0;
        switch (type)
        {
            //Assault group, for combat scenarios
            case GroupLeader.GroupType.Assault:
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
            case GroupLeader.GroupType.Mining:
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
            case GroupLeader.GroupType.Mixed:
                {

                }
                break;
            case GroupLeader.GroupType.Scouting:
                {

                }
                break;
            case GroupLeader.GroupType.Defense:
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
    private string ReadUtilityFunctionFromFile(string filepath)
    {
        Script script = new Script();
        var table = script.DoFile(filepath).Table;
        var memes = (string)table.Get("_utilityFunction").String;
        Debug.Log(memes);
        return (string)table.Get("_utilityFunction").String;
    }
        
    /// <summary>
    /// Create the drone groups
    /// </summary>
    public void CreateGroup()
    {
        List<Drone> groupedDrones = new List<Drone>();

        ++lastGroupID;
    }
    /// <summary>
    /// assign drones to groups, this is where utility happens and drones are added to the list based on their usability 
    /// </summary>
    /// <returns></returns>
    private List<Drone> AddDroneToGroup()
    {
        List<Drone> groupedDrones = new List<Drone>();

        //Go through the drones, construct the group based on your current needs 
        //so yeah, utility AI

        return groupedDrones;
    }

    /// <summary>
    /// Returns a Drone that has a given ID
    /// </summary>
    /// <param name="ID">ID in the drones List<> that is to be retrived</param>
    /// <returns></returns>
    public Drone GetDrone( int ID)
    {
        for (int i = 0; i<= Drones.Count; i++)
        {
            if(Drones[i].ID == ID)
            {
                return Drones[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Get all the drone groups of a particular type
    /// </summary>
    /// <param name="groupType"></param>
    /// <returns></returns>
    public List<GroupLeader> GetFighterGroups()
    {
        List<GroupLeader> fighterGroups = new List<GroupLeader>();
        foreach (GroupLeader group in groups)
        {
            if (group.groupType == GroupLeader.GroupType.Fighter)
            {
                fighterGroups.Add(group);
            }
        }
        return fighterGroups;
    }
    public List<GroupLeader> GetDroneGroupsByType(GroupLeader.GroupType groupType)
    {

        List<GroupLeader> tempGroups = new List<GroupLeader>();

        foreach(GroupLeader group in groups)
        {
            if (group.groupType == groupType)
            {
                tempGroups.Add(group);
            }
        }
        return tempGroups;
    }

    #region GroupTests
    /// <summary>
    /// Adds a test group to the army
    /// </summary>
    /// <returns></returns>
    public void AddTestGroup() 
    {
        Debug.Log("Adding test group");
        Drone[] tempDrones = FindObjectsOfType(typeof(Drone)) as Drone[];
        ///<summary>Add every drone on the map to the CI's drojne list</summary>
        foreach(var drones in tempDrones) 
        {
            Drones.Add(drones);
        }
        ///<summary>Make the initial drone into a group leader, assign it the newest group id</summary>
        Drones[0].gameObject.AddComponent<GroupLeader>().groupID = ++lastGroupID;
        Drones[0].leaderStatus = true;
        Drones[0].gameObject.AddComponent<ListenToChannel>();
        Drones[0].gameObject.GetComponent<ListenToChannel>().Init(EventManager.MessageChannel.groupChannel);
        ///<summary>Assign the members of the test group </summary>
        for(int i = 0; i <= MaxGroupSize; i++) 
        {
            ///<summary>Make sure it can't make groups that are too large</summary>
            if(i >= MaxGroupSize) 
            {
                break;
            }
            ///<summary>Make sure it can't extend the list of drones the CI currently possesses</summary>
            if(i > 0)
            {
                if (i <= Drones.Count) 
                {
                    Drones[i].gameObject.AddComponent<ListenToChannel>();
                    Drones[i].groupID = lastGroupID;
                    Drones[0].gameObject.GetComponent<GroupLeader>().groupMembers.Add(Drones[i]);
                }
            }
        }
        ///<summary>Add the test group to the group list</summary>
        groups.Add(Drones[0].GetComponent<GroupLeader>());
        Debug.Log("Test group was added");
    }
    /// <summary>
    /// Highlight Test group and sends a message to the test group
    /// </summary>
    public void SendToTestGroup(string s) 
    {
        Debug.Log("Message was sent to test group");
        for(int i = 0; i<= groups.Count; i++) 
        {
            groups[i].HighlightGroup();
            GetComponent<SendMessageToChannel>().Send(s, EventManager.MessageChannel.groupChannel, groups[i].groupID);
        }
    }
    #endregion
}


