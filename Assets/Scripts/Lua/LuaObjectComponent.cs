using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Lua
{
    /// <summary>
    /// A component which runs Unity methods from Lua.
    /// </summary>
    public class LuaObjectComponent : MonoBehaviour
    {
        public static Action<LuaObjectComponent> OnCreate;
        public static Action<LuaObjectComponent> OnClick;

        public LuaObject LuaObject { get; set; }

        // The script's Unity methods. Added as needed. Add them yourself or @banan.
        private DynValue _start;
        private DynValue _update;

        public void Load(string path)
        {
            LuaObject = LuaManager.CreateLuaObject(path);
            _start = LuaObject.Script.Globals.Get("Start");
            _update = LuaObject.Script.Globals.Get("Update");
        }

        private void Awake()
        {
            var _mouseClickRaycastTarget = GetComponent<MouseClickRaycastTarget>();
            if (_mouseClickRaycastTarget == null)
            {
                _mouseClickRaycastTarget = gameObject.AddComponent<MouseClickRaycastTarget>();
            }
            _mouseClickRaycastTarget.OnClick += () =>
            {
                OnClick?.Invoke(this);
            };
            OnCreate?.Invoke(this);
        }

        private void Start()
        {
            if (_start == null)
            {
                Debug.LogError("LuaObjectComponent instantiated without calling Load()");
            }
            if (_start.IsNotNil())
            {
                LuaObject.Script.Call(_start, LuaObject.Id);
            }
        }

        private void Update()
        {
            if (_update.IsNotNil())
            {
                LuaObject.Script.Call(_update, LuaObject.Id);
            }
        }

        /// <summary>
        /// Gets a value from the Actor's table.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the key.</returns>
        public DynValue Get(string key)
        {
            return LuaObject.Table.Get(key);
        }

        /// <summary>
        /// Returns every pair in the actor's table.
        /// </summary>
        /// <returns>The pairs.</returns>
        public IEnumerable<TablePair> GetTablePairs()
        {
            return LuaObject.Table.Pairs;
        }
    }
}