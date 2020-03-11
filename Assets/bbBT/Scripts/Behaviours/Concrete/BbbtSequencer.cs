using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Runs through its child behaviours sequentially.
    /// </summary>
    [CreateAssetMenu(fileName = "Sequencer", menuName = "bbBT/Behaviour/Composite/Sequencer", order = 0)]
    public class BbbtSequencer : BbbtCompositeBehaviour
    {
        private List<BbbtBehaviour> _completedChildren;

        public override string SaveDataType => "BbbtSequencer";

        /// <summary>
        /// BbbtSequencer does not have an OnInitialize implementation.
        /// </summary>
        protected override void OnInitialize(GameObject gameObject)
        {
            _completedChildren = new List<BbbtBehaviour>();
        }

        /// <summary>
        /// BbbtSequencer does not have an OnTerminate implementation.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        /// <param name="status"></param>
        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            _completedChildren = new List<BbbtBehaviour>();
        }

        /// <summary>
        /// Runs through the sequencers child nodes until they have been completed or one fails.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        /// <returns>
        /// Running if a child is running. Failure if a child failed. Success if all the children ran succesfully.
        /// </returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            foreach (var child in Children)
            {
                if (_completedChildren.Contains(child)) continue;
                var status = child.Tick(gameObject);
                if (status == BbbtBehaviourStatus.Success) _completedChildren.Add(child);
                if (status == BbbtBehaviourStatus.Running || status == BbbtBehaviourStatus.Failure)
                {
                    return status;
                }
            }
            // Successfully went through child behaviours.
            return BbbtBehaviourStatus.Success;
        }

        /*
        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        /// <returns>The generated save data.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            if (Children != null)
            {
                var childSaveData = new BbbtBehaviourSaveData[Children.Count];
                for (int i = 0; i < Children.Count; i++)
                {
                    childSaveData[i] = Children[i].ToSaveData();
                }
                return new BbbtSequencerSaveData(NodeId, childSaveData);
            }
            else
            {
                return new BbbtSequencerSaveData(NodeId, null);
            }
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
                if (castSaveData.ChildSaveData != null)
                {
                    foreach (var childSaveData in castSaveData.ChildSaveData)
                    {
                        AddChild(childSaveData.Deserialize());
                    }
                }
            }
            else
            {
                Debug.LogError("Save data passed to BbbtSequencer was not BbbtSequencerSaveData.");
            }
        }
        */
    }
}