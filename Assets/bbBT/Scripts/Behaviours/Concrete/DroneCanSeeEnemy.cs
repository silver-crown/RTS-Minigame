using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace Bbbt
{

    /// <summary>
    /// Leaf Node behavior for drone
    /// </summary>
    [CreateAssetMenu(
        fileName = "DroneCanSeeEnemy",
        menuName = "bbBT/Behaviour/Leaf/DronecanSeeEnemy",
        order = 0)]
    public class DroneCanSeeEnemy : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;

        public override BbbtBehaviourSaveData ToSaveData()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Updates the drone behavior
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns> </returns>
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