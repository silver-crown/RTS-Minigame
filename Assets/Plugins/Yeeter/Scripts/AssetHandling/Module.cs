namespace Yeeter
{
    /// <summary>
    /// Contains module data.
    /// </summary>
    public class Module
    {
        public string Id { get; set; } = "Package.Id";
        public string Name { get; set; } = "Name";
        public string Author { get; set; } = "Author";
        public string Path { get; set; }
        public string Description { get; set; } = "Description";
        public string Version { get; set; } = "1.0";
        public string[] SupportedVersions { get; set; }
        public string[] CompatibleWith { get; set; }
        public string[] IncompatibleWith { get; set; }

        public Module()
        {
        }
    }
}