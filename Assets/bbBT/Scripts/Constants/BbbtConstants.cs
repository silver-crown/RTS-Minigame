using System.IO;
using UnityEngine;

namespace Bbbt.Constants
{
    /// <summary>
    /// Contains bbBT constants.
    /// </summary>
    public static class BbbtConstants
    {
        /// <summary>
        /// The directory which contains behaviour tree json save data.
        /// </summary>
        public static string JsonDirectory { get; } = Path.Combine(
            Application.streamingAssetsPath,
            "AI",
            "BehaviourTrees",
            "Json");
    }
}