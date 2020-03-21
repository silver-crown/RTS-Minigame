using Newtonsoft.Json;
using RTS;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Leaf node which causes an actor to enter the nearest building.
    /// </summary>
    [CreateAssetMenu(fileName = "Leave Building", menuName = "bbBT/Behaviour/Leaf/Leave Building", order = 0)]
    public class BbbtLeaveBuildingBehaviour : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtLeaveBuildingBehaviour";

        private EnterableBuilding _building;

        protected override void OnInitialize(GameObject gameObject)
        {
            _building = null;
            // Find building that the actor is in.
            foreach (var building in WorldInfo.EnterableBuildings)
            {
                if (building.Contains(gameObject.GetComponent<Actor>()))
                {
                    _building = building;
                }
            }
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_building == null)
            {
                return BbbtBehaviourStatus.Failure;
            }
            else
            {
                _building.Remove(gameObject.GetComponent<Actor>());
                return BbbtBehaviourStatus.Success;
            }
        }
    }
}