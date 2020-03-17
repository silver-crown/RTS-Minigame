using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

namespace Bbbt
{
    // Creates the menu option in the unity engine
    [CreateAssetMenu(
        fileName = "Repeater",
        menuName = "bbBT/Behaviour/Decorator/Repeater",
        order = 0)]

    /// <summary>
    /// Repeats a child node for a set amount of times
    /// </summary>
    public class BbbtRepeater : BbbtDecoratorBehaviour
    {
        /// <summary>
        /// Number of times to repeat the child succsessfully
        /// </summary>
        [JsonProperty, SerializeField] private int Count;

        /// <summary>
        /// how many time the child has repeated
        /// </summary>
        private int _successCount;

        public override string SaveDataType { get; } = "BbbtRepeater";

        protected override void OnInitialize(GameObject gameObject)
        {
            Count = 0;
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            
        }

        /// <summary>
        /// Executes the repeaters child node untill it has succsessfully executed a set number of times (Count variable).
        /// </summary>
        /// <param name="gameObject">Game object that owns the behaviour.</param>
        /// <returns>
        ///     BbbtBehaviourStatus.Running : if it's child is still running
        ///     BbbtBehaviourStatus.Failure : if it gets failure from its child a single time
        ///     BbbtBehaviourStatus.Success : this means the child has succsessfully updated 'Count' number of times.
        /// </returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            while(true)
            {
                BbbtBehaviourStatus childStatus = Child.Tick(gameObject); 

                if (childStatus == BbbtBehaviourStatus.Running)
                {
                    return BbbtBehaviourStatus.Running;
                }

                if(childStatus == BbbtBehaviourStatus.Failure)
                {
                    return BbbtBehaviourStatus.Failure;
                }

                if(childStatus == BbbtBehaviourStatus.Success)
                {
                    Debug.Log("Updated Repeater Succsessfully");
                    _successCount++;
                }

                if(Count == _successCount)
                {
                    return BbbtBehaviourStatus.Success; 
                }
            }
        }
    }
}