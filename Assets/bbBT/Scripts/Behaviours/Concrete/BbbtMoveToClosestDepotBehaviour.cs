using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using UnityEngine.AI;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Move To Depot",
        menuName = "bbBT/Behaviour/Leaf/Move To Depot",
        order = 0)]
    public class BbbtMoveToClosestDepotBehaviour : BbbtLeafBehaviour
    {
        private Miner _miner;
        private NavMeshAgent _navMeshAgent;


        [SerializeField]
        float depositRange=5.0f;

        public override string SaveDataType { get; } = "BbbtMoveToClosestDepotBehaviour";

        protected override void OnInitialize(GameObject gameObject)
        {
            _miner = gameObject.GetComponent<Miner>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _navMeshAgent.SetDestination(_miner.TargetDepot.transform.position);
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {


            if (_miner.TargetDepot == null)
            {
                return BbbtBehaviourStatus.Failure;
            }
            if (depositRange >= Vector3.Distance(gameObject.transform.position, _miner.TargetDepot.transform.position))
            {
                _navMeshAgent.SetDestination(_miner.transform.position);
                return BbbtBehaviourStatus.Success;
            }
            //Debug.Log(Vector3.Distance(gameObject.transform.position, _miner.TargetDepot.transform.position));
            return BbbtBehaviourStatus.Failure;
        }
    }
}