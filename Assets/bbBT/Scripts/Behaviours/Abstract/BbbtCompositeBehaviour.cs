using System.Collections.Generic;

namespace Bbbt
{
    /// <summary>
    /// A behaviour which can have multiple children.
    /// </summary>
    public abstract class BbbtCompositeBehaviour : BbbtBehaviour
    {
        /// <summary>
        /// The child behaviours of this node.
        /// </summary>
        protected List<BbbtBehaviour> _children = null;


        /// <summary>
        /// Adds a child behaviour.
        /// </summary>
        /// <param name="child">The child to add.</param>
        public void AddChild(BbbtBehaviour child)
        {
            _children.Add(child);
        }
    }
}