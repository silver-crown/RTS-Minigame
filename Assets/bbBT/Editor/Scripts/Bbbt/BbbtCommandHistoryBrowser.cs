using Bbbt.Commands;
using Commands;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Lets the user browse the commands they have performed in the editor.
    /// </summary>
    public class BbbtCommandHistoryBrowser
    {
        /// <summary>
        /// Maps command types to the string to be displayed in the command history browser.
        /// </summary>
        private static Dictionary<Type, string> _commandStrings = new Dictionary<Type, string>()
        {
            { typeof(CreateConnectionCommand), "Create Connection" },
            { typeof(CreateNodeCommand), "Create Node" },
            { typeof(LastResetCommand), "Last Reset" },
            { typeof(MoveNodeCommand), "Move Node" },
            { typeof(RemoveConnectionCommand), "Remove Connection" },
            { typeof(RemoveNodeCommand), "Remove Node" },
        };

        /// <summary>
        /// The CommandManager that this command history browser operated with.
        /// </summary>
        private CommandManager _commandManager;

        /// <summary>
        /// List of the names of commands that have been performed.
        /// </summary>
        private List<string> _doneCommands;

        /// <summary>
        /// List of the names of commands that have been undone.
        /// </summary>
        private List<string> _undoneCommands;

        /// <summary>
        /// The style of done command labels.
        /// </summary>
        private GUIStyle _doneCommandsStyle;

        /// <summary>
        /// The style of undone command labels.
        /// </summary>
        private GUIStyle _undoneCommandsStyle;


        /// <summary>
        /// Creates a new BbbtCommandHistoryBrowser.
        /// </summary>
        /// <param name="commandManager">The command manager to listen to for done/undone commands.</param>
        public BbbtCommandHistoryBrowser(CommandManager commandManager)
        {
            // Store the CommandManager reference.
            _commandManager = commandManager;

            // Instantiate lists.
            // TODO: We want to replace these with more interactable entries at some point so we can click through
            // the history.
            _doneCommands = new List<string>();
            _undoneCommands = new List<string>();

            // Set up styles
            _doneCommandsStyle = new GUIStyle();
            _doneCommandsStyle.normal.textColor = Color.white;
            _undoneCommandsStyle = new GUIStyle();
            _undoneCommandsStyle.normal.textColor = Color.grey;

            // Listen to commandManager's events.
            commandManager.OnDo += (c) =>
            {
                _doneCommands.Add(_commandStrings[c.GetType()]);
                _undoneCommands = new List<string>();
                GUI.changed = true;
            };
            commandManager.OnRedo += (c) =>
            {
                _doneCommands.Add(_commandStrings[c.GetType()]);
                _undoneCommands.RemoveAt(0);
                GUI.changed = true;
            };
            commandManager.OnUndo += (c) =>
            {
                _undoneCommands.Insert(0, _commandStrings[c.GetType()]);
                _doneCommands.RemoveAt(_doneCommands.Count - 1);
                GUI.changed = true;
            };
        }

        /// <summary>
        /// Draws the command history browser.
        /// </summary>
        /// <param name="sidePanelRect">The rect of the panel which contains the browser.</param>
        public void Draw(Rect sidePanelRect)
        {
            float elementHeight = 20.0f;
            float x = sidePanelRect.x + 5.0f;
            int drawnElements = 0;

            // Display done commands.
            int commandsToUndo = 0;
            for (int i = 0; i < _doneCommands.Count; i++)
            {
                Rect rect = new Rect(x, drawnElements++ * elementHeight, sidePanelRect.width, elementHeight);
                if (GUI.Button(rect, _doneCommands[i], _doneCommandsStyle))
                {
                    // Undo till this command is the last done command.
                    commandsToUndo = _doneCommands.Count - i - 1;
                }
            }

            // Undo commands
            _commandManager.Undo(commandsToUndo);

            // Display undone commands.
            int commandsToRedo = 0;
            for (int i = 0; i < _undoneCommands.Count; i++)
            {
                Rect rect = new Rect(x, drawnElements++ * elementHeight, sidePanelRect.width, elementHeight);
                if (GUI.Button(rect, _undoneCommands[i], _undoneCommandsStyle))
                {
                    // Redo until this command is the last done command.
                    commandsToRedo = i + 1;
                }
            }

            // Redo commands
            _commandManager.Redo(commandsToRedo);
        }
    }
}