namespace Bbbt
{
    /// <summary>
    /// A leaf behaviour has no children and is responsible for the behaviour tree actually doing something.
    /// Examples of a lead node could be capable of are shooting enemies, gathering resources, or even calling
    /// another behaviour tree.
    /// </summary>
    public abstract class BbbtLeafBehaviour : BbbtBehaviour
    {
    }
}