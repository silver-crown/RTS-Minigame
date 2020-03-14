using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{
    /// <summary>
    /// Decorator node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    [CreateAssetMenu(fileName = "Group Attack Enemy", menuName = "bbBT/Behaviour/Leaf/Group Attack Enemy", order = 0)]
    public class BbbtGroupAttackEnemy : BbbtLeafBehaviour
    {

        public override string SaveDataType { get; } = "BbbtGroupAttackEnemy";

        protected override void OnInitialize(GameObject gameObject)
        {   

        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            return BbbtBehaviourStatus.Failure;
        }
    }
}

