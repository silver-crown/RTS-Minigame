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
        /// The CommandManager that this command history browser operated with.
        /// </summary>
        public CommandManager CommandManager { get; protected set; }

        /// <summary>
        /// List of the names of commands that have been performed.
        /// </summary>
        public List<Command> DoneCommands { get; protected set; }

        /// <summary>
        /// List of the names of commands that have been undone.
        /// </summary>
        public List<Command> UndoneCommands { get; protected set; }

        /// <summary>
        /// The last performed command last time the tab was saved.
        /// </summary>
        public Command LastSaveCommand { get; set; } = null;


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
            DoneCommands = new List<Command>();
            UndoneCommands = new List<Command>();

            // Listen to commandManager's events.
            commandManager.OnDo += (command) =>
            {
                DoneCommands.Add(command);
                UndoneCommands = new List<Command>();
                GUI.changed = true;
            };
            commandManager.OnRedo += (command) =>
            {
                DoneCommands.Add(command);
                UndoneCommands.RemoveAt(0);
                GUI.changed = true;
            };
            commandManager.OnUndo += (command) =>
            {
                UndoneCommands.Insert(0, command);
                DoneCommands.RemoveAt(DoneCommands.Count - 1);
                GUI.changed = true;

                // If we remove the last reset command just add it back in.
                if (command as LastResetCommand != null)
                {
                    commandManager.Redo();
                }
            };
        }
    }
}