using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Contains serializable save data for BbbtDebugLogBehaviour.
    /// </summary>
    public class BbbtDebugLogBehaviourSaveData : BbbtBehaviourSaveData
    {
        /// <summary>
        /// The name of the save data type used for identifying type during serialisation.
        /// </summary>
        public override string SaveDataType { get; } = "BbbtDebugLogBehaviourSaveData";

        /// <summary>
        /// The message to be displayed by the behaviour.
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// The type of message to log.
        /// </summary>
        public LogType LogType { get; protected set; }


        /// <summary>
        /// Constructs a new BbbtDebugLogBehaviourSaveData object.
        /// </summary>
        /// <param name="nodeId">The id of the node the behaviour belongs to in the editor.</param>
        /// <param name="message">The message to be displayed by the behaviour.</param>
        /// <param name="logType">The type of message to log.</param>
        public BbbtDebugLogBehaviourSaveData(int nodeId, string message, LogType logType)
        {
            NodeId = nodeId;
            Message = message;
            LogType = logType;
        }

        /*
        /// <summary>
        /// Deserializes the save data.
        /// </summary>
        /// <returns>The object represented by the save data.</returns>
        public override BbbtBehaviour Deserialize()
        {
            var behaviour = ScriptableObject.CreateInstance<BbbtDebugLogBehaviour>();
            behaviour.LoadSaveData(this);
            return behaviour;
        }
        */
    }
}