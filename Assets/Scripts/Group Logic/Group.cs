using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Group : MonoBehaviour
{
    //Leader status for the drone in question
    [SerializeField] bool _leader;
    public int _groupID;
    /// <summary>
    /// All the group members in the group
    /// </summary>
    List<Drone> _groupMembers = new List<Drone>();
    bool _listening;
    //string used for relaying a message to the group members
    string message;
    /// <summary>
    /// A list of all messages currently sent to the group
    /// </summary>
    public List<string> groupMessageList = new List<string>();
    Group()
    {
        //find each object with a Group on it
        //get the ones with your ID number in it
        //add self to group list in that group
    }
    // Update is called once per frame
    void Update()
    {
        if (!_listening)
        {
            if (_leader)
            {
                LeaderStartListening();
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
            EventManager.StartListening(message, LeaderIssueCommand, EventManager.MessageChannel.groupChannel, _groupID);
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
            EventManager.TriggerEvent(message, EventManager.MessageChannel.privateChannel, _groupMembers[i].ID);
        }
    }
}


