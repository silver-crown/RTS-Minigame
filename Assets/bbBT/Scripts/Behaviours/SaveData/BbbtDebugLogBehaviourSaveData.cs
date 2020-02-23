using System;

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
        /// Constructs a new BbbtDebugLogBehaviourSaveData object.
        /// </summary>
        /// <param name="message">The message to be displayed by the behaviour.</param>
        public BbbtDebugLogBehaviourSaveData(string message)
        {
            Message = message;
        }
    }
}