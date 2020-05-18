using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeeter;
using MoonSharp.Interpreter;

namespace RTS
{
    /// <summary>
    /// Factory component that lets the object it's attached to build drones. Requires an inventory to work properly.
    /// </summary>
    public class Factory : MonoBehaviour
    {

        Queue<DroneOrder> BuildQueue;

        private float _buildingDoneTime = 0;

        private bool _currentlyBuilding;

        private CentralIntelligence _ci;
        private ListenToChannel _listenToChannel;



        private void Start()
        {
            _listenToChannel = gameObject.GetComponent<ListenToChannel>();
            _ci = GameObject.Find("CI").GetComponent<CentralIntelligence>();

            BuildQueue = new Queue<DroneOrder>();
            _currentlyBuilding = false;
            WorldInfo.Factories.Add(this.gameObject);
            
            Debug.Log(_listenToChannel);
            _listenToChannel.MessageReceived += OnOrderReceived;
        }


        private void Update()
        {
            //if there is something in the queue 
            if (BuildQueue.Count != 0)
            {
                //and we are not currently building anything
                if (_currentlyBuilding == false)
                {
                    //set a new completion time
                    _buildingDoneTime = Time.time + BuildQueue.Peek().BuildTime;

                    //start building
                    _currentlyBuilding = true;

                    //if we've finished building something
                }
#if UNITY_EDITOR
                else if (Time.time >= _buildingDoneTime)
                {
                    _currentlyBuilding = false;
                    //pop the queue
                    DroneOrder finishedProduct = BuildQueue.Dequeue();

                    //build that drone
                    DroneStaticMethods.Create(finishedProduct.Type, transform.position.x, transform.position.y, transform.position.z);

                    Debug.Log("I just finished a drone of type " + finishedProduct.Type + " in " + finishedProduct.BuildTime + " seconds");
                }
#endif
            }
        }

        /// <summary>
        /// When an order is received, see if it's a build order, if so build a drone of that type
        /// </summary>
        /// <param name="message"></param>
        public void OnOrderReceived()
        {
            //split up the message
            string[] splitMessage = _listenToChannel.GetLastMessage().Split(' ');
            string droneType = splitMessage[splitMessage.Length - 1];

            if (splitMessage[0] == "Build")
            {
            Script droneScript = new Script();
            var table = droneScript.DoFile("Actors.Drones."+droneType).Table;

            //build a drone of the ordered type
            _orderDrone(droneType, (float)table.Get("_buildTime").Number);
            }
        }

        /// <summary>
        /// Order a drone of the specific type to be built.
        /// </summary>
        /// <param name="droneType"></param>
        private void _orderDrone(string droneType, float buildTime)
        {
            BuildQueue.Enqueue(new DroneOrder(droneType, buildTime));
        }

        //basic wrapper struct with constructor for things in the build queue
        private struct DroneOrder
        {
            //type of drone being built
            public string Type { get; private set; }

            //time to build the thing
            public float BuildTime { get; private set; }

            public DroneOrder(string name, float buildTime)
            {
                Type = name;
                BuildTime = buildTime;
            }
        }

        /// <summary>
        /// returns whether the queue is empty or not
        /// </summary>
        /// <returns></returns>
        public int GetQueueCount()
        {
            return BuildQueue.Count;
        }
    }
}