using Commands;

namespace Bbbt.Commands
{
    /// <summary>
    /// An empty command used to mark the last reset in the command history browser.
    /// </summary>
    public class LastResetCommand : Command
    {
        public override void Do()
        {
        }

        public override void Undo()
        {
        }
    }
}