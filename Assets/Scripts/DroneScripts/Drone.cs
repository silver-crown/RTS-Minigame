using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using MoonSharp.Interpreter;
using Bbbt;
using System.IO;

using RTS;

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

        /// <summary>
        /// The drone's type.
        /// </summary>
        public string Type { get; protected set; }

        ///<summary>
        ///List of all the messages the drone will me listening after
        /// </summary>
        public List<string> messageList = new List<string>();
        /// <summary>
        /// Drone Group ID
        /// </summary>
        private int groupID;


        /// <summary>
        /// Unique ID of the drone
        /// </summary>
        public int ID { get; protected set; }

        //************************************************************************************
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
            Debug.Log("Drone " + ID + " received a message in the Global Channel!");
        }

        void PrivateChannelTest()
        {
            Debug.Log("Drone " + ID + " received a message in the Private Channel!");
        }
        void groupChannelTest()
        {
            Debug.Log("Drone " + ID + " from group " + groupID + " received a message in the group Channel!");
        }

        public override void Awake()
        {
            base.Awake();

            if (_personalChannelDictionary == null)
            {
                _personalChannelDictionary = new Dictionary<string, UnityEvent>();
            }

            SetDroneType();

            ID = _nextId++;

            EventManager.AddPrivateChannel(_personalChannelDictionary);
        }

        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            ListenToChannels();
        }

        /// <summary>
        /// Reads the drone's stats from lua.
        /// </summary>
        /// <param name="type">The drone type to set </param>
        public void SetType(string type)
        {
            Type = type;
            Script script = new Script();
            _table = script.DoFile(Path.Combine("Actors", "Drones", type)).Table;
            string tree = _table.Get("_behaviourTree").String;

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

    }
