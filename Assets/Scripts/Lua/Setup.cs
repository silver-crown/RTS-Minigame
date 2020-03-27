using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeeter;

namespace RTS.Lua
{
    /// <summary>
    /// Sets up the game so it works.
    /// </summary>
    public class Setup : MonoBehaviour
    {
        void Awake()
        {
            Yeeter.UI.Create("BBInputDebugger");
            StreamingAssetsDatabase.LoadDefsFromDirectory(Application.streamingAssetsPath + "/Defs");
            BBInput.LoadProfiles();
            BBInput.Initialize();
            StreamingAssetsDatabase.LoadScriptsFromDirectory(Application.streamingAssetsPath + "/Lua");
            LuaManager.ActiveLuaManager = new RTSLuaManager();
        }
    }
}