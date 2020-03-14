using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using RTS;
using UnityEngine;


namespace Bbbt
{
    /// <summary>
    /// Decorator node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    [CreateAssetMenu(fileName = "Have Adequate Forces", menuName = "bbBT/Behaviour/Leaf/Have Adequate Forces", order = 0)]
    public class BbbtHaveAdequateForces : BbbtLeafBehaviour
    {

        public override string SaveDataType { get; } = "BbbtHaveAdequateForces";

        protected override void OnInitialize(GameObject gameObject)
        {
     
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            return BbbtBehaviourStatus.Failure;
        }
    }
}

