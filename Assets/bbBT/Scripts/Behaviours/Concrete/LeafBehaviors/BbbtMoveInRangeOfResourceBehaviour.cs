using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using UnityEngine.AI;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Move In Range Of Resource",
        menuName = "bbBT/Behaviour/Leaf/Move In Range Of Resource",
        order = 0)]

    /// <summary>
    /// Checks if the target is within attack distance of the actor
    /// </summary> 
    public class BbbtMoveInRangeOfResourceBehaviour : BbbtLeafBehaviour
    {
        private Miner _miner;
        private NavMeshAgent _navMeshAgent;

        public override string SaveDataType { get; } = "BbbtMoveInRangeOfResourceBehaviour";

        protected override void OnInitialize(GameObject gameObject)
        {
            _miner = gameObject.GetComponent<Miner>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _navMeshAgent.SetDestination(_miner.TargetResourceObject.transform.position);
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_miner.TargetResourceObject == null)
            {
                return BbbtBehaviourStatus.Failure;
            }
            float miningRange = (float)_miner.MiningRange;
            if (miningRange >= Vector3.Distance(gameObject.transform.position, _miner.TargetResourceObject.transform.position))
            {
                _navMeshAgent.SetDestination(_miner.transform.position);
                return BbbtBehaviourStatus.Success;
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}
