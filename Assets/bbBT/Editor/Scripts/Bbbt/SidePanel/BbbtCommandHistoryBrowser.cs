using Bbbt.Commands;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Displays the currently open tab's command history.
    /// </summary>
    public class BbbtCommandHistoryBrowser : BbbtSidePanelContent
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
        /// The window the browser is open in.
        /// </summary>
        private BbbtWindow _window = null;

        /// <summary>
        /// The style of done command buttons.
        /// </summary>
        private GUIStyle _doneCommandsStyle;

        /// <summary>
        /// The style of undone command buttons.
        /// </summary>
        private GUIStyle _undoneCommandsStyle;


        /// <summary>
        /// Creates an instance of BbbtCommandHistoryBrowser.
        /// </summary>
        /// <param name="window">The window the browser is open in.</param>
        public BbbtCommandHistoryBrowser(BbbtWindow window)
        {
            _window = window;

            // Set up styles
            _doneCommandsStyle = new GUIStyle();
            _doneCommandsStyle.normal.textColor = Color.white;
            _undoneCommandsStyle = new GUIStyle();
            _undoneCommandsStyle.normal.textColor = Color.grey;
        }

        public override void Draw(Rect rect)
        {
            if (_window.CurrentTab != null)
            {
                BbbtTabCommandHistory commandHistory = _window.CurrentTab.CommandHistory;
                float elementHeight = 20.0f;
                float x = rect.x + 5.0f;
                int drawnElements = 0;
                float width = rect.xMax - x - 5.0f;

                // Display done commands.
                int commandsToUndo = 0;
                var doneCommands = commandHistory.DoneCommands;
                for (int i = 0; i < doneCommands.Count; i++)
                {
                    Rect buttonRect = new Rect(x, drawnElements++ * elementHeight, rect.width, elementHeight);
                    string content = _commandStrings[doneCommands[i].GetType()];
                    if (doneCommands[i] == commandHistory.LastSaveCommand)
                    {
                        content += " (Last Save)";
                    }
                    content = Truncate(content, _doneCommandsStyle, width);
                    if (GUI.Button(buttonRect, content, _doneCommandsStyle))
                    {
                        // Undo till this command is the last done command.
                        commandsToUndo = doneCommands.Count - i - 1;
                    }
                }

                // Undo commands
                commandHistory.CommandManager.Undo(commandsToUndo);

                // Display undone commands.
                int commandsToRedo = 0;
                var undoneCommands = commandHistory.UndoneCommands;
                for (int i = 0; i < undoneCommands.Count; i++)
                {
                    Rect buttonRect = new Rect(x, drawnElements++ * elementHeight, rect.width, elementHeight);
                    string content = _commandStrings[undoneCommands[i].GetType()];
                    if (undoneCommands[i] == commandHistory.LastSaveCommand)
                    {
                        content += " (Last Save)";
                    }
                    content = Truncate(content, _undoneCommandsStyle, width);
                    if (GUI.Button(buttonRect, content, _undoneCommandsStyle))
                    {
                        // Redo until this command is the last done command.
                        commandsToRedo = i + 1;
                    }
                }

                // Redo commands
                commandHistory.CommandManager.Redo(commandsToRedo);
            }
        }

        /// <summary>
        /// Truncates a string to fit horizontally within a maximum width.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="style">The style to use for checking the string's width.</param>
        /// <param name="maxWidth">The maximum width of the string.</param>
        /// <returns>The truncated string.</returns>
        private string Truncate(string value, GUIStyle style, float maxWidth)
        {
            GUIContent content = new GUIContent(value);
            if (style.CalcSize(content).x > maxWidth)
            {
                content.text += "...";
            }

            while (style.CalcSize(content).x > maxWidth)
            {
                content.text = content.text.Remove(content.text.Length - 4, 1);
            }

            return content.text;
        }
    }
}