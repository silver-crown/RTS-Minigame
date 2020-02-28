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
    public class BbbtTabCommandHistory
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
        public CommandManager CommandManager { get; protected set; }

        /// <summary>
        /// List of the names of commands that have been performed.
        /// </summary>
        public List<string> DoneCommands { get; protected set; }

        /// <summary>
        /// List of the names of commands that have been undone.
        /// </summary>
        public List<string> UndoneCommands { get; protected set; }


        /// <summary>
        /// Creates a new BbbtCommandHistoryBrowser.
        /// </summary>
        /// <param name="commandManager">The command manager to listen to for done/undone commands.</param>
        public BbbtTabCommandHistory(CommandManager commandManager)
        {
            // Store the CommandManager reference.
            CommandManager = commandManager;

            // Instantiate lists.
            // TODO: We want to replace these with more interactable entries at some point so we can click through
            // the history.
            DoneCommands = new List<string>();
            UndoneCommands = new List<string>();

            // Listen to commandManager's events.
            commandManager.OnDo += (c) =>
            {
                DoneCommands.Add(_commandStrings[c.GetType()]);
                UndoneCommands = new List<string>();
                GUI.changed = true;
            };
            commandManager.OnRedo += (c) =>
            {
                DoneCommands.Add(_commandStrings[c.GetType()]);
                UndoneCommands.RemoveAt(0);
                GUI.changed = true;
            };
            commandManager.OnUndo += (c) =>
            {
                UndoneCommands.Insert(0, _commandStrings[c.GetType()]);
                DoneCommands.RemoveAt(DoneCommands.Count - 1);
                GUI.changed = true;
            };
        }
    }
}