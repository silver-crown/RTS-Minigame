using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Yeeter
{
    [RequireComponent(typeof(InputField))]
    public class DebugConsoleInputField : MonoBehaviour
    {
        private InputField _field;
        private Text _text;
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private Stack<string> _commandsAbove = new Stack<string>();
        private Stack<string> _commandsBelow = new Stack<string>();
        private bool _lastIsMouseInRect = false;

        /// <summary>
        /// Invoked when a command entered into the input field has been parsed and run.
        /// </summary>
        public Action<string> OnCommandDone { get; set; }
        /// <summary>
        /// Invoked when DebugConsoleInputField has loc BBInput.
        /// This is so that other classes can bypass the eaten keys if needed.
        /// </summary>
        public Action OnBBInputFocusStolen { get; set; }

        private void Awake()
        {
            _field = GetComponent<InputField>();
            _text = _field.textComponent;
            _rectTransform = GetComponent<RectTransform>();
            _canvas = FindObjectsOfType<Canvas>().Where(
                canvas => canvas.GetComponentsInChildren<DebugConsoleInputField>().Contains(this)
            ).First();

            BBInput.AddOnAxisPressed("EnterCommand", () => OnCommandEntered(_field.text));
        }

        private void OnDisable()
        {
            if (_lastIsMouseInRect) BBInput.ActivatePreviousProfile();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                bool isMouseInRect = false;
                if (_canvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    isMouseInRect = RectTransformUtility.RectangleContainsScreenPoint(
                        _rectTransform, Input.mousePosition, _canvas.worldCamera
                    );
                }
                else
                {
                    isMouseInRect = RectTransformUtility.RectangleContainsScreenPoint(
                        _rectTransform, Input.mousePosition
                    );
                }
                if (isMouseInRect && !_lastIsMouseInRect) BBInput.SetActiveProfile("Console");
                else if (_lastIsMouseInRect) BBInput.ActivatePreviousProfile();
                _lastIsMouseInRect = isMouseInRect;
            }
        }

        private void OnCommandEntered(string code)
        {
            _commandsAbove.Push(code);
            _text.text = "";
            _field.text = "";
            if (EventSystem.current.currentSelectedGameObject != gameObject) return;
            _field.OnSelect(new BaseEventData(EventSystem.current));
            if (code == null || code == "") return;

            InGameDebug.Log("> " + code);

            // Check if this is a custom command.
            string directCommand = code.ToLowerInvariant().Trim();
            if (directCommand == "help" || directCommand == "?")
            {
                InGameDebug.Help();
            }
            else if (directCommand == "clear")
            {
                InGameDebug.Clear();
            }
            else
            {
                // Lua script, let LuaManager handle this.
                LuaManager.DoString(code);
                _commandsBelow = new Stack<string>();
            }
            OnCommandDone?.Invoke(code);
        }

        private void GoUp()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject && _commandsAbove.Count > 0)
            {
                _field.text = _commandsAbove.Pop();
                _text.text = _field.text;
                _commandsBelow.Push(_field.text);
            }
        }

        private void GoDown()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject && _commandsBelow.Count > 0)
            {
                _field.text = _commandsBelow.Pop();
                _text.text = _field.text;
                _commandsAbove.Push(_field.text);
            }
        }
    }
}