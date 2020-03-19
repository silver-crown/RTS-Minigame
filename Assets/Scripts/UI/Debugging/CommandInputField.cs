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
            string code = _text.text;
            _text.text = "";
            _field.text = "";
            if (EventSystem.current.currentSelectedGameObject != gameObject) return;
            if (code == null || code == "") return;
            LuaManager.DoString("InGameDebug.Log(" + code + ")");
            EventSystem.current.SetSelectedGameObject(gameObject);
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