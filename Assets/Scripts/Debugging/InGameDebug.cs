using MoonSharp.Interpreter;
using RTS.UI.Debugging;
using System.IO;
using UnityEngine;

/// <summary>
/// Used for displaying creating and logging debug messages in-game.
/// </summary>
[MoonSharpUserData]
public class InGameDebug
{
    public static void Log(object message, object context = null)
    {
        var unityObject = context as Object;
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

    public static void Help()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Lua", "Help.txt");
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