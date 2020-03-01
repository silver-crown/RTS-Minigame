using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "DroneAllEnimesInSight",
        menuName = "bbBT/Behaviour/Leaf/Drone/DroneAllEnimesInSight",
        order = 0)]

    /// <summary>
    /// This behavior fills up the Actor class visibleEnmies List variable with enemies that the drone can see.
    /// </summary>
    public class BbbtDroneAllEnimesInSight : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtDroneAllEnimesInSight";

        private RTS.Actor _actor;

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            
        }

        /// <summary>
        /// Loops over all the enemy marines aka player units and sees if it can spot one or more enemies
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>
        ///     BbbtBehaviourStatus.Failure; If no drone was counted
        ///     BbbtBehaviourStatus.Success; if at least one drone was counted
        /// </returns>
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

                    // checks that the detected player is not already inn the list, this is to avoid
                    // the same enemy beeing added multiple times in the same list.
                    if(!_actor.VisibleEnemies.Contains(EntityLocations.MarineLocations[i]))
                    {
                        _actor.VisibleEnemies.Add(EntityLocations.MarineLocations[i]);
                    }
                }
            }

            // If VisibleEnemies is 0 we have lost sight of the enemie(s)
            if(_actor.VisibleEnemies.Count == 0)
            {
                _actor.EnemyInSight = false;
                return BbbtBehaviourStatus.Failure;
            }
            else
            {
                Debug.Log("Drone spotted " + _actor.VisibleEnemies.Count + " Number of players");
                return BbbtBehaviourStatus.Success;
            }
        }
    }
}