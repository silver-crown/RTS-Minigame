using MoonSharp.Interpreter;
using UnityEngine;

namespace RTS.Lua
{
    /// <summary>
    /// A component which runs Unity methods from Lua.
    /// </summary>
    public class LuaObjectComponent : MonoBehaviour
    {
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

        private void Start()
        {
            if (_start == null)
            {
                Debug.LogError("LuaObjectComponent instantiated without calling Load() on " + gameObject.name);
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
            } else
            {
                Debug.LogError("LuaObjectComponent is null on " + gameObject.name);
            }
        }

    }
}