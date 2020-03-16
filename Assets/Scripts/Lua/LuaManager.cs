using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Lua
{
    /// <summary>
    /// Handles Lua stuff.
    /// </summary>
    [MoonSharpUserData]
    public class LuaManager
    {
        private static uint _nextId = 0;
        private static Dictionary<uint, LuaObject> _objects = new Dictionary<uint, LuaObject>();

        /// <summary>
        /// Constructs a new LuaObject from a path.
        /// </summary>
        /// <param name="path">
        /// The path, from StreamingAssets/Lua, to the lua script.
        /// Do not include StreamingAssets/Lua in the path.
        /// For example, to load the script in "StreamingAssets/Lua/Example/Script.lua", use
        /// "Example/Script" or "Example/Script.lua".
        /// The script must return a table.
        /// </param>
        public static LuaObject CreateLuaObject(string path)
        {
            var luaObject = new LuaObject(_nextId, path);
            _objects.Add(_nextId, luaObject);
            _nextId++;
            return luaObject;
        }

        /// <summary>
        /// Sets a table value in a LuaObject.
        /// </summary>
        /// <param name="id">The object's id.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to associate with the key.</param>
        public static void Set(uint id, string key, DynValue value)
        {
            if (!_objects.ContainsKey(id))
            {
                Debug.LogError(
                    "LuaManager.Set(): " +
                    "LuaObject with id " + id +  "was not found in LuaManager. " +
                    "Either the id was invalid or the LuaObject was created with LuaManager.CreateLuaObject().");
                return;
            }
            _objects[id].Table.Set(key, value);
        }

        /// <summary>
        /// Gets a table value in a LuaObject.
        /// </summary>
        /// <param name="id">The object's id.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the key.</returns>
        public static DynValue Get(uint id, string key)
        {
            if (!_objects.ContainsKey(id))
            {
                Debug.LogError(
                    "LuaManager.Get(): " +
                    "LuaObject with id " + id + " was not found in LuaManager. " +
                    "Either the id was invalid or the LuaObject was created with LuaManager.CreateLuaObject().");
                return DynValue.Nil;
            }
            var table = _objects[id].Table;
            var value = table.Get(key);
            if (value.IsNil())
            {
                Debug.LogError(
                    "LuaManager.Get(): " +
                    "LuaObject with id " + id + ": " + key + " was not found in table.");
                return DynValue.Nil;
            }
            
            return value;
        }
    }
}