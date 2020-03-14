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
    /// <summary>
    /// String used for listening to messages contained in the message list
    /// </summary>
    string message;
    [SerializeField] Drone _leader;
    /// <summary>
    ///Leader status for the drone in question
    /// </summary>
    public bool leaderStatus;
    /// <summary>
    /// All the group members in the group
    /// </summary>
    private List<GameObject> _groupMembers = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();
    bool _listening;
    /// <summary>
    /// A list of all messages currently sent to the group
    /// </summary>
    public List<string> groupMessageList = new List<string>();

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
        for(int i = 0; i <= groupMessageList.Count; i++)
        {
            message = groupMessageList[i];
            EventManager.StartListening(message, LeaderIssueCommand, EventManager.MessageChannel.groupChannel, groupID);
        }
    }

    /// <summary>
    /// Leader relays the message to the other drones with a proxy function 
    /// </summary>
    void LeaderIssueCommand()
    {
        IssueCommand(message);
    }

    /// <summary>
    /// Issue commands to all the group members, (personal channels)
    /// </summary>
    void IssueCommand(string message)
    {
        //iterate through the group members and send the commands down to them, including yourself.
        for(int i = 0; i<= _groupMembers.Count; i++)
        {
            //Listening on a private channel requires an id number, the Drone's own id should be provided here
            EventManager.TriggerEvent(message, EventManager.MessageChannel.privateChannel, _groupMembers[i].GetComponent<Drone>().ID);
        }
    }

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
}


