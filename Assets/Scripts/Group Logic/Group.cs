﻿using System.Collections;
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
    bool _listening;

    public Bounds targetBounds;
    public float targetRadius;
    /// <summary>
    /// A list of all messages currently sent to the group
    /// </summary>
    public List<string> groupMessageList = new List<string>();

    private void Start()
    {
        SetupMessagesToListenTo();
    }
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
        //Listen in on the messages sent to the group channel dictionary
        for(int i = 0; i <= message.Length; i++)
        {
            lastMessage = i;
            EventManager.StartListening(message[i], () => { groupMessageList.Add(message[lastMessage]);},EventManager.MessageChannel.groupChannel, groupID);
        }
    }
    /// <summary>
    /// sets up the message array and the strings it can listen for
    /// </summary>
    void SetupMessagesToListenTo()
    {
        int i = 0;
        message[i++] = "Group Frontal Assault";
        message[i++] = "Group Flanking Assault";
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
            groupMembers[i].GetComponent<Drone>().myUnit = Drone.GroupUnit.Alpha;
        }
    }
    /// <summary>
    /// Assign drones to the bravo unit, these are the second-most powerful force of the group, and the primary backup unit for flanking
    /// </summary>
    /// <returns></returns>
    private void AssignBravoDrones()
    {
        //get the size of the group(alpha unit)
        int alphas = 0;
        for (int i = 0; i <= groupMembers.Count; i++)
        {
            if (groupMembers[i].GetComponent<Drone>().myUnit == Drone.GroupUnit.Alpha)
            {
                alphas++;
            }
        }
        int third = alphas / 3;
        //divide it by 3
        //1/3 should be assigned as Bravo drones, the rest remain untouched
        //thus, find the weakest third of the alpha unit, and make them bravos.
        //how? Utility AI prolly, same way leader status will be decided.
    } 
    /// <summary>
    /// Assign drones to the charlie unit, the second weakest unit of the group, they provide support for bravo unit
    /// </summary>
    /// <returns></returns>
    private void AssignCharlieDrones()
    {
        int bravos = 0;
        for (int i = 0; i <= groupMembers.Count; i++)
        {
            if(groupMembers[i].GetComponent<Drone>().myUnit == Drone.GroupUnit.Bravo)
            {
                bravos++;
            }
        }
        int third = bravos / 3;
        //get the size of the Bravo unit
        //divide it by 3
        //1/3 should be assigned as Charlie drones, the rest remain untouched
        //thus, find the weakest third of the Bravo unit, and make them Charlies.
        //how? Utility AI prolly, same way leader status will be decided.
    }
    /// <summary>
    /// Assign drones to the delta unit, the smallest and weakest unit of the group, they're dependant on the others for success in maneuvers
    /// </summary>
    /// <returns></returns>
    private void AssignDeltaDrones()
    {
        int charlies = 0;
        for (int i = 0; i <= groupMembers.Count; i++)
        {
            if (groupMembers[i].GetComponent<Drone>().myUnit == Drone.GroupUnit.Delta)
            {
                charlies++;
            }
        }
        int third = charlies / 3;
        //get the size of the charlie unit
        //divide it by 3
        //1/3 should be assigned as Deltas drones, the rest remain untouched
        //thus, find the weakest third of the Charlie unit, and make them Deltas.
        //how? Utility AI prolly, same way leader status will be decided.
    }

    /// <summary>
    /// For use in two-way flanking maneuvers, divide the army up into different tactical units up to four times
    /// </summary>
    /// <param name="divisions"></param>
    public void DivideArmy(int divisions)
    {
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


