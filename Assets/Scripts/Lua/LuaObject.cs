using MoonSharp.Interpreter;
using UnityEngine;

namespace RTS.Lua
{
    /// <summary>
    /// Contains stuff that facilitates communication between Lua and C#.
    /// </summary>
    public class LuaObject
    {
        /// <summary>
        /// The LuaObject's unique id.
        /// </summary>
        public uint Id { get; protected set; }
        /// <summary>
        /// The LuaObject's script.
        /// </summary>
        public Script Script { get; protected set; }
        /// <summary>
        /// The LuaObject's table.
        /// </summary>
        public Table Table { get; protected set; }


        /// <summary>
        /// Constructs a new LuaObject from a path.
        /// </summary>
        /// <param name="id">The LuaObject's id.</param>
        /// <param name="path">
        /// The path, from StreamingAssets/Lua, to the lua script.
        /// Do not include StreamingAssets/Lua in the path.
        /// For example, to load the script in "StreamingAssets/Lua/Example/Script.lua", use
        /// "Example/Script" or "Example/Script.lua".
        /// The script must return a table.
        /// </param>
        public LuaObject(uint id, string path)
        {
            Id = id;
            Script = LuaManager.CreateScript();
            Table = Script.DoFile(path).Table;
            Table.Set("_id", DynValue.NewNumber(id));
        }
    }
}