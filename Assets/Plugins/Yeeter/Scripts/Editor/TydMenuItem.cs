using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds a "Create>TyD" option the the Assets menu.
/// </summary>
public class TydMenuItem
{
    /// <summary>
    /// Creates a .TyD in the assets folder.
    /// </summary>
    [MenuItem("Assets/Create/TyD", priority = 82)]
    public static void CreateTydFile()
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
        string fileName = "New TyD.tyd";
        int fileNameSuffixIndex = 1;
        while (File.Exists(Path.Combine(folderPath, fileName)))
        {
            fileName = "New TyD" + fileNameSuffixIndex + ".tyd";
            fileNameSuffixIndex++;
        }
        string filePath = Path.Combine(folderPath, fileName);

        // Add the file.
        File.Create(filePath).Close();
        AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceSynchronousImport);
    }
}
