using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BehaviorTree))]
public class CentralIntelligence : MonoBehaviour
{
    /// <summary>
    /// CentralIntelligene's behavior tree 
    /// </summary>
    private BehaviorTree _behaviorTree;

    private int _crystals;
    private int _metals;
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
    // Need a counter for how many drones are doing what type of actions (Logging this could aslo help for
    // adding learning to the AI later).

    // Start is called before the first frame update
    void Start()
    {
        if(_droneCount != 0)
            _droneCount = 0;
        _behaviorTree = GetComponent<BehaviorTree>();
        SetUpTreeFromCode();
        _behaviorTree.SetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Creates a behavior tree for the Centreal Intelligence base on a pre defined code
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
    void BuildDrone(DroneType drone)
    {
        if(_droneCount > MAXDRONES)
        {
            Drone d;
            switch (drone)
            {
                case (DroneType.Worker):
                    //Create a new drone, and insert it into the list of drones
                    Instantiate(d = new Drone(_droneID++));
                    _drones.Insert(d.ID, d);
                    Debug.Log("Created drone with ID " + _droneID);
                    break;
                case (DroneType.Scout):
                    Instantiate(d = new Drone(_droneID++));
                    _drones.Insert(d.ID, d);
                    Debug.Log("Created drone with ID " + _droneID);
                    break;
                case (DroneType.Tank):
                    Instantiate(d = new Drone(_droneID++));
                    _drones.Insert(d.ID, d);
                    Debug.Log("Created drone with ID " + _droneID);
                    break;
            }
            _droneCount++;
        }
    }
}


