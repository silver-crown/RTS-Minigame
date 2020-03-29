using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yeeter
{
    /// <summary>
    /// A component which runs Unity methods from Lua.
    /// </summary>
    public class LuaObjectComponent : MonoBehaviour
    {
        public static Action<LuaObjectComponent> OnCreate;
        public static Action<LuaObjectComponent> OnClick;

        private Closure _start;
        private Closure _update;

        public int Id { get; set; }
        public string Path { get; private set; }
        public Script Script { get; set; }
        public Table Table { get; protected set; }

        public void Load(string path)
        {
            ObjectBuilder.AddExternalGameObject(gameObject);
            LuaManager.SetupLuaObject(this);
            Script = LuaManager.GlobalScript;
            Table = Script.DoFile(path).Table;
            _start = Table.Get("Start").Function;
            _update = Table.Get("Update").Function;
            Path = path;
        }

        private void Start()
        {
            if (_start == null)
            {
                Debug.LogError(name + ": Start is null. ", this);
            }
            if (_update == null)
            {
                Debug.LogError(name + ": Update is null.", this);
            }
            if (_start != null)
            {
                try
                {
                    Script.Call(_start, Table, Id);
                }
                catch (ScriptRuntimeException e)
                {
                    InGameDebug.Log("<color=red>Oh no.</color> " + e.DecoratedMessage);
                }
            }
        }

        private void Update()
        {
            if (_update != null)
            {
                try
                {
                    if (LuaManager.LoadUpdateEachFrame)
                    {
                        var _table = Script.DoFile(Path).Table;
                        _update = _table.Get("Update").Function;
                        Script.Call(_update, _table, Id, Time.deltaTime);
                    }
                    else
                    {
                        Script.Call(_update, Table, Id, Time.deltaTime);
                    }
                }
                catch (ScriptRuntimeException e)
                {
                    InGameDebug.Log("<color=red>Oh no.</color> " + e.DecoratedMessage);
                }
            }
        }

        /// <summary>
        /// Gets a value from the Actor's table.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the key.</returns>
        public DynValue Get(string key)
        {
            return Table.Get(key);
        }

        /// <summary>
        /// Returns every pair in the actor's table.
        /// </summary>
        /// <returns>The pairs in the table.</returns>
        public IEnumerable<TablePair> GetTablePairs()
        {
            return Table.Pairs;
        }
    }
}