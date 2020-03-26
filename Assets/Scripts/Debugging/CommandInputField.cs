using RTS.Lua;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Yeeter;

namespace RTS.UI.Debugging
{
    [RequireComponent(typeof(InputField))]
    public class CommandInputField : MonoBehaviour, IPointerDownHandler
    {
        private InputField _field;
        private Text _text;
        private Stack<string> _commandsAbove = new Stack<string>();
        private Stack<string> _commandsBelow = new Stack<string>();

        private void Awake()
        {
            _field = GetComponent<InputField>();
            _text = _field.textComponent;

            // Input
            _field.onEndEdit.AddListener(OnCommandEntered);
            BBInput.AddOnAxisPressed("DebugConsoleUp", GoUp);
            BBInput.AddOnAxisPressed("DebugConsoleDown", GoDown);
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

            // Check if this is a custom command. Only help for now.
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

        public void OnPointerDown(PointerEventData eventData)
        {
            BBInput.SetActiveProfile("DebugConsole");
        }
    }
}