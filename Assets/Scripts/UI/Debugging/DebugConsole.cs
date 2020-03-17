using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.Debugging
{
    /// <summary>
    /// Displays messages from DebugLogger.
    /// </summary>
    public class DebugConsole : MonoBehaviour
    {
        [Tooltip("The prefab for entries that will appear in the console." +
            "Must contain a DebugConsoleEntry and Text component.")]
        [SerializeField] private GameObject _entryPrefab;
        [Tooltip("The area where debug entries will appear. Must have a Vertical Layout Group.")]
        [SerializeField] private Transform _textArea;

        private List<DebugConsoleEntry> _entries = new List<DebugConsoleEntry>();

        private void Awake()
        {
            if (_entryPrefab == null)
            {
                Debug.LogError(GetType().Name + ": _entryPrefab was null. Set Entry Prefab in the inspector.");
            }
            else if (_entryPrefab.GetComponent<DebugConsoleEntry>() == null)
            {
                Debug.LogError(
                    GetType().Name + ": _entryPrefab had no DebugConsoleEntry component. " +
                    "Add the component to the prefab in the inspector.");
            }
            if (_textArea == null)
            {
                Debug.LogError(GetType().Name + ": _entryPrefab was null. Set Entry Prefab in the inspector.", this);
            }
        }

        private void Update()
        {
        }

        public void AddEntry(object message, object context)
        {
            var entry = Instantiate(_entryPrefab, _textArea).GetComponent<DebugConsoleEntry>();
            entry.Message = message.ToString();
            entry.Context = context;
            _entries.Add(entry);
        }
    }
}