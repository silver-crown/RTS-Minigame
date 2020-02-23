using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Marks the entry point of a behaviour tree. Inherits from BbbtBehaviour for convenience but isn't actually
    /// included in a behaviour tree, and is used in the editor to point to the real root node.
    /// </summary>
    [CreateAssetMenu(fileName = "Root", menuName = "bbBT/Behaviour/Root", order = 0)]
    public class BbbtRoot : BbbtBehaviour
    {
        /// <summary>
        /// The node that the BbbtRoot points to as the real root of the behaviour tree.
        /// </summary>
        public BbbtBehaviour Child { get; set; }

        /// <summary>
        /// BbbtRoot doesn't have any initialisation logic.
        /// This should never be called as the root node isn't actually a part of a built behaviour tree.
        /// </summary>
        protected override void OnInitialize()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// BbbtRoot doesn't have any termination logic.
        /// This should never be called as the root node isn't actually a part of a built behaviour tree.
        /// </summary>
        protected override void OnTerminate(BbbtBehaviorStatus status)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// BbbtRoot doesn't have any update logic.
        /// This should never be called as the root node isn't actually a part of a built behaviour tree.
        /// </summary>
        protected override BbbtBehaviorStatus UpdateBehavior()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Converts the behaviour to save data.
        /// Root nodes don't have save data, so the conversion doesn't do anything useful.
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