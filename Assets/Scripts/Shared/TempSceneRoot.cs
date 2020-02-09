using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Linq;

public class TempSceneRoot : MonoBehaviour
{
    [System.Serializable]
    public class AllScenesLoaded : UnityEvent<bool>
    {
    }

    [SerializeField]
    private List<string> _subscenes;

    [SerializeField]
    private AllScenesLoaded _onAllScenesLoaded;

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
        bool hasError = false;
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
                hasError = true;
                continue;
            }

            while (!async.isDone)
                yield return null;


            Debug.Log("Successfully loaded subscene: " + subscenePath, this);
        }

        // wait one frame to ensure all Awake and Start methods are called for every scene component
        yield return 0;

        Debug.Log("Successfully loaded dependencies", this);

        // the OnScenesLoaded method will be called after Awake and Start
        if (_onAllScenesLoaded != null)
            _onAllScenesLoaded.Invoke(!hasError);
}
}
