using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yeeter;

namespace RTS.Lua
{
    /// <summary>
    /// Handles Lua stuff.
    /// </summary>
    [MoonSharpUserData]
    public class RTSLuaManager : LuaManager
    {
        /// <summary>
        /// Creates a MoonSharp.Interpreter.Script set up to accept our awesome scripting API.
        /// </summary>
        /// <returns>The created script.</returns>
        public override Script CreateScript()
        {
            _loadUpdateEachFrame = false;
            BBInput.AddOnAxisPressed("ToggleScriptUpdateEachFrame", () =>
            {
                LoadUpdateEachFrame = !LoadUpdateEachFrame;
                InGameDebug.Log("LoadUpdateEachFrame = " + LoadUpdateEachFrame);
            });
            BBInput.AddOnAxisPressed("ReloadScripts", ReloadScripts);
            UserData.RegisterAssembly(typeof(LuaManager).Assembly);
            UserData.RegisterAssembly(typeof(RTSLuaManager).Assembly);
            UserData.RegisterType<SceneManager>();
            UserData.RegisterType<Application>();
            UserData.RegisterType<GameObject>();
            UserData.RegisterType<Transform>();
            UserData.RegisterType<Vector2>();
            UserData.RegisterType<Vector3>();
            UserData.RegisterType<InputField>();
            var script = new Script();
            //script.Options.ScriptLoader = new StreamingAssetsScriptLoader();
            script.Globals["LuaManager"] = new LuaManager();
            script.Globals["InGameDebug"] = new InGameDebug();
            script.Globals["Console"] = new InGameDebug();
            script.Globals["ObjectBuilder"] = new ObjectBuilder();
            script.Globals["UI"] = new Yeeter.UI();
            script.Globals["Input"] = new LuaInput();
            script.Globals["SceneManager"] = new SceneManager();
            script.Globals["Application"] = new Application();
            script.Globals["SoundManager"] = new SoundManager();
            script.Globals["Assets"] = new StreamingAssetsDatabase();
            script.Globals["StreamingAssetsDatabase"] = script.Globals["Assets"];
            script.Globals["Vector3"] = new Vector3();
            script.Globals["GameObject"] = new GameObject();
            return script;
        }
    }
}