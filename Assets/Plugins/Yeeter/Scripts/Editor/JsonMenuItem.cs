using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds a "Create>Json" option the the Assets menu.
/// </summary>
public class JsonMenuItem
{
    /// <summary>
    /// Creates a Json in the assets folder.
    /// </summary>
    [MenuItem("Assets/Create/Json", priority = 82)]
    public static void CreateJsonScript()
    {
        // Get the selected path.
        string folderPath = "Assets";
        foreach (var selectedObject in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            folderPath = AssetDatabase.GetAssetPath(selectedObject);
            if (File.Exists(folderPath))
            {
                folderPath = Path.GetDirectoryName(folderPath);
            }
            break;
        }
        string fileName = "New Json.json";
        int fileNameSuffixIndex = 1;
        while (File.Exists(Path.Combine(folderPath, fileName)))
        {
            fileName = "New Json" + fileNameSuffixIndex + ".json";
            fileNameSuffixIndex++;
        }
        string filePath = Path.Combine(folderPath, fileName);

        // Add the file.
        File.Create(filePath).Close();
        AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceSynchronousImport);
    }
}
