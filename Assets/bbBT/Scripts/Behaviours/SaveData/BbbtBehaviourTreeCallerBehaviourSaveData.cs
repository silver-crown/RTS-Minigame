using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Contains serializable save data for BbbtBehaviourTreeCallerBehaviour.
    /// </summary>
    public class BbbtBehaviourTreeCallerBehaviourSaveData : BbbtBehaviourSaveData
    {
        /// <summary>
        /// The name of the save data type used for identifying type during serialisation.
        /// </summary>
        public override string SaveDataType { get; } = "BbbtBehaviourTreeCallerBehaviourSaveData";

        /// <summary>
        /// The name of the behaviour tree to call.
        /// </summary>
        public string BehaviourTreeName { get; protected set; } = "";


        /// <summary>
        /// Constructs a new BbbtBehaviourTreeCallerBehaviourSaveData object.
        /// </summary>
        /// <param name="nodeId">The id of the node the behaviour belongs to in the editor.</param>
        /// <param name="behaviourTreeName">The name of the behaviour tree to call.</param>
        public BbbtBehaviourTreeCallerBehaviourSaveData(int nodeId, string behaviourTreeName)
        {
            NodeId = nodeId;
            BehaviourTreeName = behaviourTreeName;
        }

        /*
        /// <summary>
        /// Deserializes the save data.
        /// </summary>
        /// <returns>The object represented by the save data.</returns>
        public override BbbtBehaviour Deserialize()
        {
            var behaviour = ScriptableObject.CreateInstance<BbbtBehaviourTreeCallerBehaviour>();
            behaviour.LoadSaveData(this);
            return behaviour;
        }
        */
    }
}