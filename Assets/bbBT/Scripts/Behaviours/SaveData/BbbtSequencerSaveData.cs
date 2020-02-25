using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Contains serializable save data for BbbtSequencer
    /// </summary>
    public class BbbtSequencerSaveData : BbbtBehaviourSaveData
    {
        /// <summary>
        /// The name of the save data type used for identifying type during serialisation.
        /// </summary>
        public override string SaveDataType { get; } = "BbbtSequencerSaveData";

        /// <summary>
        /// The behaviour's children.
        /// </summary>
        public BbbtBehaviourSaveData[] ChildSaveData { get; protected set; }

        /// <summary>
        /// Constructs a new BbbtSequencerSaveData object.
        /// </summary>
        /// <param name="nodeId">The id of the node the behaviour belongs to in the editor.</param>
        /// <param name="childSaveData">The behaviour's children.</param>
        public BbbtSequencerSaveData(int nodeId, BbbtBehaviourSaveData[] childSaveData)
        {
            NodeId = nodeId;
            ChildSaveData = childSaveData;
        }

        /// <summary>
        /// Deserializes the save data.
        /// </summary>
        /// <returns>The deserialised BbbtSelector.</returns>
        public override BbbtBehaviour Deserialize()
        {
            var behaviour = ScriptableObject.CreateInstance<BbbtSequencer>();
            behaviour.LoadSaveData(this);
            return behaviour;
        }
    }
}