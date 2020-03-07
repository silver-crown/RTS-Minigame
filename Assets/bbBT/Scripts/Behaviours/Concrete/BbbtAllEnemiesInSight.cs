using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "AllEnemiesInSight",
        menuName = "bbBT/Behaviour/Leaf/AllEnemiesInSight",
        order = 0)]

    /// <summary>
    /// This behavior fills up the Actor class visibleEnmies List variable with enemies the actor can see.
    /// </summary>
    public class BbbtAllEnemiesInSight : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;

        public override string SaveDataType { get; } = "BbbtAllEnemiesInSight";

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
         
        }

        /// <summary>
        /// Loops over all potential enemies for the actor and checks if they
        /// are within the actors Line Of Sight
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>
        ///     BbbtBehaviourStatus.Failure; if no enemy is visible
        ///     BbbtBehaviourStatus.Success; if at least one enemy was spotted
        /// </returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {

             _actor.VisibleEnemies.Clear();

            switch (_actor.MyFaction)
            {

                case Factions.Drone:

                    for (int i = 0; i < WorldInfo.Marines.Count; i++)
                    {
                        if (Vector3.Distance(_actor.transform.position, WorldInfo.Marines[i].transform.position) <= _actor.LineOfSight)
                        {
                            // Records last sighting of the player
                            _actor.LastSighting = WorldInfo.Marines[i].transform.position;

                            _actor.EnemyInSight = true;

                            // checks that the detected player is not already inn the list, this is to avoid
                            // the same enemy beeing added multiple times in the same list.
                            if (!_actor.VisibleEnemies.Contains(WorldInfo.Marines[i]))
                            {
                                _actor.VisibleEnemies.Add(WorldInfo.Marines[i]);
                            }
                        }
                    }

                    // If VisibleEnemies is 0 we have lost sight of the enemie(s)
                    if (_actor.VisibleEnemies.Count == 0)
                    {
                        _actor.EnemyInSight = false;
                        return BbbtBehaviourStatus.Failure;
                    }
                    else
                    {
                        Debug.Log("Drone spotted " + _actor.VisibleEnemies.Count + " Number of players");
                        return BbbtBehaviourStatus.Success;
                    }


                case Factions.Elders:

                    // If VisibleEnemies is 0 we have lost sight of the enemie(s)
                    if (_actor.VisibleEnemies.Count == 0)
                    {
                        _actor.EnemyInSight = false;
                        return BbbtBehaviourStatus.Failure;
                    }
                    else
                    {
                        Debug.Log("Drone spotted " + _actor.VisibleEnemies.Count + " Number of players");
                        return BbbtBehaviourStatus.Success;
                    }

                case Factions.Marine:


                    // If VisibleEnemies is 0 we have lost sight of the enemie(s)
                    if (_actor.VisibleEnemies.Count == 0)
                    {
                        _actor.EnemyInSight = false;
                        return BbbtBehaviourStatus.Failure;
                    }
                    else
                    {
                        Debug.Log("Drone spotted " + _actor.VisibleEnemies.Count + " Number of players");
                        return BbbtBehaviourStatus.Success;
                    }

                default:
                    return BbbtBehaviourStatus.Failure;
            }
        }
    }
}