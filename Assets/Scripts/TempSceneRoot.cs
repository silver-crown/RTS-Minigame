using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class TempSceneRoot : MonoBehaviour
{
    [SerializeField]
    List<string> _subscenes;

    private void Awake()
    {
        if (_subscenes == null)
            _subscenes = new List<string>();
    }

    /// <summary>
    /// Returns true if the scene after the Scene Folder is loaded, otherwise returns false
    /// </summary>
    /// <param name="relativeScenePath">Assets/Scenes/"relativeScenePath".unity</param>
    /// <returns></returns>
    public bool IsSceneLoaded(string relativeScenePath)
    {
        string subsceneAssetPath = "Assets/Scenes/" + relativeScenePath + ".unity";
#if UNITY_EDITOR
        var scenes = UnityEditor.SceneManagement.EditorSceneManager.GetAllScenes();

        for (int i = 0; i < UnityEditor.SceneManagement.EditorSceneManager.loadedSceneCount; i++)
        {
            var loadedScene = UnityEditor.SceneManagement.EditorSceneManager.GetSceneAt(i);
            Debug.Log("Found existing scene: " + loadedScene.path);

            if (string.Equals(loadedScene.path, subsceneAssetPath, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
#else
        var scenes = SceneManager.GetAllScenes();

        for (int i = 0; i < SceneManager.loadedSceneCount; i++)
        {
            var loadedScene = SceneManager.GetSceneAt(i);
            Debug.Log("Found existing scene: " + loadedScene.path);

            if (string.Equals(loadedScene.path, subsceneAssetPath, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
#endif

        return false;
    }

    private IEnumerator Start()
    {
        foreach (var scene in _subscenes)
        {
            string subscenePath = "Scenes/" + scene;

            if(IsSceneLoaded(scene))
            {
                Debug.Log("Scene is already loaded: " + subscenePath, this);
                continue;
            }

            var async = SceneManager.LoadSceneAsync(subscenePath, LoadSceneMode.Additive);

            if (async == null)
            {
                Debug.LogError("Incorrect subscene name: " + subscenePath, this);
                continue;
            }

            while (!async.isDone)
                yield return null;


            Debug.Log("Successfully loaded subscene: " + subscenePath, this);
        }

        Debug.Log("Successfully loaded dependencies", this);
    }

    /*
    /// <summary>
    /// TODO: modding configuration for the scenes - dependency injection lite
    /// </summary>
    /// <returns></returns>
    public SceneConfiguration GetSceneConfig()
    {

    }
    */
}
