using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;
using Yeeter;

namespace UtilityAI
{
    /// <summary>
    /// An action whose utility can be influenced by the game state.
    /// </summary>
    [Preserve, MoonSharpUserData]
    public class UtilityAction
    {
        /// <summary>
        /// The action to perform when the UtilityAction is invoked.
        /// </summary>
        private Action _action;
        /// <summary>
        /// The factors that influence the utility of the action.
        /// </summary>
        private List<Func<float>> _factors = new List<Func<float>>();

        /// <summary>
        /// An empty constructor. This exists solely so that we can expose static methods to Lua.
        /// </summary>
        public UtilityAction()
        {
        }
        /// <summary>
        /// Constructs a new UtilityAction.
        /// </summary>
        /// <param name="action">The action to be invoked when the UtilityAction is selected.</param>
        public UtilityAction(Action action)
        {
            _action = action;
        }
        /// <summary>
        /// Constructs a new UtilityAction with a Lua funciton.
        /// </summary>
        /// <param name="action">The Lua function to be called when the UtilityAction is selected.</param>
        public UtilityAction(DynValue action)
        {
            _action = () => LuaManager.Call(action);
        }
        /// <summary>
        /// Invokes the UtilityAction.
        /// </summary>
        public void Invoke()
        {
            _action.Invoke();
        }
        /// <summary>
        /// Adds a factor to the UtilityAction.
        /// </summary>
        /// <param name="factor">
        /// The factor to be added.
        /// This should be a logistic function with an output range of 0..1.
        /// </param>
        public void AddFactor(Func<float> factor)
        {
            _factors.Add(factor);
        }
        /// <summary>
        /// Adds a factor to the UtilityAction.
        /// </summary>
        /// <param name="factor">
        /// The factor to be added.
        /// This should be a logistic function with an output range of 0..1.
        /// </param>
        public void AddFactor(DynValue factor)
        {
            _factors.Add(() => (float)LuaManager.Call(factor).Number);
        }
        /// <summary>
        /// Gets the utility of the action be averaging the result of the utility factor functions.
        /// </summary>
        /// <returns>The action's utility.</returns>
        public float GetUtility()
        {
            return _factors.Sum(factor => factor.Invoke()) / _factors.Count;
        }
        /// <summary>
        /// Creates a new UtilityAction.
        /// </summary>
        /// <param name="action">The action to be invoked when the UtilityAction is selected.</param>
        /// <returns>The newly created UtilityAction.</returns>
        public static UtilityAction Create(Action action)
        {
            return new UtilityAction(action);
        }
        /// <summary>
        /// Creates a new UtilityAction.
        /// </summary>
        /// <param name="action">The Lua function to be called when the UtilityAction is selected.</param>
        /// <returns>The newly created UtilityAction.</returns>
        public static UtilityAction Create(DynValue action)
        {
            return new UtilityAction(action);
        }
    }
}