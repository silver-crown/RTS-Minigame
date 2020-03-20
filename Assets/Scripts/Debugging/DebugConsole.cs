using System;
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
        private static List<KeyValuePair<object, object>> _staticEntries = new List<KeyValuePair<object, object>>();
        private static Action<object, object> _onAddStaticEntry;
        private static Action _onClear;

        [SerializeField] private GameObject _entryPrefab = null;
        [SerializeField] private Transform _textArea = null;
        [SerializeField] private Scrollbar _scrollbar = null;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup = null;

        private List<DebugConsoleEntry> _entries = new List<DebugConsoleEntry>();
        float _timeRemainingWhenScrollbarShouldBeForcedToZero = 0.0f;

        private void Awake()
        {
            foreach (var pair in _staticEntries)
            {
                AddEntry(pair.Key, pair.Value);
            }
            _onAddStaticEntry += (object message, object context) => { AddEntry(message, context); };
            _onClear += RemoveEntries;
            DebugConsoleEntry.OnSetupComplete += (DebugConsoleEntry entry) =>
            {
                Canvas.ForceUpdateCanvases();
                _verticalLayoutGroup.enabled = false;
                _verticalLayoutGroup.enabled = true;
                _timeRemainingWhenScrollbarShouldBeForcedToZero = 0.5f;
            };
        }

        private void Update()
        {
            while (_timeRemainingWhenScrollbarShouldBeForcedToZero > 0)
            {
                _scrollbar.value = 0.0f;
                _timeRemainingWhenScrollbarShouldBeForcedToZero -= Time.deltaTime;
            }
        }

        private void AddEntry(object message, object context)
        {
            var entry = Instantiate(_entryPrefab, _textArea).GetComponent<DebugConsoleEntry>();
            entry.Message = message != null ? message.ToString() : "null";
            entry.Context = context;
            _entries.Add(entry);
        }

        private void RemoveEntries()
        {
            foreach (var entry in _entries)
            {
                Destroy(entry.gameObject);
            }
            _entries = new List<DebugConsoleEntry>();
        }

        public static void StaticAddEntry(object message, object context)
        {
            _staticEntries.Add(new KeyValuePair<object, object>(message, context));
            _onAddStaticEntry?.Invoke(message, context);
        }

        public static void Clear()
        {
            _staticEntries = new List<KeyValuePair<object, object>>();
            _onClear?.Invoke();
        }
    }
}