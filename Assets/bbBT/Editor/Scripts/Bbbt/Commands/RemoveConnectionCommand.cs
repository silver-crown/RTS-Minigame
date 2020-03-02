using Commands;

namespace Bbbt.Commands
{
    /// <summary>
    /// Command which creates a connection in the bbBT editor.
    /// </summary>
    public class CreateConnectionCommand : Command
    {
        /// <summary>
        /// The window in which to add the connection.
        /// </summary>
        private BbbtWindow _window;

        /// <summary>
        /// The connection to add.
        /// </summary>
        private BbbtConnection _connection;


        /// <summary>
        /// Creates a new CreateConnectionCommand instance.
        /// </summary>
        /// <param name="window">The window in which to add the connection.</param>
        /// <param name="connection">The connection to add.</param>
        public CreateConnectionCommand(BbbtWindow window, BbbtConnection connection)
        {
            _window = window;
            _connection = connection;
        }

        public override void Do()
        {
            _window.CurrentTab.Connections.Add(_connection);
            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }

        public override void Undo()
        {
            _window.CurrentTab.Connections.Remove(_connection);
            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }
    }
}