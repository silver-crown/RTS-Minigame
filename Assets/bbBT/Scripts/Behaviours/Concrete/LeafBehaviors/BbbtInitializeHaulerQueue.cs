using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Bbbt
{

    [CreateAssetMenu(
    fileName = "BbbtInitializeHaulerQueue",
    menuName = "bbBT/Behaviour/Leaf/Initialize Hauler Queue",
    order = 0)]
    public class BbbtInitializeHaulerQueue : BbbtLeafBehaviour
    {
        Hauler _hauler;

        public override string SaveDataType { get; } = "BbbtInitializeHaulerQueue";

        protected override void OnInitialize(GameObject gameObject)
        {
            _hauler = gameObject.GetComponent<Hauler>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            _hauler.InitializeQueue();
            return BbbtBehaviourStatus.Success;
        }
    }
}