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
        /// The window in which the node should move.
        /// </summary>
        private BbbtWindow _window;

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
        /// <param name="window">The window in which the node should move.</param>
        /// <param name="node">The node to move.</param>
        /// <param name="delta">The amount by which to move the node.</param>
        public MoveNodeCommand(BbbtWindow window, BbbtNode node, Vector2 delta)
        {
            _window = window;
            _node = node;
            _delta = delta;
        }

        public override void Do()
        {
            _node.Drag(_delta);
            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }

        public override void Undo()
        {
            _node.Drag(-_delta);
            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }
    }
}