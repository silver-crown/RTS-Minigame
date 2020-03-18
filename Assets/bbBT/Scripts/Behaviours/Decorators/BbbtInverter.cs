using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{

    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Inverter",
        menuName = "bbBT/Behaviour/Decorator/Inverter",
        order = 0)]

    /// <summary>
    /// Inverters the result of the child node
    /// </summary>
    public class BbbtInverter : BbbtDecoratorBehaviour
    {
        public override string SaveDataType { get; } = "BbbtInverter";

        protected override void OnInitialize(GameObject gameObject)
        {
         
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
           
        }

        /// <summary>
        /// Inverts the result of the child behavior
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>
        ///     BbbtBehaviourStatus.Success : if the child fails
        ///     BbbtBehaviourStatus.Failure : if the child success
        ///     BbbtBehaviourStatus.Runnig  : child is running
        /// </returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            Debug.Log("I am trash man");
            BbbtBehaviourStatus childStatus = Child.Tick(gameObject);

            if(childStatus == BbbtBehaviourStatus.Failure)
            {
                return BbbtBehaviourStatus.Success;
            }

            if(childStatus == BbbtBehaviourStatus.Success)
            {
                return BbbtBehaviourStatus.Failure;
            }

            if(childStatus == BbbtBehaviourStatus.Running)
            {
                return BbbtBehaviourStatus.Running;
            }

            return childStatus;
        }
    }
}