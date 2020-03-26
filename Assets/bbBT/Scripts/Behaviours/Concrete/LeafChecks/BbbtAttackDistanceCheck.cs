using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Attack Distance Check",
        menuName = "bbBT/Behaviour/Leaf/AttackDistanceCheck",
        order = 0)]

    /// <summary>
    /// Checks if the target is within attack distance of the actor
    /// </summary> 
    public class BbbtAttackDistanceCheck : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;

        public override string SaveDataType { get; } = "BbbtAttackDistanceCheck";

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
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
                return BbbtBehaviourStatus.Success;
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}
