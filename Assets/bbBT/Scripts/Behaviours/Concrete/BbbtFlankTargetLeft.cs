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
            int angle = -90;
            //attack the target force from left
            _group.CreateTargetBounds();
            Vector3 initialPosition = _group.transform.position;
            Vector3 center = _group.targetBounds.center;
            float radius = _group.targetRadius;
            float moveRadius = radius * 1.7f;

            if (_actor.Target == null)
            {
                return BbbtBehaviourStatus.Failure;
            }

            //if you're not on the left, go to the left
            //encircle the center until you reach the back, then move towards the target and return success.
            while (_actor.transform.position != (initialPosition + center + new Vector3(radius + Mathf.Cos(angle), radius + Mathf.Sin(angle))))
            {
                _navMeshAgent.SetDestination(center + new Vector3(radius * Mathf.Cos(-Time.deltaTime), radius * Mathf.Sin(-Time.deltaTime)));
            }
            if (_actor.transform.position == (initialPosition + center + new Vector3(radius + Mathf.Cos(angle), radius + Mathf.Sin(angle))))
            {
                return BbbtBehaviourStatus.Success;
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}
