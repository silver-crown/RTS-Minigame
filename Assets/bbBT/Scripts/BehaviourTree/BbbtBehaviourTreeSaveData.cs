using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Data needed to serialise and deserialise a behaviour tree.
    /// </summary>
    public class BbbtBehaviourTreeSaveData
    {
        /// <summary>
        /// The data needed to load the root behaviour.
        /// </summary>
        public BbbtRootSaveData RootSaveData { get; protected set; }

        /// <summary>
        /// Constructs a new BbbtBehaviourTreeSaveData object.
        /// </summary>
        /// <param name="rootSaveData">The root behaviour's save data.</param>
        public BbbtBehaviourTreeSaveData(BbbtRootSaveData rootSaveData)
        {
            RootSaveData = rootSaveData;
        }
    }
}