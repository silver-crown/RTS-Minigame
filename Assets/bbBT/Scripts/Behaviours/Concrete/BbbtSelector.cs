using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Behaviour which selects the leftmost valid child for execution.
    /// </summary>
    [CreateAssetMenu(fileName = "Selector", menuName =  "bbBT/Behaviour/Composite/Selector", order = 0)]
    public class BbbtSelector : BbbtCompositeBehaviour
    {
        /// <summary>
        /// BbbtSelector doesn't use any initialisation logic.
        /// </summary>
        protected override void OnInitialize()
        {
        }

        /// <summary>
        /// BbbtSelector doesn't use any termination logic.
        /// </summary>
        /// <param name="status">The status of the behaviour upon termination.</param>
        protected override void OnTerminate(BbbtBehaviorStatus status)
        {
        }

        /// <summary>
        /// Executes the first runnable child behaviour.
        /// </summary>
        /// <returns>The status of the child that was run, or Invalid if none was found.</returns>
        protected override BbbtBehaviorStatus UpdateBehavior()
        {
            // Iterate through the children and return the status of the first child that
            // returned Running or Success.
            foreach (var child in _children)
            {
                var status = child.Tick();
                if (status == BbbtBehaviorStatus.Running || status == BbbtBehaviorStatus.Success)
                {
                    // Found a child that
                    return status;
                }
            }
            // No child could be run.
            return BbbtBehaviorStatus.Invalid;
        }

        /// <summary>
        /// Converts the behaviour to save data.
        /// Selector nodes don't have save data, so the conversion doesn't do anything useful.
        /// </summary>
        /// <returns>Null.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            return null;
        }

        /// <summary>
        /// Sets up the behaviour from save data.
        /// </summary>
        /// <param name="saveData">The save data to use for setting up the behaviour.</param>
        public override void LoadSaveData(BbbtBehaviourSaveData saveData)
        {
        }
    }
}