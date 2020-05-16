using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{
    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Got Flanking Behind Message", menuName = "bbBT/Behaviour/Leaf/Got Flanking Behind Message", order = 0)]
    /// <summary>
    /// Leaf node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    public class BbbtGotFlankingBehindMessage : BbbtLeafBehaviour
    {
        private ListenToChannel _listenToChannel;
        private Drone _drone;

        public override string SaveDataType { get; } = "BbbtGotFlankingBehindMessage";

        protected override void OnInitialize(GameObject gameObject)
        {
            _drone = gameObject.GetComponent<Drone>();
            _listenToChannel = gameObject.GetComponent<ListenToChannel>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            throw new System.NotImplementedException();
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //Get the attack message from CI and return success, assuming it's the newest message received from CI           
            if (_listenToChannel.GetLastMessage() == "Flanking Assault Behind")
            {
                return BbbtBehaviourStatus.Success;
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}
