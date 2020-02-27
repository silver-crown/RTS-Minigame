using Commands;
using UnityEngine;

namespace Bbbt.Commands
{
    /// <summary>
    /// Command which moves a node in the bbBT editor.
    /// </summary>
    public class MoveNodeCommand : Command
    {
        /// <summary>
        /// The node to move.
        /// </summary>
        private BbbtNode _node;

        /// <summary>
        /// The old node position.
        /// </summary>
        private Vector2 _oldPosition;

        /// <summary>
        /// The new node position.
        /// </summary>
        private Vector2 _newPosition;


        /// <summary>
        /// Constructs a new MoveNodeCommand.
        /// </summary>
        /// <param name="node">The node to move.</param>
        /// <param name="oldPosition">The old node position.</param>
        /// <param name="newPosition">The new node position.</param>
        public MoveNodeCommand(BbbtNode node, Vector2 oldPosition, Vector2 newPosition)
        {
            _node = node;
            _oldPosition = oldPosition;
            _newPosition = newPosition;
        }

        public override void Do()
        {
            _node.SetPosition(_newPosition);
        }

        public override void Undo()
        {
            _node.SetPosition(_oldPosition);
        }
    }
}