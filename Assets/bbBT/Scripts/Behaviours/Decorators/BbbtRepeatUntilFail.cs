using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Repeat Until Fail",
        menuName = "bbBT/Behaviour/Decorator/Repeat Until Fail",
        order = 0)]

    /// <summary>
    /// Repeats the child nodes untills it returns BbbtBehaviourStatus.Failure
    /// </summary>
    public class BbbtRepeatUntilFail : BbbtDecoratorBehaviour
    {
        public override string SaveDataType { get; } = "BbbtRepeatUntilFail";

        protected override void OnInitialize(GameObject gameObject)
        {

        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            //Debug.Log("Mordi");
            BbbtBehaviourStatus childStatus;
            childStatus = Child.Tick(gameObject);

            if (childStatus == BbbtBehaviourStatus.Failure)
            {
                return BbbtBehaviourStatus.Success;
            }

            return BbbtBehaviourStatus.Running;
        }
    }
}