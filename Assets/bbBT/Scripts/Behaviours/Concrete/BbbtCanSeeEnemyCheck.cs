using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "CanSeeEnemy",
        menuName = "bbBT/Behaviour/Leaf/CanSeeEnemyCheck",
        order = 0)]

    /// <summary>
    /// Leaf node determines if the actor can see an enemy.
    /// </summary>
    public class BbbtCanSeeEnemyCheck : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;

        public override string SaveDataType { get; } = "BbbtCanSeeEnemyCheck";

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        /// <summary>
        /// Checks if the actor has an enemy in its line of sight
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns> SUCCESS : The actor can see at least one enemy
        ///           FAILURE : the cator can not see any enemies at all" </returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            switch (_actor.MyFaction)
            {
                case Factions.Drone:

                    for (int i = 0; i < EntityLocations.MarineLocations.Count; i++)
                    {
                        if (Vector3.Distance(_actor.transform.position, EntityLocations.MarineLocations[i].transform.position) <= _actor.LineOfSight)
                        {
                            // Inn here we have spotted a player
                            Debug.Log("Spotted player!");

                            // Records last sighting of the player
                            _actor.LastSighting = EntityLocations.MarineLocations[i].transform.position;

                            _actor.EnemyInSight = true;
                            return BbbtBehaviourStatus.Success;
                        }
                    }

                    break;

                case Factions.Elders:
                    break;

                case Factions.Marine:
                    break;
            }
            _actor.EnemyInSight = false;
            return BbbtBehaviourStatus.Failure;
        }
    }
}