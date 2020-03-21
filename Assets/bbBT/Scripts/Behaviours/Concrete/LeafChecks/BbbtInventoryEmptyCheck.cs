using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{

    /// <summary>
    /// Leaf node that checks if Inventory component is empty
    /// </summary>
    [CreateAssetMenu(
        fileName = "Inventory Empty Check",
        menuName = "bbBT/Behaviour/Leaf/Inventory Empty Check",
        order = 0)]
    public class BbbtInventoryEmptyCheck : BbbtLeafBehaviour
    {
        private Inventory _inventory;

        public override string SaveDataType { get; } = "BbbtInventoryEmptyCheck";

        protected override void OnInitialize(GameObject gameObject)
        {
            _inventory = gameObject.GetComponent<Inventory>();
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

            if (_inventory.GetAvailableSpace() == _inventory.Capacity)
            {
                return BbbtBehaviourStatus.Success;
            }
            else
            {
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}