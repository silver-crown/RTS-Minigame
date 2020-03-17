using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;
using RTS.Lua;
/// <summary>
/// Drones are used by the enemy AI/CI to interact in the world
/// </summary>
public class Drone : RTS.Actor
{
    /// <summary>
    /// The id to assign to the next instantiated drone.
    /// </summary>
    private static int _nextId = 0;

    /// <summary>
    /// Each channel needs to store their own messages on dictionaries
    /// </summary>
    private Dictionary<string, UnityEvent> _personalChannelDictionary;

    ///<summary>
    ///List of all the messages the drone will me listening after
    /// </summary>
    public List<string> messageList = new List<string>();
    /// <summary>
    /// String used for listening to messages contained in the message list
    /// </summary>
    string[] message;
    int lastMessage;
    /// <summary>
    /// Drone Group and it's ID
    /// </summary>
    [System.NonSerialized] public int groupID;
    [System.NonSerialized] public bool leaderStatus = false;
    [SerializeField] Group group;

    public enum GroupUnit
    {
        Alpha,
        Bravo,
        Charlie,
        Delta
    }
    public GroupUnit myUnit;
    /// <summary>
    /// set the group script's id to match that of the drone
    /// </summary>
    void SetupGroup()
    {
        group.groupID = groupID;
        group.leaderStatus = leaderStatus;
    }
    /// <summary>
    /// Unique ID of the drone
    /// </summary>
    public int ID { get; protected set; }
    public int killCount;

    /// <summary>
    /// The drone's central intelligence.
    /// </summary>
    public CentralIntelligence CentralIntelligence { get; set; }

    /// <summary>
    /// Message Listening, with example functions below
    /// </summary>
    void ListenToChannels()
    {
        //listening on a public channel
        EventManager.StartListening("Testing Worker Channel", globalChannelTest, EventManager.MessageChannel.workerChannel);

        //Listening on a private channel requires an id number, the Drone's own id should be provided here
        EventManager.StartListening("Testing Private Channel", PrivateChannelTest, EventManager.MessageChannel.privateChannel, ID);
    }
    void globalChannelTest()
    {

        messageList.Add(" received a message in the Global Channel!");

    }

    void PrivateChannelTest()
    {
        //Debug.Log("Drone " + ID + " received a message in the Private Channel!");
    }
    void groupChannelTest()
    {
        //Debug.Log("Drone " + ID + " from group " + groupID + " received a message in the group Channel!");
    }

    public override void Awake()
    {
        base.Awake();

        if (_personalChannelDictionary == null)
        {
            _personalChannelDictionary = new Dictionary<string, UnityEvent>();
        }

        ID = _nextId++;

        EventManager.AddPrivateChannel(_personalChannelDictionary);
    }

    public override void Start()
    {
        base.Start();
        SetupMessagesToListenTo();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //Debug.Log(_script.Call(_script.Globals["Update"]));

        var lastTimeScouted = GetValue("_lastTimeChunkWasScouted");
        if (lastTimeScouted.IsNotNil())
        {
            // Update when the actor saw a chunk
            foreach (var chunk in WorldInfo.Chunks)
            {
                var rect = new Rect(chunk, Vector2.one);
                var pos = new Vector2(transform.position.x, transform.position.z);
                var distances = new float[]
                {
                    Vector2.Distance(pos, new Vector2(rect.xMin, rect.yMin)),
                    Vector2.Distance(pos, new Vector2(rect.xMin, rect.yMax)),
                    Vector2.Distance(pos, new Vector2(rect.xMax, rect.yMin)),
                    Vector2.Distance(pos, new Vector2(rect.xMax, rect.yMax))
                };

                bool inSightRange = true;
                foreach (var distance in distances)
                {
                    if (distance > GetValue("_sightRange").Number)
                    {
                        inSightRange = false;
                    }
                }

                if (inSightRange)
                {
                    lastTimeScouted.Table.Set(chunk.ToString(), DynValue.NewNumber(Time.time));
                    if (CentralIntelligence != null)
                    {
                        CentralIntelligence.SetLastTimeScouted(chunk, Time.time);
                    }
                }
            }
        }
    }

    public override void Attack()
    {
        base.Attack();
    }


    /// <summary>
    /// Reads the drone's stats from lua.
    /// </summary>
    /// <param name="type">The drone type to set </param>
    public void SetType(string type)
    {
        Type = type;
        _luaObject = LuaManager.CreateLuaObject("Actors/Drones/" + type);
        string tree = GetValue("_behaviourTree").String;

        if (tree != null)
        {
            GetComponent<BbbtBehaviourTreeComponent>().SetBehaviourTree(tree);
        }
        else
        {
            Debug.LogError(GetType().Name + ".SetType(): _behaviourTree not present in " + type + ".lua", this);
        }
    }

    public void ReceiveMessageOnChannel(string message, EventManager.MessageChannel channel)
    {
        //a switch for the channel
        switch (channel)
        {
            case (EventManager.MessageChannel.globalChannel):
                {
                    //and a nested one for the message itself
                    switch (message)
                    {
                        //a test message
                        case ("Test message"):
                            {
                                break;
                            }
                    }
                    break;
                }
        }
    }
    void ListenToMessages()
    {
        for (int i = 0; i <= message.Length; i++)
        {
            lastMessage = i;
            EventManager.StartListening(message[i], () => { messageList.Add(message[lastMessage]); }, EventManager.MessageChannel.privateChannel, ID);
        }
    }
    /// <summary>
    /// sets up the message array and the strings it can listen for
    /// </summary>
    void SetupMessagesToListenTo()
    {
        if (message == null) return;
        int i = 0;
        message[i++] = "Frontal Assault";
        message[i++] = "Flanking Assault Frontal";
        message[i++] = "Flanking Assault Behind";
        message[i++] = "Flanking Assault Right";
        message[i++] = "Flanking Assault Left";
    }


}
