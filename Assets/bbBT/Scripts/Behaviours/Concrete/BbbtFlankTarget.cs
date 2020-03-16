using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{

    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Flank Target", menuName = "bbBT/Behaviour/Leaf/Flank Target", order = 0)]
   
    /// <summary>
    /// Leaf node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    public class BbbtGotFlankingAssault : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private Group _group;
        private Drone _drone;

        public override string SaveDataType { get; } = "BbbtFlankTarget";

        protected override void OnInitialize(GameObject gameObject)
        {
            if (gameObject.GetComponent<Actor>() != null)
            {
                _actor = gameObject.GetComponent<Actor>();
                if (_actor.GetComponent<Drone>() != null)
                {
                    _drone = _actor.GetComponent<Drone>();
                }
                if(_actor.GetComponent<Group>() != null)
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
            Bounds dumbo = _group.CreateTargetRadius();
            //Flank the enemy and attack
            //take the group's target radius, and go to one of the four points
            //x position of the centerpoint of the bounds
            float width = dumbo.size.x;
            float height = dumbo.size.y;

            Vector3 topRight = dumbo.center;
            Vector3 topLeft = dumbo.center;
            Vector3 bottomRight = dumbo.center;
            Vector3 bottomLeft = dumbo.center;

            //Set the top right of the bounds
            topRight.x += width / 2;
            topRight.y += height / 2;
            //Set the top left of the bounds
            topLeft.x = width / 2;
            topLeft.y = height / 2;
            //Set the bottom right of the bounds
            bottomRight.x = width / 2;
            bottomRight.y = height / 2;
            //Set the bottom left of the bounds
            bottomLeft.x = width / 2;
            bottomLeft.y = height / 2;
            //victory!
            return BbbtBehaviourStatus.Failure;
        }
    }
}

