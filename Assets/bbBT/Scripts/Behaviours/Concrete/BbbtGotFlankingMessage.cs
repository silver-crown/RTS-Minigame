﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
namespace Bbbt
{

    ///  // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Got Flanking Message", menuName = "bbBT/Behaviour/Leaf/Got Flanking Message", order = 0)]
    /// <summary>
    /// Leaf node for checking if there's enough forces to risk attacking an enemy
    /// </summary>
    public class BbbtGotFlankingMessage : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private Drone _drone;

        public override string SaveDataType { get; } = "BbbtGotFlankingMessage";

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
            //Get the attack message from CI and return success, assuming it's the newest message received from CI           
            if (_drone.messageList[_drone.messageList.Count - 1] == "Flanking Assault")
            {
                return BbbtBehaviourStatus.Success;
            }
            return BbbtBehaviourStatus.Failure;
        }
    }
}
