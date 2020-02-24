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
        public List<BbbtBehaviour> Children { get; protected set; } = null;


        /// <summary>
        /// Adds a child behaviour.
        /// </summary>
        /// <param name="child">The child to add.</param>
        public override void AddChild(BbbtBehaviour child)
        {
            if (Children == null)
            {
                Children = new List<BbbtBehaviour>();
            }
            Children.Add(child);
        }

        /// <summary>
        /// Removes all of the behaviour's children.
        /// </summary>
        public override void RemoveChildren()
        {
            Children = null;
        }
    }
}