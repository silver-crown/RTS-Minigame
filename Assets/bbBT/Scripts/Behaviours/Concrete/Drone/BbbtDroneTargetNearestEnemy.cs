using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "DroneTargetNearestEnemy",
        menuName = "bbBT/Behaviour/Leaf/Drone/DroneTargetNearestEnemy",
        order = 0)]

    /// <summary>
    /// Action behavior that Targetes the drones nearest enemy.
    /// </summary>
    public class BbbtDroneTargetNearestEnemy : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtDroneTargetNearestEnemy";

        private GameObject _nearestEnemy;
        

        private RTS.Actor _actor;

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
            if(_actor.VisibleEnemies.Count == 0)
            {
                return BbbtBehaviourStatus.Failure;
            }

            _nearestEnemy = _actor.VisibleEnemies[0];
            for (int i = 0; i < _actor.VisibleEnemies.Count; i++)
            {
                

            }

            // 1. Loop Over List of Visible enemies

            throw new System.NotImplementedException();
        }
    }
}
