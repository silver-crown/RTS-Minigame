using Newtonsoft.Json;
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
        public override string SaveDataType { get; } = "BbbtRoot";

        /// <summary>
        /// The node that the BbbtRoot points to as the real root of the behaviour tree.
        /// </summary>
        [JsonProperty] public BbbtBehaviour Child { get; protected set; }

        /// <summary>
        /// BbbtRoot doesn't have any initialisation logic.
        /// This should never be called as the root node isn't actually a part of a built behaviour tree.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override void OnInitialize(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// BbbtRoot doesn't have any termination logic.
        /// This should never be called as the root node isn't actually a part of a built behaviour tree.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// BbbtRoot doesn't have any update logic.
        /// This should never be called as the root node isn't actually a part of a built behaviour tree.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override BbbtBehaviourStatus UpdateBehavior(GameObject gameObject)
        {
            throw new System.NotImplementedException();
        }

        /*
        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <returns>The save data object.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            return new BbbtRootSaveData(NodeId, Child != null ? Child.ToSaveData() : null);
        }

        /// <summary>
        /// Sets up the behaviour from save data.
        /// </summary>
        /// <param name="saveData">The save data to use for setting up the behaviour.</param>
        public override void LoadSaveData(BbbtBehaviourSaveData saveData)
        {
            base.LoadSaveData(saveData);
            var castSaveData = saveData as BbbtRootSaveData;
            if (castSaveData != null)
            {
                if (castSaveData.ChildSaveData != null)
                {
                    Child = castSaveData.ChildSaveData.Deserialize();
                }
            }
            else
            {
                Debug.LogError("Save data passed to BbbtRoot was not BbbtBehaviourSaveData.");
            }
        }
        */

        /// <summary>
        /// Adds a child to the node.
        /// </summary>
        /// <param name="child">The child to add.</param>
        public override void AddChild(BbbtBehaviour child)
        {
            Child = child;
        }
        
        /// <summary>
         /// Removes all of the behaviour's children.
         /// </summary>
        public override void RemoveChildren()
        {
            Child = null;
        }
    }
}