﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(fileName = "Group Have Adequate Forces", menuName = "bbBT/Behaviour/Leaf/Group Have Adequate Forces", order = 0)]

    /// <summary>
    ///  Attacks the actors current target using the actors attack method.
    /// </summary>
    public class BbbtGroupHaveAdequateForces : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;
        private int _forces;

        public override string SaveDataType { get; } = "BbbtGroupHaveAdequateForces";

        protected override void OnInitialize(GameObject gameObject)
        {
            if(gameObject.GetComponent<Actor>() != null)
            {
                _actor = gameObject.GetComponent<Actor>();
                _forces = _actor.GetComponent<Group>().groupSize;
            }
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if(_forces > _actor.GetComponent<Group>().enemyList.Count)
            {
                return BbbtBehaviourStatus.Success;
            }
            return BbbtBehaviourStatus.Failure;
        }
    }


}