using System;
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
        /// Action invoked when CommandManager.Do() is called.
        /// </summary>
        public Action<Command> OnDo;

        /// <summary>
        /// Action invoked when CommandManager.Undo() is called with a non-empty undo stack.
        /// </summary>
        public Action<Command> OnUndo;

        /// <summary>
        /// Action invoked when CommandManager.Redo() is called with a non-empty redo stack.
        /// </summary>
        public Action<Command> OnRedo;


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
            OnDo?.Invoke(command);
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
                OnUndo?.Invoke(command);
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
                OnRedo?.Invoke(command);
            }
        }
    }
}