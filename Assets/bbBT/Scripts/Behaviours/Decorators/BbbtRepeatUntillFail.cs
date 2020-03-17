using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "RepeatUntillFail",
        menuName = "bbBT/Behaviour/Decorator/RepeatUntillFail",
        order = 0)]

    /// <summary>
    /// Repeats the child nodes untills it returns BbbtBehaviourStatus.Failure
    /// </summary>
    public class BbbtRepeatUntillFail : BbbtDecoratorBehaviour
    {
        public override string SaveDataType { get; } = "BbbtRepeatUntillFail";

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
            BbbtBehaviourStatus childStatus;
            while(true)
            {
                childStatus = Child.Tick(gameObject);

                if(childStatus == BbbtBehaviourStatus.Failure)
                {
                    return BbbtBehaviourStatus.Success;
                }

                if(childStatus == BbbtBehaviourStatus.Running)
                {
                    return BbbtBehaviourStatus.Running;
                }
            }
        }
    }
}