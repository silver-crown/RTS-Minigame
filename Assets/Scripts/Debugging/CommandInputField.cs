using RTS.Lua;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RTS.UI.Debugging
{
    [RequireComponent(typeof(InputField))]
    public class CommandInputField : MonoBehaviour
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
            BBInput.AddOnKeyDown(KeyCode.Return, OnCommandEntered, -1);
            BBInput.AddOnKeyDown(KeyCode.UpArrow, GoUp);
            BBInput.AddOnKeyDown(KeyCode.DownArrow, GoDown);
            BBInput.OnEatenKeysReset += StealBBInputFocus;
        }

        private void OnCommandEntered()
        {
            string code = _field.text;
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

        private void StealBBInputFocus()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                BBInput.EatAll();
                BBInput.UnEat(KeyCode.Return);
                BBInput.UnEat(KeyCode.UpArrow);
                BBInput.UnEat(KeyCode.DownArrow);
            }
        }
    }
}