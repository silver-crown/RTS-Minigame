using JsonSubTypes;
using Newtonsoft.Json;
using System;

namespace Bbbt
{
    /// <summary>
    /// Abstract class for storing BbbtBehaviour save data.
    /// </summary>
    [JsonConverter(typeof(JsonSubtypes), "SaveDataType")]
    public abstract class BbbtBehaviourSaveData
    {
        /// <summary>
        /// The name of the save data type used for identifying type during serialisation.
        /// </summary>
        public abstract string SaveDataType { get; }

        /// <summary>
        /// The id of the node that contains the behaviour in the editor.
        /// We need this to get direct behaviour references when opening behaviour trees in the editor in debug mode.
        /// </summary>
        public int NodeId { get; set; }

        /// <summary>
        /// Deserializes the save data.
        /// </summary>
        /// <returns>The object represented by the save data.</returns>
        public abstract BbbtBehaviour Deserialize();
    }
}