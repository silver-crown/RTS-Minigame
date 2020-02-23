using System;

namespace Bbbt
{
    /// <summary>
    /// Contains data needed to save and load a bbBT behaviour tree connection.
    /// </summary>
    [Serializable]
    public class BbbtConnectionSaveData
    {
        /// <summary>
        /// The id of the node to which the connection leads.
        /// </summary>
        public int InNodeId;

        /// <summary>
        /// The id of the node from which the connection leads.
        /// </summary>
        public int OutNodeId;


        /// <summary>
        /// Constructs a BbbtConnectionSaveData object.
        /// </summary>
        /// <param name="inNodeId">The id of the node to which the connection leads.</param>
        /// <param name="outNodeId">The id of the node from which the connection leads.</param>
        public BbbtConnectionSaveData(int inNodeId, int outNodeId)
        {
            InNodeId = inNodeId;
            OutNodeId = outNodeId;
        }
    }
}