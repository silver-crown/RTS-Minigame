using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace Yeeter
{
    /// <summary>
    /// Handles Lua stuff.
    /// </summary>
    [Preserve, MoonSharpUserData]
    public class LuaManager
    {
        public static Dictionary<int, Dictionary<string, Action<DynValue>>> _onValueSet { get; set; }
            = new Dictionary<int, Dictionary<string, Action<DynValue>>>();
        protected static Dictionary<int, LuaObjectComponent> _objects = new Dictionary<int, LuaObjectComponent>();

        /// <summary>
        /// The active LuaManager is the lua manager that we use to execute scripts and build lua objects.
        /// </summary>
        public static LuaManager ActiveLuaManager { get; set; } = new LuaManager();
        public static Action<LuaObjectComponent> OnLuaObjectSetUp { get; set; }

        public static Script GlobalScript
        {
            get
            {
                if (ActiveLuaManager._globalScript == null)
                {
                    ActiveLuaManager._globalScript = ActiveLuaManager.CreateScript();
                }
                return ActiveLuaManager._globalScript;
            }
        }
        public static bool LoadUpdateEachFrame
        {
            get => ActiveLuaManager._loadUpdateEachFrame;
            set => ActiveLuaManager._loadUpdateEachFrame = value;
        }

        /// <summary>
        /// Used so that stuff can be shared essentially.
        /// </summary>
        protected Script _globalScript;
        protected bool _loadUpdateEachFrame = false;

        public static void ReloadScripts()
        {
            InGameDebug.Log("OH EYAH YEAH ");
            foreach (var luaObject in _objects.Values)
            {
                luaObject.Load(luaObject.Path);
            }
        }

        /// <summary>
        /// Sets up a LuaObjectComponent.
        /// </summary>
        /// <param name="path">T</param>
        [MoonSharpHidden]
        public static void SetupLuaObject(LuaObjectComponent luaObject)
        {
            luaObject.Id = ObjectBuilder.GetId(luaObject.gameObject);
            _objects.Add(luaObject.Id, luaObject);
            luaObject.Script = GlobalScript;
            OnLuaObjectSetUp?.Invoke(luaObject);
        }

        /// <summary>
        /// Adds an action to be performed when a value is a associated with a given key for a
        /// LuaObject with a given id.
        /// </summary>
        /// <param name="id">The LuaObject id.</param>
        /// <param name="key">The key to listen to.</param>
        /// <param name="action">The action to perform when the value is set.</param>
        public static void AddOnValueSetListener(int id, string key, Action<DynValue> action)
        {
            if (!_onValueSet.ContainsKey(id))
            {
                _onValueSet.Add(id, new Dictionary<string, Action<DynValue>>());
            }
            if (!_onValueSet[id].ContainsKey(key))
            {
                _onValueSet[id].Add(key, action);
            }
            else
            {
                _onValueSet[id][key] += action;
            }
        }
        /// <summary>
        /// Adds an action to be performed when a value is a associated with a given key for a
        /// LuaObject with a given id.
        /// </summary>
        /// <param name="id">The LuaObject id.</param>
        /// <param name="key">The key to listen to.</param>
        /// <param name="action">The action to perform when the value is set.</param>
        public static void AddOnValueSetListener(int id, string key, DynValue action)
        {
            if (!_onValueSet.ContainsKey(id))
            {
                _onValueSet.Add(id, new Dictionary<string, Action<DynValue>>());
            }
            if (!_onValueSet[id].ContainsKey(key))
            {
                _onValueSet[id].Add(key, v => Call(action, v));
            }
            else
            {
                _onValueSet[id][key] += v => Call(action, v);
            }
        }

        /// <summary>
        /// Sets a table value in a LuaObject.
        /// </summary>
        /// <param name="id">The object's id.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value to associate with the key.</param>
        public static void Set(int id, string key, DynValue value)
        {
            if (!_objects.ContainsKey(id))
            {
                Debug.LogError(
                    "LuaManager.Set(): " +
                    "LuaObject with id " + id + "was not found in LuaManager. " +
                    "Either the id was invalid or the LuaObject was created with LuaManager.CreateLuaObject().");
                return;
            }
            _objects[id].Table[key] = value;
        }

        /// <summary>
        /// Gets a table value in a LuaObject.
        /// </summary>
        /// <param name="id">The object's id.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the key.</returns>
        public static DynValue Get(int id, string key)
        {
            if (!_objects.ContainsKey(id))
            {
                Debug.LogError(
                    "LuaManager.Get(): " +
                    "LuaObject with id " + id + " was not found in LuaManager. " +
                    "Either the id was invalid or the LuaObject was created with LuaManager.CreateLuaObject().");
                return DynValue.Nil;
            }
            var value = _objects[id].Table.Get(key);
            if (value.IsNil())
            {
                Debug.LogError(
                    "LuaManager.Get(): " +
                    "LuaObject with id " + id + ": " + key + " was not found in table.");
                return DynValue.Nil;
            }

            return value;
        }

        /// <summary>
        /// Gets the LuaObject id of a GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject's id.</param>
        /// <returns>The LuaObject id.</returns>
        public static DynValue GetLuaObjectId(int id)
        {
            var luaObject = _objects[id];
            return DynValue.NewNumber(ObjectBuilder.Get(id).GetComponent<LuaObjectComponent>().Id);
        }

        /// <summary>
        /// Loads and executes a string.
        /// </summary>
        /// <param name="code">The code to run.</param>
        /// <returns>The return value from the executed code.</returns>
        public static DynValue DoString(string code)
        {
            DynValue result = null;
            try
            {
                return GlobalScript.DoString(code);
            }
            catch (ScriptRuntimeException e)
            {
                InGameDebug.Log("<color=red>Oh no.</color> " + e.DecoratedMessage);
            }
            return result;
        }
        /// <summary>
        /// Executes a function.
        /// </summary>
        /// <param name="function">The function to run.</param>
        /// <returns>The return value from the executed function.</returns>
        public static DynValue Call(DynValue function)
        {
            DynValue result = null;
            try
            {
                return GlobalScript.Call(function);
            }
            catch (ScriptRuntimeException e)
            {
                InGameDebug.Log("<color=red>Oh no.</color> " + e.DecoratedMessage);
            }
            return result;
        }
        /// <summary>
        /// Executes a function.
        /// </summary>
        /// <param name="function">The function to run.</param>
        /// <param name="args">The arguments to pass to the function.</param>
        /// <returns>The return value from the executed function.</returns>
        public static DynValue Call(DynValue function, params DynValue[] args)
        {
            DynValue result = null;
            try
            {
                result =  GlobalScript.Call(function, args);
            }
            catch (ScriptRuntimeException e)
            {
                InGameDebug.Log("<color=red>Oh no.</color> " + e.DecoratedMessage);
            }
            return result;
        }

        /// <summary>
        /// Creates a script set up with the globals we need.
        /// </summary>
        /// <returns>The created script.</returns>
        public virtual Script CreateScript()
        {
            UserData.RegisterAssembly();
            UserData.RegisterType<SceneManager>();
            UserData.RegisterType<Application>();
            UserData.RegisterType<GameObject>();
            UserData.RegisterType<Transform>();
            UserData.RegisterType<Vector2>();
            UserData.RegisterType<Vector3>();
            UserData.RegisterType<InputField>();
            var script = new Script();
            script.Globals["LuaManager"] = new LuaManager();
            script.Globals["InGameDebug"] = new InGameDebug();
            script.Globals["Console"] = new InGameDebug();
            script.Globals["ObjectBuilder"] = new ObjectBuilder();
            script.Globals["UI"] = new UI();
            script.Globals["Input"] = new LuaInput();
            script.Globals["SceneManager"] = new SceneManager();
            script.Globals["Application"] = new Application();
            script.Globals["SoundManager"] = new SoundManager();
            script.Globals["Assets"] = new StreamingAssetsDatabase();
            script.Globals["StreamingAssetsDatabase"] = script.Globals["Assets"];
            script.Globals["Vector3"] = new Vector3();
            script.Globals["GameObject"] = new GameObject();
            script.Globals["BBInput"] = new BBInput();
            return script;
        }
    }
}