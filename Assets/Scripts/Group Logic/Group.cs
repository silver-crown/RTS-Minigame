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
    private Bounds CreateTargetBounds()
    {
        //gusto 1 is north (positive y), gusto 2 is east (positive x), gusto 3 is south(negative y), gusto 4 is west(negative x)
        Vector4 fourWayTemp = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
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
            float temp = Vector3.Distance(targets[i].position, longestYard);
            if(temp > dist)
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
        return myBounds;
    }
    /// <summary>
    /// Produces four corners from bounds passed as an argument
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    private List<Vector3> ProduceCornersOfBounds(Bounds bounds)
    {
        //take the group's target radius, and produces four corner points
        List<Vector3> corners = new List<Vector3>();
        float width = bounds.size.x;
        float height = bounds.size.y;

        Vector3 topRight = bounds.center;
        Vector3 topLeft = bounds.center;
        Vector3 bottomRight = bounds.center;
        Vector3 bottomLeft = bounds.center;

        //Set the top right of the bounds
        topRight.x += width / 2;
        topRight.y += height / 2;
        //Set the top left of the bounds
        topLeft.x = width / 2;
        topLeft.y = height / 2;
        //Set the bottom right of the bounds
        bottomRight.x = width / 2;
        bottomRight.y = height / 2;
        //Set the bottom left of the bounds
        bottomLeft.x = width / 2;
        bottomLeft.y = height / 2;

        corners.Add(topRight);
        corners.Add(topLeft);
        corners.Add(bottomRight);
        corners.Add(bottomLeft);

        return corners;
    }
    /// <summary>
    /// Produces four sides from bounds pass as an argument
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    private List<Vector3> ProduceSidesOfBounds(Bounds bounds)
    {

        float width = bounds.size.x;
        float height = bounds.size.y;

        List<Vector3> sides = new List<Vector3>();

        Vector3 top = bounds.center;
        Vector3 bottom = bounds.center;
        Vector3 right = bounds.center;
        Vector3 left = bounds.center;

        top.y += height / 2;
        bottom.y += height / 2;
        right.x += width / 2;
        left.x += width / 2;

        sides.Add(top);
        sides.Add(bottom);
        sides.Add(right);
        sides.Add(left);

        return sides;
    }
    /// <summary>
    /// Gets the sides of the group target's bounds
    /// 0 is top, 1 is bottom, 2 is right, 3 is left
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetGroupTargetSides()
    {
        return ProduceSidesOfBounds(CreateTargetBounds());
    }
    /// <summary>
    /// Gets the corners of the group's target bounds
    /// 0 is top right, 1 is top left, 2 is bottom right, 3 is bottom left
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetGroupTargetCorners()
    {
        return ProduceCornersOfBounds(CreateTargetBounds());
    }
}


