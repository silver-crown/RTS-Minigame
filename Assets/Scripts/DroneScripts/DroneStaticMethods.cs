using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;
using Yeeter;

using Mirror;

namespace RTS
{
    /// <summary>
    /// Contains static methods for dealing with drones. This exists to provide an interface for creating drones in Lua.
    /// </summary>
    [Preserve, MoonSharpUserData]
    public class DroneStaticMethods
    {
        /// <summary>
        /// Creates a drone of type 'type' at the specified location and returns its DynValue.
        /// </summary>
        /// <param name="type">The drone type to be spawned.</param>
        /// <param name="x">The x spawn position of the drone.</param>
        /// <param name="y">The y spawn position of the drone.</param>
        /// <param name="z">The z spawn position of the drone.</param>
        /// <returns>The drone's id.</returns>
        public static DynValue Create(string type, float x = 0, float y = 0, float z = 0)
        {
            var position = new Vector3(x, y, z);
            int id = (int)ObjectBuilder.Instantiate("Drone").Number;
            var go = ObjectBuilder.Get(id);
            go.GetComponent<Drone>().Initialize(type, id);
            go.transform.position = position;
            NetworkServer.Spawn(go);
            return DynValue.NewNumber(id);
        }

        /// <summary>
        /// Creates several drones.
        /// </summary>
        /// <param name="count">The amound of drones to create.</param>
        /// <param name="type">The type of the drones.</param>
        /// <param name="x">The x position of the drone.</param>
        /// <param name="y">The y position of the drone.</param>
        /// <param name="z">The z position of the drone.</param>
        /// <returns>A table containing the created drones.</returns>
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