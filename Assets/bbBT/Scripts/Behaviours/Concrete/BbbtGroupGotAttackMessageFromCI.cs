using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{

    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Group Got Attack Message From CI", menuName = "bbBT/Behaviour/Leaf/Group Got Attack Message From CI", order = 0)]
    /// <summary>
    /// Leaf node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    public class BbbtGroupGotAttackMessageFromCI : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private Group _group;

        public override string SaveDataType { get; } = "BbbtGroupGotAttackMessageFromCI";

        protected override void OnInitialize(GameObject gameObject)
        {
            if (gameObject.GetComponent<Actor>() != null)
            {
                _actor = gameObject.GetComponent<Actor>();
                if (_actor.GetComponent<Group>() != null)
                {
                    _group = _actor.GetComponent<Group>();
                }
            }
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            throw new System.NotImplementedException();
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //Get the attack message from CI and return success, assuming it's the newest message received from CI           
            if(_group.groupMessageList[_group.groupMessageList.Count - 1] == "Group Frontal Assault")
            {
                return BbbtBehaviourStatus.Success;
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}

