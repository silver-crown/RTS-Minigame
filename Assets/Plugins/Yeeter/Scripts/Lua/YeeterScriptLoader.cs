#if DOTNET_CORE || (!(PCL || ENABLE_DOTNET || NETFX_CORE))
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MoonSharp.Interpreter.Loaders
{
	/// <summary>
	/// Script Loader for DungeonYeeter.
	/// </summary>
	public class YeeterScriptLoader : ScriptLoaderBase
	{
		public static Dictionary<string, string> AssetDatabaseToPath = new Dictionary<string, string>();

		/// <summary>
		/// Checks if a script file exists.
		/// </summary>
		/// <param name="name">The script filename.</param>
		/// <returns>True if the file exists, false otherwise.</returns>
		public override bool ScriptFileExists(string name)
		{
			return File.Exists(AssetDatabaseToPath[name]);
		}

		/// <summary>
		/// Opens a file for reading the script code.
		/// It can return either a string, a byte[] or a Stream.
		/// If a byte[] is returned, the content is assumed to be a serialized (dumped) bytecode. If it's a string, it's
		/// assumed to be either a script or the output of a string.dump call. If a Stream, autodetection takes place.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="globalContext">The global context.</param>
		/// <returns>
		/// A string, a byte[] or a Stream.
		/// </returns>
		public override object LoadFile(string key, Table globalContext)
		{
			if (AssetDatabaseToPath.ContainsKey(key))
			{
				string path = AssetDatabaseToPath[key];
				return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}
			else
			{
				Debug.LogError("YeeterScriptLoader: Script '" + key + "' not found.");
				return null;
			}
		}
	}
}
#endif