using Commands;

namespace Bbbt.Commands
{
    /// <summary>
    /// Command which removes a connection in the bbBT editor.
    /// </summary>
    public class RemoveConnectionCommand : Command
    {
        /// <summary>
        /// The window in which to remove the connection.
        /// </summary>
        private BbbtWindow _window;

        /// <summary>
        /// The connection to remove.
        /// </summary>
        private BbbtConnection _connection;


        /// <summary>
        /// Creates a new RemoveConnectionCommand instance.
        /// </summary>
        /// <param name="window">The window in which to add the connection.</param>
        /// <param name="connection">The connection to add.</param>
        public RemoveConnectionCommand(BbbtWindow window, BbbtConnection connection)
        {
            _window = window;
            _connection = connection;
        }

        public override void Do()
        {
            _window.CurrentTab.Connections.Remove(_connection);
            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }

        public override void Undo()
        {
            _window.CurrentTab.Connections.Add(_connection);
            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }
    }
}