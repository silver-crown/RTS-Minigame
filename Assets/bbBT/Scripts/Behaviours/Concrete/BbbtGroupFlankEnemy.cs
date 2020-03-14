using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{
    /// <summary>
    /// Decorator node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    [CreateAssetMenu(fileName = "Group Flank Enemy", menuName = "bbBT/Behaviour/Leaf/Group Flank Enemy", order = 0)]
    public class BbbtGroupFlankEnemy : BbbtLeafBehaviour
    {

        public override string SaveDataType { get; } = "BbbtGroupFlankEnemy";

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
