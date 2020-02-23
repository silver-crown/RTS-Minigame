﻿using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// The return statuses possible from a behaviour's execution.
    /// </summary>
    public enum BbbtBehaviorStatus
    {
        /// <summary>
        /// The behaviour could not be run?
        /// </summary>
        Invalid,
        /// <summary>
        /// The behaviour completed succesfully.
        /// </summary>
        Success,
        /// <summary>
        /// The behaviour failed.
        /// </summary>
        Failure,
        /// <summary>
        /// The behaviour is running.
        /// </summary>
        Running,
        /// <summary>
        /// The behaviour is suspended (i.e. not running temporarily)
        /// </summary>
        Suspended,
        /// <summary>
        /// The behaviour aborted without failing?
        /// </summary>
        Aborted
    }

    /// <summary>
    /// A behaviour to be attached to a BbbtNode.
    /// </summary>
    public abstract class BbbtBehaviour : ScriptableObject
    {
        /// <summary>
        /// The current status of the behaviour.
        /// </summary>
        public BbbtBehaviorStatus Status { get; protected set; } = BbbtBehaviorStatus.Invalid;


        /// <summary>
        /// Tick gets called every time the behaviour tree reaches the node containing this behaviour.
        /// </summary>
        /// <returns>The status of the behaviour tree.</returns>
        public BbbtBehaviorStatus Tick()
        {
            if (Status != BbbtBehaviorStatus.Running)
            {
                // Behaviour hasn't started running.
                OnInitialize();
            }

            Status = UpdateBehavior();

            if (Status != BbbtBehaviorStatus.Running)
            {
                // Behaviour finished running.
                OnTerminate(Status);
            }

            return Status;
        }

        /// <summary>
        /// OnInitialize is called once, immediately before the first call to the behavior’s update method
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// Updace is called once each time the behavior tree is updated,
        /// until it signals it has terminated thanks to its return status
        /// </summary>
        /// <returns>The status of the behaviour.</returns>
        protected abstract BbbtBehaviorStatus UpdateBehavior();

        /// <summary>
        /// OnTerminate is called once, immediately after the previous update signals it’s no longer running.
        /// </summary>
        /// <param name="status">The behaviour's status upoin termination.</param>
        protected abstract void OnTerminate(BbbtBehaviorStatus status);

        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <returns>The generated save data.</returns>
        public abstract BbbtBehaviourSaveData ToSaveData();

        /// <summary>
        /// Sets up the behaviour from save data.
        /// </summary>
        /// <param name="saveData">The save data to use for setting up the behaviour.</param>
        public abstract void LoadSaveData(BbbtBehaviourSaveData saveData);

        /// <summary>
        /// Resets the behaviour to its initial state.
        /// </summary>
        public void Reset()
        {
            Status = BbbtBehaviorStatus.Invalid;
        }

        /// <summary>
        /// Gracefully aborts the behaviour.
        /// </summary>
        public void Abort()
        {
            OnTerminate(BbbtBehaviorStatus.Aborted);
            Status = BbbtBehaviorStatus.Aborted;
        }

        /// <summary>
        /// Checks whether the behaviour has terminated.
        /// </summary>
        /// <returns>True if the behaviour is terminated, false otherwise.</returns>
        public bool IsTerminated()
        {
            return Status == BbbtBehaviorStatus.Success || Status == BbbtBehaviorStatus.Failure;
        }

        /// <summary>
        /// Tries to find a behaviour that matches a given query.
        /// </summary>
        /// <param name="query">
        /// The string matching the name of the behaviour to find.
        /// Has to match the name of the behaviour exactly as instantiated in the assets folder.
        /// </param>
        /// <returns>The behaviour that maches the query, if found. Null otherwise.</returns>
        public static BbbtBehaviour FindBehaviourWithName(string query)
        {
            // Try to find a behaviour with name matching the query.
            var guids = AssetDatabase.FindAssets(query);
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string file = Path.GetFileNameWithoutExtension(path);
                if (file == query)
                {
                    var behaviour = AssetDatabase.LoadAssetAtPath<BbbtBehaviour>(path);
                    if (behaviour != null)
                    {
                        // Found a behaviour matching the query.
                        return AssetDatabase.LoadAssetAtPath<BbbtBehaviour>(path);
                    }
                }
            }

            // No behaviour found with name matching the query.
            return null;
        }
    }
}