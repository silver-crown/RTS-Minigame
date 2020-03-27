using MoonSharp.Interpreter;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting;

namespace Yeeter
{ 
    [Preserve, MoonSharpUserData, MoonSharpAlias("Console")]
    public class InGameDebug
    {
        public static bool LogObjectCreated { get; set; } = true;

        public static void Log(object message, object context = null)
        {
            var unityObject = context as UnityEngine.Object;
            if (unityObject != null)
            {
                Debug.Log(message, unityObject);
            }
            else
            {
                Debug.Log(message);
            }
            DebugConsole.StaticAddEntry(message, context);
        }

        public static void SetTheme(string key)
        {
            DebugConsole.SetTheme(key);
        }

        public static void Help()
        {
            var path = Path.Combine(Application.streamingAssetsPath, "Mods", "Help.txt");
            if (File.Exists(path))
            {
                Log(File.ReadAllText(path));
            }
            else
            {
                Log("<color=red>" + path + " not found.</color>");
            }
        }

        public static void Clear()
        {
            DebugConsole.Clear();
        }
    }
}