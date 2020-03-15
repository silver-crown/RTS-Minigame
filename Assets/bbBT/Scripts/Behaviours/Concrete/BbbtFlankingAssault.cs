using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{

    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Flanking Assault", menuName = "bbBT/Behaviour/Leaf/Flanking Assault", order = 0)]
   
    /// <summary>
    /// Leaf node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    public class BbbtGotFlankingAssault : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private Drone _drone;

        public override string SaveDataType { get; } = "BbbtFlankingAssault";

        protected override void OnInitialize(GameObject gameObject)
        {
            if (gameObject.GetComponent<Actor>() != null)
            {
                _actor = gameObject.GetComponent<Actor>();
                if (_actor.GetComponent<Drone>() != null)
                {
                    _drone = _actor.GetComponent<Drone>();
                }
            }
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            throw new System.NotImplementedException();
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //Flank the enemy and attack
            return BbbtBehaviourStatus.Failure;
        }
    }
}

