using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Bbbt
{

    /// <summary>
    /// Move near the CI
    /// </summary>
    [CreateAssetMenu(
        fileName = "Move To CI",
        menuName = "bbBT/Behaviour/Leaf/Move To CI",
        order = 0)]
    public class BbbtMoveToCIBehaviour : BbbtLeafBehaviour
    {
        private CentralIntelligence _ci;
        private NavMeshAgent _navMeshAgent;

        /// <summary>
        /// The range from the CI the drone will try to move to while idling
        /// </summary>
        [SerializeField]
        private float _minRange = 5.0f;


        public override string SaveDataType { get; } = "BbbtMoveToCIBehaviour";

        protected override void OnInitialize(GameObject gameObject)
        {
            _ci = gameObject.GetComponent<Drone>().CentralIntelligence;
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _navMeshAgent.SetDestination(_ci.transform.position);
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //success if we are within range of the CI, failure otherwise.
            if (_minRange >= Vector3.Distance(gameObject.transform.position, _ci.transform.position))
            {
                _navMeshAgent.SetDestination(_ci.transform.position);
                return BbbtBehaviourStatus.Success;
            }

            return BbbtBehaviourStatus.Failure;
        }
    }
}