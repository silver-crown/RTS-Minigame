using MoonSharp.Interpreter;
using RTS.Lua;
using UnityEngine;
using Yeeter;

namespace RTS
{
    [MoonSharpUserData]
    public class DroneStaticMethods
    {
        private static GameObject _prefab;

        public static DynValue Create(string type, float x = 0, float y = 0, float z = 0)
        {
            var position = new Vector3(x, y, z);
            int id = (int)ObjectBuilder.Instantiate("Drone").Number;
            var go = ObjectBuilder.Get(id);
            go.GetComponent<Drone>().Initialize(type, id);
            go.transform.position = position;
            return DynValue.NewNumber(id);
        }

        public static Table Create(int count, string type, float x = 0, float y = 0, float z = 0)
        {
            var table = new Table(LuaManager.GlobalScript);
            for (int i = 0; i < count; i++)
            {
                table.Set(i, Create(type, x, y, z));
            }
            return table;
        }
    }
}