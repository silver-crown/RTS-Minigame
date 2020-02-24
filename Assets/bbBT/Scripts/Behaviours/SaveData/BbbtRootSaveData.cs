using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Contains serializable save data for BbbtRoot.
    /// </summary>
    public class BbbtRootSaveData : BbbtBehaviourSaveData
    {
        /// <summary>
        /// The name of the save data type used for identifying type during serialisation.
        /// </summary>
        public override string SaveDataType { get; } = "BbbtRootSaveData";

        /// <summary>
        /// The root behaviour's child.
        /// </summary>
        public BbbtBehaviourSaveData ChildSaveData { get; protected set; }

        /// <summary>
        /// Constructs a new BbbtRootSaveData object.
        /// </summary>
        /// <param name="childSaveData">The root behaviour's child.</param>
        public BbbtRootSaveData(BbbtBehaviourSaveData childSaveData)
        {
            ChildSaveData = childSaveData;
        }

        /// <summary>
        /// Deserializes the save data.
        /// </summary>
        /// <returns>The object represented by the save data.</returns>
        public override BbbtBehaviour Deserialize()
        {
            var behaviour = ScriptableObject.CreateInstance<BbbtRoot>();
            behaviour.LoadSaveData(this);
            return behaviour;
        }
    }
}