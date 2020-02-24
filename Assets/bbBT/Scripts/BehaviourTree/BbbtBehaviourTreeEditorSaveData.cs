namespace Bbbt
{
    /// <summary>
    /// Contains data needed to save and load a bbBT behaviour tree.
    /// </summary>
    public class BbbtBehaviourTreeEditorSaveData
    {
        /// <summary>
        /// The save data for the nodes belonging to the behaviour tree.
        /// </summary>
        public BbbtNodeSaveData[] Nodes;

        /// <summary>
        /// The save data for the connections between the nodes in the behaviour tree.
        /// </summary>
        public BbbtConnectionSaveData[] Connections;

        /// <summary>
        /// The amount by which the behaviour tree has been offset from the centre of the editor window in the x axis.
        /// </summary>
        public float WindowOffsetX;

        /// <summary>
        /// The amount by which the behaviour tree has been offset from the centre of the editor window in the y axis.
        /// </summary>
        public float WindowOffsetY;


        /// <summary>
        /// Constructs a BbbtBehaviourTreeSaveData object.
        /// </summary>
        /// <param name="nodes">The save data for the nodes belonging to the behaviour tree.</param>
        /// <param name="connections">
        /// The save data for the connections between the nodes in the behaviour tree.
        /// </param>
        /// <param name="windowOffsetX">
        /// The amount by which the behaviour tree has been offset from the centre of the editor window in the x axis.
        /// </param>
        /// <param name="windowOffsetY">
        /// The amount by which the behaviour tree has been offset from the centre of the editor window in the y axis.
        /// </param>
        public BbbtBehaviourTreeEditorSaveData(
            BbbtNodeSaveData[] nodes,
            BbbtConnectionSaveData[] connections,
            float windowOffsetX,
            float windowOffsetY)
        {
            Nodes = nodes;
            Connections = connections;
            WindowOffsetX = windowOffsetX;
            WindowOffsetY = windowOffsetY;
        }
    }
}