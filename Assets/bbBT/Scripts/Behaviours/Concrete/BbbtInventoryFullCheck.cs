using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{

    /// <summary>
    /// Leaf node that checks if Inventory component is full
    /// </summary>
    [CreateAssetMenu(
        fileName = "Inventory Full Check",
        menuName = "bbBT/Behaviour/Leaf/Inventory Full Check",
        order = 0)]
    public class BbbtInventoryFullCheck : BbbtLeafBehaviour
    {
        private Inventory _inventory;

        public override string SaveDataType { get; } = "BbbtInventoryFullCheck";

        protected override void OnInitialize(GameObject gameObject)
        {
            _inventory.gameObject.GetComponent<Inventory>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_inventory == null)
            {
                Debug.LogError("Error: GameObject " + gameObject.name + " has no Inventory component.");
                return BbbtBehaviourStatus.Invalid;
            }

            if (_inventory.GetAvailableSpace() == 0)
            {
                return BbbtBehaviourStatus.Success;
            } else
            {
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}