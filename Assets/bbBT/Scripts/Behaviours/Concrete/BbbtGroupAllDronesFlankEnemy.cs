﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{
    /// <summary>
    /// Decorator node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    [CreateAssetMenu(fileName = "Group All Drones Flank Enemy", menuName = "bbBT/Behaviour/Leaf/Group All Drones Flank Enemy", order = 0)]
    public class BbbtGroupAllDronesFlankEnemy : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private GroupLeader _group;

        public override string SaveDataType { get; } = "BbbtGroupAllDronesFlankEnemy";

        protected override void OnInitialize(GameObject gameObject)
        {
            if (gameObject.GetComponent<Actor>() != null)
            {
                _actor = gameObject.GetComponent<Actor>();
                if (_actor.GetComponent<GroupLeader>() != null)
                {
                    _group = _actor.GetComponent<GroupLeader>();
                }
            }
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //send the attack message to the drones in the group and return success

            //*************************************************************************************************************
            //A four-way encirclement flank, need to find a way to select flank types in here, probably a utility AI thing.
            //*************************************************************************************************************

            //Divide the group into four units
            _group.DivideArmy(4);

            //send the individual units in the direction you want

            //Get the Alpha unit, and send them to the frontlines
            for(int i = 0; i <= _group.GetGroupUnits(Drone.GroupUnit.Alpha).Count; i++)
            {
                _actor.GetComponent<SendMessageToChannel>().Send("Flanking Assault Frontal", EventManager.MessageChannel.privateChannel,
                    _group.GetGroupUnits(Drone.GroupUnit.Alpha)[i].GetComponent<Drone>().ID);
            }
            //Get the Bravo unit, send them left
            for (int i = 0; i <= _group.GetGroupUnits(Drone.GroupUnit.Bravo).Count; i++)
            {
                _actor.GetComponent<SendMessageToChannel>().Send("Flanking Assault Left", EventManager.MessageChannel.privateChannel,
                    _group.GetGroupUnits(Drone.GroupUnit.Bravo)[i].GetComponent<Drone>().ID);
            }

            //Get the Charlie unit, send them right
            for (int i = 0; i <= _group.GetGroupUnits(Drone.GroupUnit.Charlie).Count; i++)
            {
                _actor.GetComponent<SendMessageToChannel>().Send("Flanking Assault Right", EventManager.MessageChannel.privateChannel,
                    _group.GetGroupUnits(Drone.GroupUnit.Charlie)[i].GetComponent<Drone>().ID);
            }
            //Send the Delta unit behind the enemies 
            for (int i = 0; i <= _group.GetGroupUnits(Drone.GroupUnit.Delta).Count; i++)
            {
                _actor.GetComponent<SendMessageToChannel>().Send("Flanking Assault Behind", EventManager.MessageChannel.privateChannel,
                    _group.GetGroupUnits(Drone.GroupUnit.Delta)[i].GetComponent<Drone>().ID);
            }

            return BbbtBehaviourStatus.Success;
        }
    }
}
