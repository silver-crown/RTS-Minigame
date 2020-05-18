using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Drone Is Idle Check",
        menuName = "bbBT/Behaviour/Leaf/DroneIsIdleCheck",
        order = 0)]

    public class BbbtDroneIsIdleCheck : BbbtLeafBehaviour
    {
        private Drone _drone;
        public override string SaveDataType { get; } = "BbbtDroneIsIdleCheck";

        protected override void OnInitialize(GameObject gameObject)
        {
            _drone = gameObject.GetComponent<Drone>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_drone.GetValue("_status").String == "Idle")
            {
                return BbbtBehaviourStatus.Success;
            } else
            {
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}