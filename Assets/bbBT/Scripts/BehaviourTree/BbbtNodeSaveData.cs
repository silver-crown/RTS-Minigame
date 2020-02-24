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
        /// The name of the node's base behaviour.
        /// </summary>
        public string BaseBehaviour;

        /// <summary>
        /// The save data of the node's behaviour.
        /// </summary>
        public BbbtBehaviourSaveData BehaviourSaveData;

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
        /// Constructs a BbbtNodeSaveData object.
        /// </summary>
        /// <param name="id">A unique ID used to identify the node.</param>
        /// <param name="baseBehaviour">The name of the node's base behaviour.</param>
        /// <param name="behaviourSaveData">The save data of the node's behaviour.</param>
        /// <param name="x">The node's x position.</param>
        /// <param name="y">The node's y position.</param>
        /// <param name="isSelected">Whether the node is selected.</param>
        public BbbtNodeSaveData(
            int id,
            string baseBehaviour,
            BbbtBehaviourSaveData behaviourSaveData,
            float x,
            float y,
            bool isSelected)
        {
            Id = id;
            BaseBehaviour = baseBehaviour;
            BehaviourSaveData = behaviourSaveData;
            X = x;
            Y = y;
            IsSelected = isSelected;
        }
    }
}