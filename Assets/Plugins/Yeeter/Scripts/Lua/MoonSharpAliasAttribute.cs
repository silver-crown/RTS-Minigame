using System;

namespace Yeeter
{
    /// <summary>
    /// Marks a class with a MoonSharp.Interpreter.MoonSharpUserDataAttribute as going by another name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MoonSharpAliasAttribute : Attribute
    {
        /// <summary>
        /// The aliases used for the class in Lua.
        /// </summary>
        public string[] Aliases { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias">The alias to use.</param>
        public MoonSharpAliasAttribute(string alias)
        {
            Aliases = new string[] { alias };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="aliases">The aliases to use.</param>
        public MoonSharpAliasAttribute(string[] aliases)
        {
            Aliases = aliases;
        }
    }
}