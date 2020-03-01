using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "DroneCanSeeEnemy",
        menuName = "bbBT/Behaviour/Leaf/Drone/DronecanSeeEnemy",
        order = 0)]

    /// <summary>
    /// Leaf Node behavior for drone
    /// </summary>
    public class BbbtDroneCanSeeEnemyCheck : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtDroneCanSeeEnemyCheck";

        private RTS.Actor _actor;

        /*
        public override BbbtBehaviourSaveData ToSaveData()
        {
            throw new System.NotImplementedException();
        }
        */

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            
        }

        /// <summary>
        /// Updates the drone behavior
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>Returns SUCCESS if the drone can see an enemy
        ///              and FAILURE it can not" </returns>
        protected override BbbtBehaviourStatus UpdateBehavior(GameObject gameObject)
        {
            for (int i = 0; i < EntityLocations.MarineLocations.Count; i++)
            {
                if (Vector3.Distance(_actor.transform.position, EntityLocations.MarineLocations[i].transform.position) <= _actor.LineOfSight)
                {
                    // Inn here we have spotted a player
                    Debug.Log("Spotted player!");

                    // Records last sighting of the player
                    _actor.LastSighting = EntityLocations.MarineLocations[i].transform.position;

                    // Let CI know about the spotted player?

                    return BbbtBehaviourStatus.Success;
                }
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}