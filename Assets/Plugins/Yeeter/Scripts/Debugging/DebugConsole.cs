using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using Tyd;
using UnityEngine;
using UnityEngine.UI;

namespace Yeeter
{
    /// <summary>
    /// Displays messages from DebugLogger.
    /// </summary>
    public class DebugConsole : MonoBehaviour
    {
        private static string _theme;
        private static List<KeyValuePair<object, object>> _staticEntries = new List<KeyValuePair<object, object>>();
        private static Action<object, object> _onAddStaticEntry;
        private static Action _onSetTheme;
        private static Action _onClear;

        [SerializeField] private GameObject _entryPrefab = null;
        [SerializeField] private Transform _textArea = null;
        [SerializeField] private Scrollbar _scrollbar = null;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup = null;
        // For applying theme
        [SerializeField] private Text  _placeholder = null;
        [SerializeField] private Text  _text = null;
        [SerializeField] private Image _background = null;
        [SerializeField] private Image _scrollView = null;
        [SerializeField] private Image _commandInputField = null;
        [SerializeField] private Image _scrollbarBackground = null;
        [SerializeField] private Image _scrollbarHandle = null;

        private List<DebugConsoleEntry> _entries = new List<DebugConsoleEntry>();
        private float _timeRemainingWhenScrollbarShouldBeForcedToZero = 0.0f;

        private void Awake()
        {
            /*
            foreach (var pair in _staticEntries)
            {
                AddEntry(pair.Key, pair.Value);
            }
            */
            _onAddStaticEntry += (object message, object context) => { AddEntry(message, context); };
            DebugConsoleEntry.OnSetupComplete += (DebugConsoleEntry entry) =>
            {
                Canvas.ForceUpdateCanvases();
                if (_verticalLayoutGroup != null)
                {
                    _verticalLayoutGroup.enabled = false;
                    _verticalLayoutGroup.enabled = true;
                }
                _timeRemainingWhenScrollbarShouldBeForcedToZero = 0.5f;
            };

            if (_theme == null)
            {
                _theme = "DefaultDark";
            }
            _onSetTheme += ApplyTheme;
            _onClear += RemoveEntries;
        }

        private void Update()
        {
            while (_timeRemainingWhenScrollbarShouldBeForcedToZero > 0)
            {
                _scrollbar.value = 0.0f;
                _timeRemainingWhenScrollbarShouldBeForcedToZero -= Time.deltaTime;
            }
        }

        private void ApplyTheme()
        {
            Color ColorFromTydTable(TydTable table)
            {
                return new Color(
                    int.Parse(((TydString)table["r"]).Value) / 255.0f,
                    int.Parse(((TydString)table["g"]).Value) / 255.0f,
                    int.Parse(((TydString)table["b"]).Value) / 255.0f,
                    int.Parse(((TydString)table["a"]).Value) / 255.0f);
            }
            var key = "UI.ConsoleThemes." + _theme;
            var def = (TydTable)StreamingAssetsDatabase.GetDef(key);
            var background = (TydTable)def["Background"];
            var scrollView = (TydTable)def["ScrollView"];
            var commandInputField = (TydTable)def["CommandInputField"];
            var placeholder = (TydTable)def["Placeholder"];
            var text = (TydTable)def["Text"];
            var scrollbarBackground = (TydTable)def["ScrollbarBackground"];
            var scrollbarHandle = (TydTable)def["ScrollbarHandle"];

            _background.color = ColorFromTydTable(background);
            _scrollView.color = ColorFromTydTable(scrollView);
            _commandInputField.color = ColorFromTydTable(commandInputField);
            _placeholder.color = ColorFromTydTable(placeholder);
            _text.color = ColorFromTydTable(text);
            _scrollbarBackground.color = ColorFromTydTable(scrollbarBackground);
            _scrollbarHandle.color = ColorFromTydTable(scrollbarHandle);
        }

        public static void SetTheme(string key)
        {
            _theme = key;
            _onSetTheme?.Invoke();
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