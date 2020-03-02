namespace Commands
{
    /// <summary>
    /// A container for a method that can be done or undone at will.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Does the command.
        /// </summary>
        public abstract void Do();

        /// <summary>
        /// Undoes the command.
        /// </summary>
        public abstract void Undo();
    }
}