using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;


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
            //find an idle worker
            //tell them to mine
            //_ci.SendMessage

            return BbbtBehaviourStatus.Success;
        }
    }
}