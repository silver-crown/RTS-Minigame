namespace Bbbt
{
    /// <summary>
    /// A section of the BbbtHelpMenu.
    /// </summary>
    public class BbbtHelpSection
    {
        /// <summary>
        /// The section's header.
        /// </summary>
        public string Header { get; protected set; }

        /// <summary>
        /// The section's content.
        /// </summary>
        public string Content { get; protected set; }

        /// <summary>
        /// Whether the section's content should be displayed in the menu.
        /// </summary>
        public bool IsActive { get; set; } = false;


        /// <summary>
        /// Constructs a new instance of BbbtHelpSection.
        /// </summary>
        /// <param name="header">The section's header.</param>
        /// <param name="content">The section's content.</param>
        public BbbtHelpSection(string header, string content)
        {
            Header = header;
            Content = content;
        }
    }
}