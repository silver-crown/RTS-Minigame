using Commands;
using System.Collections.Generic;

namespace Bbbt.Commands
{
    /// <summary>
    /// Command which removes a node in the bbBT editor.
    /// </summary>
    public class RemoveNodeCommand : Command
    {
        /// <summary>
        /// The window in which to remove a node.
        /// </summary>
        private BbbtWindow _window;
        
        /// <summary>
        /// The node to add.
        /// </summary>
        private BbbtNode _node;

        /// <summary>
        /// The connections that need to be removed along with the node.
        /// </summary>
        private List<BbbtConnection> _connections;


        /// <summary>
        /// Creates a new RemoveNodeCommand instance.
        /// </summary>
        public RemoveNodeCommand(BbbtWindow window, BbbtNode node, List<BbbtConnection> connections)
        {
            _window = window;
            _node = node;
            _connections = connections;
        }

        public override void Do()
        {
            // Remove connections
            foreach (var connection in _connections)
            {
                _window.CurrentTab.Connections.Remove(connection);
            }

            // Remove node
            _window.CurrentTab.Nodes.Remove(_node);

            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }

        public override void Undo()
        {
            // Add back node
            _window.CurrentTab.Nodes.Add(_node);

            // Add back connections
            foreach (var connection in _connections)
            {
                _window.CurrentTab.Connections.Add(connection);
            }

            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }
    }
}