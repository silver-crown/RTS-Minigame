using Newtonsoft.Json;
using RTS;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Leaf node which checks if an actor is in a building or not.
    /// </summary>
    [CreateAssetMenu(
        fileName = "Is In Building Check",
        menuName = "bbBT/Behaviour/Leaf/Is In Building Check",
        order = 0)]
    public class BbbtIsInBuildingCheck : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtIsInBuildingCheck";

        protected override void OnInitialize(GameObject gameObject)
        {
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            foreach (var building in WorldInfo.EnterableBuildings)
            {
                if (building.Contains(gameObject.GetComponent<Actor>()))
                {
                    return BbbtBehaviourStatus.Success;
                }
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}