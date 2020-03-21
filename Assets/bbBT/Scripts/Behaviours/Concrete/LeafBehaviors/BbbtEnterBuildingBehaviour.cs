using Newtonsoft.Json;
using RTS;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Leaf node which causes an actor to enter the nearest building.
    /// </summary>
    [CreateAssetMenu(fileName = "Enter Building", menuName = "bbBT/Behaviour/Leaf/Enter Building", order = 0)]
    public class BbbtEnterBuildingBehaviour : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtEnterBuildingBehaviour";

        private EnterableBuilding _building;

        protected override void OnInitialize(GameObject gameObject)
        {
            _building = null;
            // Find nearest building.
            EnterableBuilding nearestBuilding = null;
            float distanceToNearestBuilding = Mathf.Infinity;
            foreach (var building in WorldInfo.EnterableBuildings)
            {
                if (building.IsFull) continue;
                var distance = Vector3.Distance(gameObject.transform.position, building.transform.position);
                if (distance < distanceToNearestBuilding)
                {
                    nearestBuilding = building;
                    distanceToNearestBuilding = distance;
                }
            }
            if (nearestBuilding != null)
            {
                _building = nearestBuilding;
                gameObject.GetComponent<Actor>().agent.SetDestination(_building.EntryPoint);
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
            else if (Vector3.Distance(gameObject.transform.position, _building.EntryPoint) < 0.75f)
            {
                if (!_building.IsFull)
                {
                    _building.Add(gameObject.GetComponent<Actor>());
                    return BbbtBehaviourStatus.Success;
                }
                else
                {
                    return BbbtBehaviourStatus.Failure;
                }
            }
            else if (_building != null)
            {
                return BbbtBehaviourStatus.Running;
            }

            return BbbtBehaviourStatus.Invalid;
        }
    }
}