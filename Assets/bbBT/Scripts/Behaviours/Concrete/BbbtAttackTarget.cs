﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Attack Target",
        menuName = "bbBT/Behaviour/Leaf/Attack Target",
        order = 0)]

    /// <summary>
    ///  Attacks the actors current target using the actors attack method.
    /// </summary>
    public class BbbtAttackTarget : BbbtLeafBehaviour
    {
        private RTS.Actor _actor;

        public override string SaveDataType { get; } = "BbbtAttackTarget";

        protected override void OnInitialize(GameObject gameObject)
        {
            _actor = gameObject.GetComponent<Actor>();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            Debug.Log("Attacked target!");

            _actor.Attack();

            return BbbtBehaviourStatus.Success;
        }
    }


}