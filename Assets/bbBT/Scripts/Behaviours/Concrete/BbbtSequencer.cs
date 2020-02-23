using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Runs through its child behaviours sequentially.
    /// </summary>
    [CreateAssetMenu(fileName = "Sequencer", menuName = "bbBT/Behaviour/Composite/Sequencer", order = 0)]
    public class BbbtSequencer : BbbtCompositeBehaviour
    {
        /// <summary>
        /// BbbtSequencer does not have an OnInitialize implementation.
        /// </summary>
        protected override void OnInitialize()
        {
        }

        /// <summary>
        /// BbbtSequencer does not have an OnTerminate implementation.
        /// </summary>
        /// <param name="status"></param>
        protected override void OnTerminate(BbbtBehaviorStatus status)
        {
        }

        /// <summary>
        /// Runs through the sequencers child nodes until they have been completed or one fails.
        /// </summary>
        /// <returns>
        /// Running if a child is running. Failure if a child failed. Success if all the children ran succesfully.
        /// </returns>
        protected override BbbtBehaviorStatus UpdateBehavior()
        {
            // Iterate through each child behaviour until we find one that's running or returned failure.
            // The assumption is that if a child is not in one of those states then it must have successfully
            // ran in the past, so we only care about the first behaviour that returns running or failure.
            foreach (var child in _children)
            {
                var status = child.Tick();
                if (status == BbbtBehaviorStatus.Running || status == BbbtBehaviorStatus.Failure)
                {
                    return status;
                }
            }
            // Successfully went through child behaviours.
            return BbbtBehaviorStatus.Success;
        }

        /// <summary>
        /// Converts the behaviour to save data.
        /// Sequencer nodes don't have save data, so the conversion doesn't do anything useful.
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