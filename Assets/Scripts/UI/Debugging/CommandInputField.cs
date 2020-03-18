using RTS.Lua;
using System.Collections;
using System.Collections.Generic;
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

        private void Awake()
        {
            _field = GetComponent<InputField>();
            _text = _field.textComponent;
            BBInput.AddOnKeyDown(KeyCode.Return, OnCommandEntered, -1);
            BBInput.OnEatenKeysReset += StealBBInputFocus;
        }

        private void OnCommandEntered()
        {
            if (EventSystem.current.currentSelectedGameObject != gameObject) return;
            if (_text.text == null || _text.text == "") return;
            LuaManager.DoString("InGameDebug.Log(" + _text.text + ")");
            _text.text = "";
        }

        private void StealBBInputFocus()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                BBInput.EatAll();
                BBInput.UnEat(KeyCode.Return);
            }
        }
    }
}