using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;
using MoonSharp.Interpreter;


namespace ssuai
{
    /// <summary>
    /// Behavior for gathering various resourcers. NOTE: Init MUST be run to select resource type, or this will not work properly.
    /// </summary>
    public class CIGatherResource : BbbtBehaviour
    {
        private string _resourceType;
        private CentralIntelligence _ci;



        public override string SaveDataType => throw new System.NotImplementedException();

        public override void AddChild(BbbtBehaviour child)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Initialize the action
        /// </summary>
        /// <param name="resourceType"> the type of resource to order the drones to mine for</param>
        public void Init(string resourceType)
        {
            _resourceType = resourceType;
            _ci = FindObjectOfType<CentralIntelligence>();
        }

        public override void RemoveChildren()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialize(GameObject gameObject)
        {

        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //Debug.Log("Dronecount: " + _ci.Drones.Count);
            //go through drones, look for an idle worker
            foreach (Drone drone in _ci.Drones)
            {
                if (drone.GetValue("_name").String == "Worker Drone")
                {
                    if (drone.GetValue("_status").String == "Idle")
                    {
                        Debug.Log("Assigned worker.");
                        //tell them to gather the resource
                        //TODO This should use the messaging system.
                        drone.gameObject.GetComponent<RTS.Miner>().MineOrder(_resourceType);
                        return BbbtBehaviourStatus.Success;
                    } 
                }
            }

            //If we couldn't find an idle worker, find one that isn't doing this
            foreach (Drone drone in _ci.Drones)
            {
                if (drone.GetValue("_name").String == "Worker Drone")
                {
                    if (drone.GetValue("_status").String == "Mining " + _resourceType)
                    {
                        //tell them to gather the resource
                        //TODO This should still use the messaging system.
                        drone.gameObject.GetComponent<RTS.Miner>().MineOrder(_resourceType);
                        return BbbtBehaviourStatus.Success;
                    }
                }
            }

            //if we couldn't do either, we've failed.
            return BbbtBehaviourStatus.Failure;
        }
    }
}