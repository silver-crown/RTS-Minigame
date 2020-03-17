using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RTS;
namespace Bbbt
{
    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Flank Target Left", menuName = "bbBT/Behaviour/Leaf/Flank Target Left", order = 0)]

    /// <summary>
    /// Leaf node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    public class BbbtFlankTargetLeft : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private Group _group;
        private Drone _drone;
        private NavMeshAgent _navMeshAgent;

        public override string SaveDataType { get; } = "BbbtFlankTargetLeft";

        protected override void OnInitialize(GameObject gameObject)
        {
            if (gameObject.GetComponent<Actor>() != null)
            {
                _actor = gameObject.GetComponent<Actor>();
                if (_actor.GetComponent<Drone>() != null)
                {
                    _drone = _actor.GetComponent<Drone>();
                }
                if (_actor.GetComponent<Group>() != null)
                {
                    _group = _actor.GetComponent<Group>();
                }
                if (_actor.GetComponent<NavMeshAgent>() != null)
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
            //seperate the group, make some of them attack on front
            //Others should attack on the left and right
            //maybe do this in the group tree?
            //hm yeah maybe. we'll need seperate, nearly identical trees for this through
            //is all gon b k

            //attack the target force from the left

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
