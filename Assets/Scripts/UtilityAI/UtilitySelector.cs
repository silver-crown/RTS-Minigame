using MoonSharp.Interpreter;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace UtilityAI
{
    /// <summary>
    /// A helper class used for selecting desirable UtilityActions.
    /// </summary>
    [Preserve, MoonSharpUserData]
    public class UtilitySelector
    {
        /// <summary>
        /// The candidate actions.
        /// </summary>
        private List<UtilityAction> _actions = new List<UtilityAction>();

        /// <summary>
        /// Adds an action to the list of potential actions.
        /// </summary>
        /// <param name="action">The action to add.</param>
        public void AddAction(UtilityAction action)
        {
            _actions.Add(action);
        }
        /// <summary>
        /// Gets the UtilityAction with the highest utility.
        /// </summary>
        /// <returns>The UtilityAction with the highest utility.</returns>
        public UtilityAction GetBestAction()
        {
            return _actions.OrderByDescending(action => action.GetUtility()).FirstOrDefault();
        }
        /// <summary>
        /// Gets the UtilityAction with the lowest utility.
        /// </summary>
        /// <returns>The UtilityAction with the lowest utility.</returns>
        public UtilityAction GetWorstAction()
        {
            return _actions.OrderBy(action => action.GetUtility()).FirstOrDefault();
        }
        /// <summary>
        /// Gets a random UtilityAction.
        /// </summary>
        /// <returns>A random UtilityAction.</returns>
        public UtilityAction GetRandomAction()
        {
            return _actions[Random.Range(0, _actions.Count)];
        }
        /// <summary>
        /// Gets all the actions that can be selected ordered from highest to lowest utility.
        /// </summary>
        /// <returns>The list of actions ordered from highest to lowest utility.</returns>
        public List<UtilityAction> GetSortedActions()
        {
            return _actions.OrderByDescending(action => action.GetUtility()).ToList();
        }
        /// <summary>
        /// Invokes the UtilityAction with the highest utility.
        /// </summary>
        public void InvokeBestAction()
        {
            GetBestAction().Invoke();
        }
        /// <summary>
        /// 
        /// Invokes the UtilityAction with the lowest utility.
        /// </summary>
        public void InvokeWorstAction()
        {
            GetBestAction().Invoke();
        }
        /// <summary>
        /// Invokes a random UtilityAction.
        /// </summary>
        public void InvokeRandomAction()
        {
            GetRandomAction().Invoke();
        }
        /// <summary>
        /// Creates a new UtilitySelector.
        /// </summary>
        /// <returns>The newly created UtilitySelector.</returns>
        public static UtilitySelector Create()
        {
            return new UtilitySelector();
        }
    }
}