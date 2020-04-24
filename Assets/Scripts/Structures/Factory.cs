using RTS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        private void Start()
        {
            BuildQueue = new Queue<DroneOrder>();
            _currentlyBuilding = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                BuildQueue.Enqueue(new DroneOrder("WorkerDrone", 2.0f));
            }
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
        /// Order a drone of the specific type to be built.
        /// </summary>
        /// <param name="droneType"></param>
        void OrderDrone(string droneType, float buildTime)
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
    }
}