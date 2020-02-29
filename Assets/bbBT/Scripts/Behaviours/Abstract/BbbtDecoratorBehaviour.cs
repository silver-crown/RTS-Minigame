using Newtonsoft.Json;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// A decorator is a behaviour which has a single child.
    /// </summary>
    public abstract class BbbtDecoratorBehaviour : BbbtBehaviour
    {
        /// <summary>
        /// The decorator's child behaviour.
        /// </summary>
        [JsonProperty] public BbbtBehaviour Child { get; protected set; } = null;


        /// <summary>
        /// Adds a child to the node.
        /// </summary>
        /// <param name="child">The child to add.</param>
        public override void AddChild(BbbtBehaviour child)
        {
            Child = child;
        }

        /// <summary>
        /// Removes all of the behaviour's children.
        /// </summary>
        public override void RemoveChildren()
        {
            Child = null;
        }
    }
}