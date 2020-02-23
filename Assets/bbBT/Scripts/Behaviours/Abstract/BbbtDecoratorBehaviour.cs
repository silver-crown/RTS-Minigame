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
        public BbbtBehaviour Child { get; set; }
    }
}