using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using RTS;

public class Group : MonoBehaviour
{
    /// <summary>
    /// the unique ID of the group
    /// </summary>
    public int groupID;
    /// <summary>
    /// Number of members of the group in question
    /// </summary>
    public int groupSize;
    int lastMessage;
    /// <summary>
    /// String used for listening to messages contained in the message list
    /// </summary>
    string[] message;
    [SerializeField] Drone _leader;
    /// <summary>
    ///Leader status for the drone in question
    /// </summary>
    public bool leaderStatus;
    /// <summary>
    /// All the group members in the group
    /// </summary>
    public List<GameObject> groupMembers = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();


    // The units the group is divided into
    private List<Drone> _alphaDrones = new List<Drone>();
    private List<Drone> _bravoDrones = new List<Drone>();
    private List<Drone> _charlieDrones = new List<Drone>();
    private List<Drone> _deltaDrones = new List<Drone>();
    bool _listening;

    public Bounds targetBounds;
    public float targetRadius;
    /// <summary>
    /// A list of all messages currently sent to the group
    /// </summary>
    public List<string> groupMessageList = new List<string>();

    // Update is called once per frame
    void Update()
    {
        groupSize = groupMembers.Count;
        if (!_listening)
        {
            if (leaderStatus)
            {
                LeaderStartListening();
            }
        }
    }
    /// <summary>
    /// Construct the list of enemies visible to the group
    /// </summary>
    void ConstructEnemyList()
    {
        //through each group member
        for (int i = 0; i <= groupMembers.Count; i++)
        {
            //iterate though the visible enemies list of each member
            for (int j = 0; j <= groupMembers[i].GetComponent<Actor>().VisibleEnemies.Count; j++)
            {
                //add them to the enemy list of the group if they're not already in it 
                if (!enemyList.Contains(groupMembers[i].GetComponent<Actor>().VisibleEnemies[j]))
                {
                    enemyList.Add(groupMembers[i].GetComponent<Actor>().VisibleEnemies[j]);
                }
            }
        }
    }
    /// <summary>
    /// Make the leader listen in on the channel
    /// </summary>
    void LeaderStartListening()
    {
        if (GetComponent<Drone>().leaderStatus)
        {
            //Listen in on the messages sent to the group channel dictionary
            for (int i = 0; i <= message.Length; i++)
            {
                lastMessage = i;
                GetComponent<ListenToChannel>().ListenToMessage("This is a message that will be added to the message list once someone sends it.");
            }
        }
    }
    /// <summary>
    /// assign a new leader to the group, presumably because the previous one is dead.
    /// </summary>
    void AssignNewLeader()
    {
        //Leader (or the whole group, depending on how I want to do this) assigns a new group leader.
    }
    void ConstructGroup()
    {
        //find each object with a Group on it
        //get the ones with your ID number in it
        //add self and rest to group list in that group
        Drone[] groupMember = FindObjectsOfType(typeof(Drone)) as Drone[];
        //no this doesn't work
        Debug.Log("Found " + groupMember.Length + " instances with Group attached");

        for (int i = 0; i <= groupMember.Length; i++)
        {
            if (groupMember[i].groupID == groupID)
            {
                groupMembers.Add(groupMember[i].gameObject);
            }
        }
    }
    /// <summary>
    /// Create the radius of all the targets the group members are currently assigned to
    /// this is used for flanking behaviour
    /// </summary>
    public void CreateTargetBounds()
    {
        //gusto 1 is north (positive y), gusto 2 is east (positive x), gusto 3 is south(negative y), gusto 4 is west(negative x)
        Vector4 fourWayTemp = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        Vector3 origin = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 longestYard = new Vector3(0.0f, 0.0f, 0.0f);
        float radius = 0.0f;
        float dist = 0.0f;
        List <Transform> targets = new List<Transform>();
        for (int i = 0; i <= groupMembers.Count; i++)
        {
            //for every group member, take their targets's positions
            targets.Add(groupMembers[i].GetComponent<Actor>().Target.transform);
        }
        // and produce a radius based on this
        //Get the origin by first getting the 4 outermost points
        //^^^retard way
        for(int i = 0; i <= targets.Count; i++)
        {
            //North (positive y)
            if(targets[i].position.x > fourWayTemp.x)
            {
                fourWayTemp.x = targets[i].position.x;
            }
            //East (positive x)
            if (targets[i].position.y > fourWayTemp.y)
            {
                fourWayTemp.y = targets[i].position.y;
            }
            //South (negative y)
            if (targets[i].position.x < fourWayTemp.z)
            {
                fourWayTemp.z = targets[i].position.x;
            }
            //west (negative x)
            if (targets[i].position.y < fourWayTemp.w)
            {
                fourWayTemp.w = targets[i].position.y;
            }
            //get the longest distance from origin 
            radius = Vector3.Distance(targets[i].position, longestYard);
            if(radius > dist)
            {
                longestYard = targets[i].position;
            }
        }
        //x position = gusto2 - gusto4/2
        //y position = gusto1 - gusto3/2
        origin.x = fourWayTemp.y - (fourWayTemp.w / 2);
        origin.y = fourWayTemp.x - (fourWayTemp.z / 2);

        //create an overlapping sphere of all the target 
        Collider[] myColliders = Physics.OverlapSphere(origin, Vector3.Distance(origin, longestYard));
        Bounds myBounds = new Bounds(transform.position, Vector3.zero);
        foreach(Collider nextCollider in myColliders)
        {
            myBounds.Encapsulate(nextCollider.bounds);
        }
        targetBounds = myBounds;
        targetRadius = radius;
    }
    /// <summary>
    /// Assign drones to the alpha unit, the primary fighting force of the group. Everyone starts in this unit and are diverged
    /// into sub-units later as needed
    /// </summary>
    /// <returns></returns>
    private void AssignAlphaDrones()
    {
        //Assign everyone as alpha to begin with
        for (int i = 0; i <= groupMembers.Count; i++)
        {
            //assign the drone as an alpha unit
            groupMembers[i].GetComponent<Drone>().myUnit = Drone.GroupUnit.Alpha;
            //add them to the list of alphas
            _alphaDrones.Add(groupMembers[i].GetComponent<Drone>());
        }
    }
    /// <summary>
    /// Assign drones to the bravo unit, these are the second-most powerful force of the group, and the primary backup unit for flanking
    /// </summary>
    /// <returns></returns>
    private void AssignBravoDrones()
    {
        //get the size of the group(alpha unit)
        //divide it by 3
        int alphas = _alphaDrones.Count;
        int third = alphas / 3;
        
        //1/3 should be assigned as Bravo drones, the rest remain untouched
        for (int i = 0; i <= third; i++)
        {
            //go through all of them and find the weakest guy, asign him to bravo unit
            for (int j = 0; j <= _alphaDrones.Count; j++)
            {
                double lowestGuy = _alphaDrones.Min(_alphaDrones => _alphaDrones.powerLevel);
                //go through all the alphas, if they're the lowestGuy, add them to bravos and remove them from alphas
                for (int k = 0; k <= _alphaDrones.Count; k++)
                {
                    if (_alphaDrones[k].powerLevel == lowestGuy)
                    {
                        _bravoDrones.Add(_alphaDrones[k]);
                        _alphaDrones.Remove(_alphaDrones[k]);
                        break;
                    }
                }
            }
        }
    } 
    /// <summary>
    /// Assign drones to the charlie unit, the second weakest unit of the group, they provide support for bravo unit
    /// </summary>
    /// <returns></returns>
    private void AssignCharlieDrones()
    {
        //get the size of the Bravo unit
        //divide it by 3
        int bravos = _bravoDrones.Count;
        int third = bravos / 3;
      
        //1/3 should be assigned as Charlie drones, the rest remain untouched
        for (int i = 0; i <= third; i++)
        {
            //go through all of them and find the weakest guy, asign him to bravo unit
            for (int j = 0; j <= _bravoDrones.Count; j++)
            {
                double lowestGuy = _bravoDrones.Min(_bravoDrones => _bravoDrones.powerLevel);
                //go through all the alphas, if they're the lowestGuy, add them to bravos and remove them from alphas
                for (int k = 0; k <= _bravoDrones.Count; k++)
                {
                    if (_bravoDrones[k].powerLevel == lowestGuy)
                    {
                        _charlieDrones.Add(_bravoDrones[k]);
                        _bravoDrones.Remove(_bravoDrones[k]);
                        break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Assign drones to the delta unit, the smallest and weakest unit of the group, they're dependant on the others for success in maneuvers
    /// </summary>
    /// <returns></returns>
    private void AssignDeltaDrones()
    {
        //get the size of the charlie unit
        //divide it by 3
        int charlies = _charlieDrones.Count;
        int third = charlies / 3;

        //1/3 should be assigned as Deltas drones, the rest remain untouched
        for (int i = 0; i <= third; i++)
        {
            //go through all of them and find the weakest guy, asign him to bravo unit
            for (int j = 0; j <= _charlieDrones.Count; j++)
            {
                double lowestGuy = _charlieDrones.Min(_charlieDrones => _charlieDrones.powerLevel);
                //go through all the alphas, if they're the lowestGuy, add them to bravos and remove them from alphas
                for (int k = 0; k <= _charlieDrones.Count; k++)
                {
                    if (_charlieDrones[k].powerLevel == lowestGuy)
                    {
                        _deltaDrones.Add(_charlieDrones[k]);
                        _charlieDrones.Remove(_charlieDrones[k]);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// For use in two-way flanking maneuvers, divide the army up into different tactical units up to four times
    /// </summary>
    /// <param name="divisions"></param>
    public void DivideArmy(int divisions)
    {
        for(int i = 0; i<= groupMembers.Count; i++)
        {
            groupMembers[i].GetComponent<Drone>().CalculatePowerLevel();
        }
        switch (divisions)
        {
            case (2):
                AssignAlphaDrones();
                AssignBravoDrones();
                break;
            case (3):
                AssignAlphaDrones();
                AssignBravoDrones();
                AssignCharlieDrones();
                break;
            case (4):
                AssignAlphaDrones();
                AssignBravoDrones();
                AssignCharlieDrones();
                AssignDeltaDrones();
                break;
            default:
                Debug.Log("Tried dividing the group up more than what's allowed");
                break;
        }
    }

    /// <summary>
    /// return the group unit specified in the argument
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public List<GameObject> GetGroupUnits(Drone.GroupUnit unit)
    {
        List<GameObject> drones = new List<GameObject>();

        switch (unit)
        {
            case Drone.GroupUnit.Alpha:
                for(int i = 0; i<= groupMembers.Count; i++)
                {
                    if(groupMembers[i].GetComponent<Drone>().myUnit == Drone.GroupUnit.Alpha)
                    {
                        drones.Add(groupMembers[i]);
                    }
                }
                break;
            case Drone.GroupUnit.Bravo:
                for (int i = 0; i <= groupMembers.Count; i++)
                {
                    if (groupMembers[i].GetComponent<Drone>().myUnit == Drone.GroupUnit.Bravo)
                    {
                        drones.Add(groupMembers[i]);
                    }
                }
                break;
            case Drone.GroupUnit.Charlie:
                for (int i = 0; i <= groupMembers.Count; i++)
                {
                    if (groupMembers[i].GetComponent<Drone>().myUnit == Drone.GroupUnit.Charlie)
                    {
                        drones.Add(groupMembers[i]);
                    }
                }
                break;
            case Drone.GroupUnit.Delta:
                for (int i = 0; i <= groupMembers.Count; i++)
                {
                    if (groupMembers[i].GetComponent<Drone>().myUnit == Drone.GroupUnit.Delta)
                    {
                        drones.Add(groupMembers[i]);
                    }
                }
                break;
        }
        return drones;
    }
}


