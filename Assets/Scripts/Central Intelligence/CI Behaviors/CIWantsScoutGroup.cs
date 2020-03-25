using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bbbt;

namespace ssuai
{
    public class CIConstructGroup : BbbtBehaviour
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
            throw new System.NotImplementedException();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status) 
        {
            throw new System.NotImplementedException();
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {

            CentralIntelligence CI = gameObject.GetComponent<CentralIntelligence>();
            CI.CreateGroup();

            return BbbtBehaviourStatus.Success;
        }
    }
}

