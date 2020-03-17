using RTS.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.Debugging
{
    [RequireComponent(typeof(InputField))]
    public class CommandInputField : MonoBehaviour
    {
        private Text _text;
        private void Awake()
        {
            _text = GetComponent<InputField>().textComponent;
        }
        public void OnCommandEntered()
        {
            var value = LuaManager.DoString(_text.text);
            InGameDebug.Log(value);
            _text.text = "";
        }
    }
}