using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{
    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Group All Drones Attack", menuName = "bbBT/Behaviour/Leaf/Group All Drones Attack", order = 0)]
    /// <summary>
    /// Leaf node for commanding all drones in a group to attack their targets.
    /// </summary>
    public class BbbtGroupAllDronesAttack : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private Group _group;

        public override string SaveDataType { get; } = "BbbtGroupAllDronesAttack";

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
            //send the attack message to the drones in the group and return success
            for(int i = 0; i <= _group.groupSize; i++)
            {
                EventManager.TriggerEvent("Full Frontal Assault", EventManager.MessageChannel.groupChannel,_group.groupMembers[i].GetComponent<Drone>().ID);
            }
            return BbbtBehaviourStatus.Success;
        }
    }
}

