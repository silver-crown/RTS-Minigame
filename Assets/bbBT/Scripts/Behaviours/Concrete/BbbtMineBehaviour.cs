using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Bbbt
{

    [CreateAssetMenu(
        fileName = "Mine ",
        menuName = "bbBT/Behaviour/Leaf/Mine",
        order = 0)]
    public class BbbtMineBehaviour : BbbtLeafBehaviour
    {
        private Miner _miner;

        public override string SaveDataType { get; } = "Mine";

        protected override void OnInitialize(GameObject gameObject)
        {
            _miner = gameObject.GetComponent<Miner>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        //mine
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_miner.Mine())
            {
                return BbbtBehaviourStatus.Success;
            }
            else
            {
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}