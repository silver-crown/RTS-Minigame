using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{
  
    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Group Attack Enemy", menuName = "bbBT/Behaviour/Leaf/Group Attack Enemy", order = 0)]
    /// <summary>
    /// Leaf node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    public class BbbtGroupAttackEnemy : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private Group _group;
        private int _forces;

        public override string SaveDataType { get; } = "BbbtGroupAttackEnemy";

        protected override void OnInitialize(GameObject gameObject)
        {   
            if(gameObject.GetComponent<Actor>() != null)
            {
                _actor = gameObject.GetComponent<Actor>();
                if(_actor.GetComponent<Group>() != null)
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

            //iterate through the group, and get them to attack their target by activating their attack trees
              if(_actor.Target == null)
            {
                Debug.Log("Attack Failed: no target is assigned");
                return BbbtBehaviourStatus.Failure;
            }

            if(Vector3.Distance(_actor.Target.transform.position, _actor.transform.position) > _actor.AttackRange)
            {
                Debug.Log("Attack Failed: target is out of range");
                return BbbtBehaviourStatus.Failure;
            }

            Debug.Log("Attacked target!");

            _actor.Attack();

            return BbbtBehaviourStatus.Success;
        }
    }
}

