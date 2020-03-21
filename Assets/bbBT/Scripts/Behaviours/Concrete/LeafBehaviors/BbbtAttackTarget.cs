using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RTS;
using MoonSharp.Interpreter;

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

            if (_actor.Target == null)
            {
                return BbbtBehaviourStatus.Failure;
            }
            var attackRange = (float)_actor.GetValue("_attackRange").Number;
            if (Vector3.Distance(_actor.Target.transform.position, _actor.transform.position) > attackRange)
            {
                return BbbtBehaviourStatus.Failure;
            }

            var lastAttackTime = (float)_actor.GetValue("_lastAttackTime").Number;
            if (Time.time >= lastAttackTime + 1.0f / _actor.GetValue("_fireRate").Number)
            {
                _actor.Attack();
                _actor.SetValue("_lastAttackTime", DynValue.NewNumber(Time.time));
                return BbbtBehaviourStatus.Success;
            }
            else
            {
                return BbbtBehaviourStatus.Running;
            }
        }
    }


}