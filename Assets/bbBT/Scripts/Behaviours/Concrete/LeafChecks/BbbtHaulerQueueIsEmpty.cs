using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Bbbt
{
    [CreateAssetMenu(
        fileName = "Hauler Queue Is Empty",
        menuName = "bbBT/Behaviour/Leaf/Bbbt Hauler Queue Is Empty",
        order = 0)]
    /// <summary>
    /// Checks if the queue for a Hauler is empty or not.
    /// </summary>
    public class BbbtHaulerQueueIsEmpty : BbbtLeafBehaviour
    {
        Hauler _hauler;

        public override string SaveDataType { get; } = "BbbtHaulerQueueIsEmpty";

        protected override void OnInitialize(GameObject gameObject)
        {
            _hauler = gameObject.GetComponent<Hauler>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_hauler == null)
            {
                return BbbtBehaviourStatus.Failure;
            }
            if(_hauler.IsQueueEmpty())
            {
                return BbbtBehaviourStatus.Success;
            } else
            {
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}