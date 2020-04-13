using UnityEngine;
using Newtonsoft.Json;
using Yeeter;
using MoonSharp.Interpreter;
using System;

namespace Bbbt
{
    /// <summary>
    /// Loads a behaviour from Lua.
    /// </summary>
    [CreateAssetMenu(
        fileName = "Lua Behaviour",
        menuName = "bbBT/Behaviour/Leaf/Lua Behaviour",
        order = 0)]
    public class BbbtLuaBehaviour : BbbtLeafBehaviour
    {
        [Tooltip("The path to the Lua file. The Tick function of this file will be run. " +
        "Relative to StreamingAssets/Lua/Behaviours.")]
        [SerializeField, JsonProperty] private string _behaviour = null;

        private bool _isLoaded = false;
        private Script _script;
        private Table _table;
        private Closure _tick;

        public override string SaveDataType { get; } = "BbbtLuaBehaviour";

        private void OnLoad()
        {
            _script = LuaManager.GlobalScript;
            try
            {
                InGameDebug.Log(name + ": Loading behaviour from Lua: " +_behaviour);
                var table = _script.DoFile("Behaviours." +_behaviour
                ).Table;
                _tick = table.Get("Tick").Function;
                InGameDebug.Log(_tick);
            }
            catch (ScriptRuntimeException e)
            {
                InGameDebug.Log("<color=red>Oh no.</color> " + e.DecoratedMessage);
            }
            _isLoaded = true;
        }

        protected override void OnInitialize(GameObject gameObject)
        {
            if (!_isLoaded) OnLoad();
            _table = gameObject.GetComponent<LuaObjectComponent>().Table;
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
            
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            var result = _script.Call(_tick, ObjectBuilder.GetId(gameObject)).String;
            return (BbbtBehaviourStatus)Enum.Parse(typeof(BbbtBehaviourStatus), result);
        }
    }
}