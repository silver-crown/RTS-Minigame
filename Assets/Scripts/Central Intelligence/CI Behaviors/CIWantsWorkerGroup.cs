using System.Collections;
using System.Collections.Generic;
using Bbbt;
using UnityEngine;

namespace ssuai
{
    /// <summary>
    /// Central intelligence behavior node for the desire to have a worker group
    /// </summary>
    public class CIWantsWorkerGroup : BbbtBehaviour
    {
        public override string SaveDataType => throw new System.NotImplementedException();

        public override void AddChild(BbbtBehaviour child) {
            throw new System.NotImplementedException();
        }

        public override void RemoveChildren() {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialize(GameObject gameObject) {

        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status) {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject) {
            //TODO make this not a placeholder function
            CentralIntelligence CI = gameObject.GetComponent<CentralIntelligence>();

            CI.TestBuildDrone();

            return BbbtBehaviourStatus.Success;
        }
    }
}