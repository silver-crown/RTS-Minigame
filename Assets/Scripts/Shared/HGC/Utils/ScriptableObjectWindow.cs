using System;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
internal class EndNameEdit : UnityEditor.ProjectWindowCallback.EndNameEditAction
{
	#region implemented abstract members of EndNameEditAction
	public override void Action (int instanceId, string pathName, string resourceFile)
	{
        UnityEditor.AssetDatabase.CreateAsset(UnityEditor.EditorUtility.InstanceIDToObject(instanceId), UnityEditor.AssetDatabase.GenerateUniqueAssetPath(pathName));
	}

	#endregion
}

/// <summary>
/// Scriptable object window.
/// </summary>
public class ScriptableObjectWindow : UnityEditor.EditorWindow
{
    private static string _lastName;
    private int _selectedIndex;
	private static string[] _names;	
	private static Type[] _types;
	
	private static Type[] Types
	{ 
		get { return _types; }
		set
		{
			_types = value;
			_names = _types.Select(t => t.FullName).ToArray();
		}
	}

    public static void Init(Type[] scriptableObjects)
	{
		Types = scriptableObjects;
        ScriptableObjectWindow window = (ScriptableObjectWindow)UnityEditor.EditorWindow.GetWindow(typeof(ScriptableObjectWindow));
		window.Show();

        if(!_names.IsNullOrEmpty())
            _lastName = GetUnqualifiedTypeName(_names[0]);
    }

    public static string GetUnqualifiedTypeName(string qualifiedType)
    {
        if (qualifiedType.Contains("."))
            return qualifiedType.Remove(0, qualifiedType.LastIndexOf(".") + 1);
        else
            return qualifiedType;
    }

	public void OnGUI()
	{
		GUILayout.Label("ScriptableObject Class", UnityEditor.EditorStyles.boldLabel);

        if (_names.IsNullOrEmpty())
            return;

        _lastName = UnityEditor.EditorGUILayout.TextField("Asset Name", _lastName);
        int newIndex = UnityEditor.EditorGUILayout.Popup("Type", _selectedIndex, _names);

        if(newIndex != _selectedIndex)
        {
            _selectedIndex = newIndex;
            _lastName = GetUnqualifiedTypeName(_names[_selectedIndex]);
        }

		if (GUILayout.Button("Create Asset"))
		{
			var asset = ScriptableObject.CreateInstance(_types[_selectedIndex]);
            UnityEditor.ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
				asset.GetInstanceID(),
				ScriptableObject.CreateInstance<EndNameEdit>(),
				string.Format("{0}.asset", _lastName),
                UnityEditor.AssetPreview.GetMiniThumbnail(asset), 
				null);

			Close();
		}
	}
}

#endif