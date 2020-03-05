using Newtonsoft.Json;
using System;
using UnityEngine;

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
        /// The instance id of the node's base behaviour.
        /// </summary>
        public int BaseBehaviourInstanceId;

        /// <summary>
        /// The behaviour attached to the node.
        /// </summary>
        public BbbtBehaviour Behaviour;

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
        /// <param name="baseBehaviourInstanceId">The instance id of the node's base behaviour.</param>
        /// <param name="behaviour">The behaviour attached to the node.</param>
        /// <param name="x">The node's x position.</param>
        /// <param name="y">The node's y position.</param>
        /// <param name="isSelected">Whether the node is selected.</param>
        public BbbtNodeSaveData(
            int id,
            string baseBehaviour,
            int baseBehaviourInstanceId,
            BbbtBehaviour behaviour,
            float x,
            float y,
            bool isSelected)
        {
            Id = id;
            BaseBehaviour = baseBehaviour;
            BaseBehaviourInstanceId = baseBehaviourInstanceId;
            Behaviour = behaviour;
            X = x;
            Y = y;
            IsSelected = isSelected;
        }
    }
}