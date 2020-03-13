using UnityEditor;
using UnityEngine;

namespace ExtensionMethods
{
    #if UNITY_EDITOR
    /// <summary>
    /// Contains extension methods for UnityEditor.AssetDatabase.
    /// </summary>
    public static class AssetDatabaseWrapper
    {
        /// <summary>
        /// Finds and returns a texture in the asset database.
        /// </summary>
        /// <param name="query">The search term to use.</param>
        /// <returns>The texture matching the query.</returns>
        public static Texture2D FindTexture2D(string query)
        {
            var guid = AssetDatabase.FindAssets(query + " t:texture2d")[0];
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
    }

    #endif
}