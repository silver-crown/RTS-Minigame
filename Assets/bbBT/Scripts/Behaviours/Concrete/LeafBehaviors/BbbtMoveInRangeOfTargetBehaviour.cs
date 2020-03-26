using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;
using UnityEngine.AI;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Move In Range Of Target",
        menuName = "bbBT/Behaviour/Leaf/Move In Range Of Target",
        order = 0)]

    /// <summary>
    /// Checks if the target is within attack distance of the actor
    /// </summary> 
    public class BbbtMoveInRangeOfTargetBehaviour : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private NavMeshAgent _navMeshAgent;

        public override string SaveDataType { get; } = "BbbtMoveInRangeOfTargetBehaviour";

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _navMeshAgent.SetDestination(_actor.Target.transform.position);
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_actor.Target == null)
            {
                return BbbtBehaviourStatus.Failure;
            }
            float attackRange = (float)_actor.GetValue("_attackRange").Number;
            if (attackRange >= Vector3.Distance(gameObject.transform.position, _actor.Target.transform.position))
            {
                _navMeshAgent.SetDestination(_actor.transform.position);
                return BbbtBehaviourStatus.Success;
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}
