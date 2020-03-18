using MoonSharp.Interpreter;
using RTS.Lua;
using UnityEngine;

namespace RTS
{
    [MoonSharpUserData]
    public class DroneStaticMethods
    {
        private static GameObject _prefab;

        public static DynValue Create(string type, float x = 0, float y = 0, float z = 0)
        {
            if (_prefab == null) _prefab = Resources.Load<GameObject>("Prefabs/Drone");
            var position = new Vector3(x, y, z);
            var drone = Object.Instantiate(_prefab, position, Quaternion.identity, null).GetComponent<Drone>();
            drone.SetType(type);
            return drone.GetValue("_id");
        }

        public static Table Create(int count, string type, float x = 0, float y = 0, float z = 0)
        {
            var table = new Table(LuaManager.CreateScript());
            for (int i = 0; i < count; i++)
            {
                table.Set(i, Create(type, x, y, z));
            }
            return table;
        }
    }
}