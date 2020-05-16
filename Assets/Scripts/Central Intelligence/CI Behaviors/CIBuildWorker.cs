using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;


namespace ssuai
{
    /// <summary>
    /// Central intelligence behavior node for building a worker
    /// </summary>
    public class CIBuildWorker : BbbtBehaviour
    {
        private CentralIntelligence _ci; 
        public override string SaveDataType => throw new System.NotImplementedException();

        public override void AddChild(BbbtBehaviour child)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveChildren()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialize(GameObject gameObject)
        {
            _ci = gameObject.GetComponent<CentralIntelligence>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
           
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //pay the resource cost of the worker
            //tell the factory to enqueue a worker

            return BbbtBehaviourStatus.Success;
        }


    }
}