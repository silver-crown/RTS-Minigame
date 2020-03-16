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
    public List<GameObject> _groupMembers = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();
    bool _listening;
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
        groupSize = _groupMembers.Count;
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
        for (int i = 0; i <= _groupMembers.Count; i++)
        {
            //iterate though the visible enemies list of each member
            for (int j = 0; j <= _groupMembers[i].GetComponent<Actor>().VisibleEnemies.Count; j++)
            {
                //add them to the enemy list of the group if they're not already in it 
                if (!enemyList.Contains(_groupMembers[i].GetComponent<Actor>().VisibleEnemies[j]))
                {
                    enemyList.Add(_groupMembers[i].GetComponent<Actor>().VisibleEnemies[j]);
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
                _groupMembers.Add(groupMember[i].gameObject);
            }
        }
    }
    /// <summary>
    /// Create the radius of all the targets the group members are currently assigned to
    /// this is used for flanking behaviour
    /// </summary>
    public Collider[] CreateTargetRadius()
    {
        Ray hit;
        //gusto 1 is north (positive y), gusto 2 is east (positive x), gusto 3 is south(negative y), gusto 4 is west(negative x)
        Vector4 fourWayGusto = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        Vector3 origin = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 longestYard = new Vector3(0.0f, 0.0f, 0.0f);
        float dist = 0.0f;
        List <Transform> targets = new List<Transform>();
        for (int i = 0; i <= _groupMembers.Count; i++)
        {
            //for every group member, take their targets's positions
            targets.Add(_groupMembers[i].GetComponent<Actor>().Target.transform);
        }
        // and produce a radius based on this
        //Get the origin by first getting the 4 outermost points
        //^^^retard way
        for(int i = 0; i <= targets.Count; i++)
        {
            //North (positive y)
            if(targets[i].position.x > fourWayGusto.x)
            {
                fourWayGusto.x = targets[i].position.x;
            }
            //East (positive x)
            if (targets[i].position.y > fourWayGusto.y)
            {
                fourWayGusto.y = targets[i].position.y;
            }
            //South (negative y)
            if (targets[i].position.x < fourWayGusto.z)
            {
                fourWayGusto.z = targets[i].position.x;
            }
            //west (negative x)
            if (targets[i].position.y < fourWayGusto.w)
            {
                fourWayGusto.w = targets[i].position.y;
            }
            //get the longest distance from origin 
            float temp = Vector3.Distance(targets[i].position, longestYard);
            if(temp > dist)
            {
                longestYard = targets[i].position;
            }
        }
        //x position = gusto2 - gusto4/2
        //y position = gusto1 - gusto3/2
        origin.x = fourWayGusto.y - (fourWayGusto.w / 2);
        origin.y = fourWayGusto.x - (fourWayGusto.z / 2);

        return  Physics.OverlapSphere(origin, Vector3.Distance(origin, longestYard));    
    }
}


