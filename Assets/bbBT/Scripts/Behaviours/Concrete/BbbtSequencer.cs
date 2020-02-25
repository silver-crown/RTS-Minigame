﻿using UnityEngine;

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
        protected override void OnTerminate(BbbtBehaviourStatus status)
        {
        }

        /// <summary>
        /// Runs through the sequencers child nodes until they have been completed or one fails.
        /// </summary>
        /// <returns>
        /// Running if a child is running. Failure if a child failed. Success if all the children ran succesfully.
        /// </returns>
        protected override BbbtBehaviourStatus UpdateBehavior()
        {
            // Iterate through each child behaviour until we find one that's running or returned failure.
            // The assumption is that if a child is not in one of those states then it must have successfully
            // ran in the past, so we only care about the first behaviour that returns running or failure.
            foreach (var child in Children)
            {
                var status = child.Tick();
                if (status == BbbtBehaviourStatus.Running || status == BbbtBehaviourStatus.Failure)
                {
                    return status;
                }
            }
            // Successfully went through child behaviours.
            return BbbtBehaviourStatus.Success;
        }

        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <returns>The generated save data.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            var childSaveData = new BbbtBehaviourSaveData[Children.Count];
            for (int i = 0; i < Children.Count; i++)
            {
                childSaveData[i] = Children[i].ToSaveData();
            }
            return new BbbtSequencerSaveData(NodeId, childSaveData);
        }

        /// <summary>
        /// Sets up the behaviour from save data.
        /// </summary>
        /// <param name="saveData">The save data to use for setting up the behaviour.</param>
        public override void LoadSaveData(BbbtBehaviourSaveData saveData)
        {
            base.LoadSaveData(saveData);
            Children = null;
            var castSaveData = saveData as BbbtSequencerSaveData;
            if (castSaveData != null)
            {
                foreach (var childSaveData in castSaveData.ChildSaveData)
                {
                    AddChild(childSaveData.Deserialize());
                }
            }
            else
            {
                Debug.LogError("Save data passed to BbbtSequencer was not BbbtSequencerSaveData.");
            }
        }
    }
}