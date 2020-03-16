using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{
    /// <summary>
    /// Decorator node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    [CreateAssetMenu(fileName = "Group All Drones Flank Enemy", menuName = "bbBT/Behaviour/Leaf/Group All Drones Flank Enemy", order = 0)]
    public class BbbtGroupAllDronesFlankEnemy : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private Group _group;

        public override string SaveDataType { get; } = "BbbtGroupAllDronesFlankEnemy";

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
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //send the attack message to the drones in the group and return success
            //Get the strongest in your force and get them in front, send others to the sides
            for (int i = 0; i <= _group.groupSize; i++)
            {
                EventManager.TriggerEvent("Flanking Assault", EventManager.MessageChannel.groupChannel, _group._groupMembers[i].GetComponent<Drone>().ID);
            }
            return BbbtBehaviourStatus.Success;
        }
    }
}
