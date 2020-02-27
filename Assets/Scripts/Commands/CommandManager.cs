using System.Collections.Generic;

namespace Commands
{
    /// <summary>
    /// Manages the undo/redo stack for undoing and redoing commands.
    /// </summary>
    public class CommandManager
    {
        /// <summary>
        /// Stack of commands that have been performed and can be undone.
        /// </summary>
        private Stack<Command> _undoStack;

        /// <summary>
        /// Stack of commands that have been undone and can be redone.
        /// </summary>
        private Stack<Command> _redoStack;


        /// <summary>
        /// Constructs a new CommandManager.
        /// </summary>
        public CommandManager()
        {
            _undoStack = new Stack<Command>();
            _redoStack = new Stack<Command>();
        }

        /// <summary>
        /// Performs a command and adds it to the CommandManager's undo stack.
        /// Note: This will also clear the redo stack as non-linear command histories are not supported.
        /// </summary>
        /// <param name="command">The command to be performed.</param>
        public void Do(Command command)
        {
            command.Do();
            _undoStack.Push(command);
            _redoStack = new Stack<Command>();
        }

        /// <summary>
        /// Undoes the last command handled by the CommandManager.
        /// </summary>
        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
            }
        }

        /// <summary>
        /// Redoes the last command undone by the CommandManager.
        /// </summary>
        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                command.Do();
                _undoStack.Push(command);
            }
        }
    }
}