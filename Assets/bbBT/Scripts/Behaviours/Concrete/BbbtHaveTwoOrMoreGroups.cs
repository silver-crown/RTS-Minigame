using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{
    /// <summary>
    /// Decorator node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    [CreateAssetMenu(fileName = "Have Two Or More Groups", menuName = "bbBT/Behaviour/Leaf/Have Two Or More Groups", order = 0)]
    public class BbbtHaveTwoOrMoreGroups : BbbtLeafBehaviour
    {

        private RTS.Actor _actor;

        public override string SaveDataType { get; } = "BbbtHaveTwoOrMoreGroups";

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