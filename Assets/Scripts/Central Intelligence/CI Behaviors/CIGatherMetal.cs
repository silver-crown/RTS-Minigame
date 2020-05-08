using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;

namespace ssuai {

    /// <summary>
    /// Central intelligence behavior node for gathering metal
    /// </summary>
    public class CIGatherMetal : BbbtBehaviour
    {
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
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {         
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //TODO make this not a placeholder function
            CentralIntelligence CI = gameObject.GetComponent<CentralIntelligence>();

            //CI.TestGatherMetal();
            //Debug.Log("Gathering metal, new total: " + CI.Inventory.Contents["Metal"]);

            return BbbtBehaviourStatus.Success;
        }
    }
}
