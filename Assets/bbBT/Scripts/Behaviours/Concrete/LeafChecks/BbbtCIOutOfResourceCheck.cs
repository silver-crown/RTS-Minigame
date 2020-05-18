using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{

    /// <summary>
    /// Leaf node that checks if Ithe CI is completely out of this resource
    /// </summary>
    [CreateAssetMenu(
        fileName = "CI Out Of Resource Check",
        menuName = "bbBT/Behaviour/Leaf/CI Out Of Resource Check",
        order = 0)]
    public class BbbtCIOutOfResourceCheck : BbbtLeafBehaviour
    {
        private Inventory _ciInventory;
        private Drone _drone;

        public override string SaveDataType { get; } = "BbbtCIOutOfResourceCheck";

        protected override void OnInitialize(GameObject gameObject)
        {
            _ciInventory = GameObject.Find("CI").GetComponent<Inventory>();
            _drone = gameObject.GetComponent<Drone>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_drone == null)
            {
                Debug.LogError("Error: GameObject " + gameObject.name + " has no Drone component.");
                return BbbtBehaviourStatus.Invalid;
            }

            if (_ciInventory.GetAmountOfResource(_drone.TargetResourceType) == 0)
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