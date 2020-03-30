using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Bbbt
{
    [CreateAssetMenu(
        fileName = "HaulerPopQueue",
        menuName = "bbBT/Behaviour/Leaf/Hauler Pop Queue",
        order = 0)]

    public class BbbtHaulerPopQueue : BbbtLeafBehaviour
    {
        Hauler _hauler;
        Drone _drone;

        public override string SaveDataType { get; } = "BbbtHaulerPopQueue";

        protected override void OnInitialize(GameObject gameObject)
        {
            _hauler = gameObject.GetComponent<Hauler>();
            _drone = gameObject.GetComponent<Drone>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            _drone.TargetDepot = _hauler.PopQueue();


            return BbbtBehaviourStatus.Success;
        }
    }
}