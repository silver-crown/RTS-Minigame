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
        /// Deserializes the save data.
        /// </summary>
        /// <returns>The object represented by the save data.</returns>
        public abstract BbbtBehaviour Deserialize();
    }
}