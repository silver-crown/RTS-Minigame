#if DOTNET_CORE || (!(PCL || ENABLE_DOTNET || NETFX_CORE))
using System.IO;
using UnityEngine;

namespace MoonSharp.Interpreter.Loaders
{
	/// <summary>
	/// A script loader loading scripts from /StreamingAssets/Lua/. When loading files the subpath of the aforementioned
	/// folder should be used.
	/// </summary>
	public class StreamingAssetsScriptLoader : ScriptLoaderBase
	{
		/// <summary>
		/// Checks if a script file exists.
		/// </summary>
		/// <param name="name">The script filename.</param>
		/// <returns>True if the file exists, false otherwise.</returns>
		public override bool ScriptFileExists(string name)
		{
			// Get the full path.
			string fullPath = Path.Combine(Application.streamingAssetsPath, "Lua", name);

			// Append with .lua if not provided.
			if (!fullPath.EndsWith(".lua"))
			{
				fullPath += ".lua";
			}

			return File.Exists(fullPath);
		}

		/// <summary>
		/// Opens a file for reading the script code.
		/// It can return either a string, a byte[] or a Stream.
		/// If a byte[] is returned, the content is assumed to be a serialized (dumped) bytecode. If it's a string, it's
		/// assumed to be either a script or the output of a string.dump call. If a Stream, autodetection takes place.
		/// </summary>
		/// <param name="path">The file path (after StreamingAssets/Lua/).</param>
		/// <param name="globalContext">The global context.</param>
		/// <returns>
		/// A string, a byte[] or a Stream.
		/// </returns>
		public override object LoadFile(string path, Table globalContext)
		{
			// Get the full path.
			string fullPath = Path.Combine(Application.streamingAssetsPath, "Lua", path);

			// Append with .lua if not provided.
			if (!fullPath.EndsWith(".lua"))
			{
				fullPath += ".lua";
			}

			return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		}
	}
}
#endif