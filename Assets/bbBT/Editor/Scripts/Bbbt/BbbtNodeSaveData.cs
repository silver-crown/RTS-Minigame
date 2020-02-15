using System;

namespace Bbbt
{
    /// <summary>
    /// Contains data needed to save and load a bbBT behaviour tree node.
    /// </summary>
    [Serializable]
    public class BbbtNodeSaveData
    {
        /// <summary>
        /// Unique ID used to identify the node.
        /// </summary>
        public int Id;

        /// <summary>
        /// The node's type, stored as a string so that enum ordering doesn't matter.
        /// </summary>
        public string Type;

        /// <summary>
        /// The node's X position.
        /// </summary>
        public float X;

        /// <summary>
        /// The node's Y position.
        /// </summary>
        public float Y;

        /// <summary>
        /// Whether the node is selected.
        /// </summary>
        public bool IsSelected;

        /// <summary>
        /// The label used to identify the action attached to the node.
        /// </summary>
        public string ActionLabel;


        /// <summary>
        /// Constructs a BbbtNodeSaveData object.
        /// </summary>
        /// <param name="id">A unique ID used to identify the node.</param>
        /// <param name="type">The node's type.</param>
        /// <param name="x">The node's x position.</param>
        /// <param name="y">The node's y position.</param>
        /// <param name="isSelected">Whether the node is selected.</param>
        /// <param name="actionLabel">The label used to identify the action attached to the node.</param>
        public BbbtNodeSaveData(int id, string type, float x, float y, bool isSelected, string actionLabel)
        {
            Id = id;
            Type = type;
            X = x;
            Y = y;
            IsSelected = isSelected;
            ActionLabel = actionLabel;
        }
    }
}