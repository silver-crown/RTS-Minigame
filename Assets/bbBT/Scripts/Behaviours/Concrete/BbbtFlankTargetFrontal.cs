using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS;
namespace Bbbt
{

    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Flank Target Frontal", menuName = "bbBT/Behaviour/Leaf/Flank Target Frontal", order = 0)]
   
    /// <summary>
    /// Leaf node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    public class BbbtFlankTargetFrontal : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private GroupLeader _group;
        private Drone _drone;
        private NavMeshAgent _navMeshAgent;

        public override string SaveDataType { get; } = "BbbtFlankTargetFrontal";

        protected override void OnInitialize(GameObject gameObject)
        {
            if (gameObject.GetComponent<Actor>() != null)
            {
                _actor = gameObject.GetComponent<Actor>();
                if (_actor.GetComponent<Drone>() != null)
                {
                    _drone = _actor.GetComponent<Drone>();
                }
                if(_actor.GetComponent<GroupLeader>() != null)
                {
                    _group = _actor.GetComponent<GroupLeader>();
                }
                if(_actor.GetComponent<NavMeshAgent>() != null)
                {
                    _navMeshAgent = _actor.GetComponent<NavMeshAgent>();
                }
            }
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            throw new System.NotImplementedException();
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //attack the target force from the front
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
            //victory!
            return BbbtBehaviourStatus.Failure;
        }
    }
}

