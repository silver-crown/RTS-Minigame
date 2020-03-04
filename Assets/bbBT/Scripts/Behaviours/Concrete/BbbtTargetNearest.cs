using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;


namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "TargetNearest",
        menuName = "bbBT/Behaviour/Leaf/TargetNearest",
        order = 0)]

    ///<summary>
    /// This class targets the closest enemy of the actor
    ///</summary>
    public class BbbtTargetNearest : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;

        public override string SaveDataType { get; } = "BbbtTargetNearest";

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if(_actor.EnemyInSight == false)
            {
                Debug.Log("Tried to target enemy but actor bool says there is no enemies that the actor can see");
                return BbbtBehaviourStatus.Failure;
            }
            
            if (_actor.VisibleEnemies.Count == 0)
            {
                Debug.Log("Tried to target enemy but actors list of visible enmies are empty");
                _actor.EnemyInSight = false;
                return BbbtBehaviourStatus.Failure;
            }


            for(int i = 0; i < _actor.VisibleEnemies.Count; i++)
            {
                   
            }

            return BbbtBehaviourStatus.Failure;
        }

    }
}