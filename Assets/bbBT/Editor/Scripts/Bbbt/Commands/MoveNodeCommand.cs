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
        private Vector2 _delta;


        /// <summary>
        /// Constructs a new MoveNodeCommand.
        /// </summary>
        /// <param name="node">The node to move.</param>
        /// <param name="delta">The amount by which to move the node.</param>
        public MoveNodeCommand(BbbtNode node, Vector2 delta)
        {
            _node = node;
            _delta = delta;
        }

        public override void Do()
        {
            _node.Drag(_delta);
        }

        public override void Undo()
        {
            _node.Drag(-_delta);
        }
    }
}