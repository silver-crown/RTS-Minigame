using Commands;

namespace Bbbt.Commands
{
    /// <summary>
    /// Command which creates a node in the bbBT editor.
    /// </summary>
    public class CreateNodeCommand : Command
    {
        /// <summary>
        /// The window in which to add a node.
        /// </summary>
        private BbbtWindow _window;
        
        /// <summary>
        /// The node to add.
        /// </summary>
        private BbbtNode _node;


        /// <summary>
        /// Creates a new CreateNodeCommand instance.
        /// </summary>
        public CreateNodeCommand(BbbtWindow window, BbbtNode node)
        {
            _window = window;
            _node = node;
        }

        public override void Do()
        {
            _window.CurrentTab.Nodes.Add(_node);
            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }

        public override void Undo()
        {
            _window.CurrentTab.Nodes.Remove(_node);
            _window.SetUnsavedChangesTabTitle(_window.CurrentTab);
        }
    }
}