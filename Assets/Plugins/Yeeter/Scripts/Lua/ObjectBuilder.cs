using MoonSharp.Interpreter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using Object = UnityEngine.Object;

namespace Yeeter
{
    /// <summary>
    /// Contains Lua-compatible methods for building and managing GameObjects.
    /// </summary>
    [Preserve, MoonSharpUserData]
    public class ObjectBuilder
    {
        /// <summary>
        /// The id of the next GameObject.
        /// </summary>
        private static int _nextId = 0;
        /// <summary>
        /// Maps ids to GameObjects.
        /// </summary>
        private static Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
        /// <summary>
        /// Maps GameObject to ids.
        /// </summary>
        private static Dictionary<GameObject, int> _ids = new Dictionary<GameObject, int>();

        /// <summary>
        /// Adds an object the _objects and _ids dictionaries.
        /// </summary>
        /// <param name="gameObject">The GameObject to add.</param>
        /// <returns>The GameObject's id.</returns>
        private static DynValue AddObject(GameObject gameObject)
        {
            _objects.Add(_nextId, gameObject);
            _ids.Add(gameObject, _nextId);
            foreach (var child in gameObject.transform)
            {
                var go = child as GameObject;
                if (go == null) continue;
                if (!_ids.ContainsKey(go)) AddObject(go);
            }
            gameObject.name = gameObject.name.Replace("(Clone)", "");
            if (InGameDebug.LogObjectCreated)
            {
                InGameDebug.Log("Created GameObject '" + gameObject.name + "' with id " + _nextId + ".");
            }
            return DynValue.NewNumber(_nextId++);
        }

        /// <summary>
        /// Instantiates a new GameObject from a prefab.
        /// </summary>
        /// <param name="path">The path to the prefab relative to "Resources/Prefabs/".</param>
        /// <returns>The id of the instantiated GameObject.</returns>
        public static DynValue Instantiate(string path)
        {
            if (!path.StartsWith("Prefabs/"))
            {
                path = "Prefabs/" + path;
            }
            var go = Object.Instantiate(Resources.Load<GameObject>(path));
            return AddObject(go);
        }
        /// <summary>
        /// Instantiates a new UI element and sets its parent to the first canvas found in the scene.
        /// </summary>
        /// <returns>The id of the instantiated GameObject.</returns>
        public static DynValue InstantiateUIElement()
        {
            var go = Object.Instantiate(new GameObject(), Object.FindObjectOfType<Canvas>().transform);
            go.AddComponent<RectTransform>();
            return AddObject(go);
        }
        /// <summary>
        /// Instantiates a new UI element from a prefab and sets its parent to the first canvas found in the scene.
        /// </summary>
        /// <param name="path">The path to the prefab relative to "Resources/Prefabs/".</param>
        /// <returns>The id of the instantiated GameObject.</returns>
        public static DynValue InstantiateUIElement(string path)
        {
            var go = Object.Instantiate(
                Resources.Load<GameObject>("Prefabs/UI/" + path),
                Object.FindObjectOfType<Canvas>().transform);
            return AddObject(go);
        }
        /// <summary>
        /// Creates a new empty GameObject at a given position.
        /// </summary>
        /// <param name="x">The x position of the GameObject.</param>
        /// <param name="y">The x position of the GameObject.</param>
        /// <returns>The id of the created GameObject.</returns>
        public static DynValue Create(float x = 0, float y = 0)
        {
            var go = Object.Instantiate(new GameObject(), new Vector3(x, y), Quaternion.identity, null);
            return AddObject(go);
        }
        /// <summary>
        /// Gets the ids of all child GameObjects of a GameObject with a given id.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <returns>The child ids.</returns>
        public static List<int> GetChildIds(int id)
        {
            var result = new List<int>();
            foreach (var child in Get(id).transform)
            {
                var go = child as GameObject;
                if (go != null)
                {
                    result.Add(_ids[go]);
                }
            }
            return result;
        }
        /// <summary>
        /// Gets a GameObject with a given id.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <returns>The GameObject with the given id.</returns>
        public static GameObject Get(int id)
        {
            if (!_objects.ContainsKey(id))
            {
                return null;
            }
            var go = _objects[id];
            if (go == null)
            {
                _objects.Remove(id);
                return null;
            }
            return _objects[id];
        }
        /// <summary>
        /// Gets a GameObject's position.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <returns>The GameObject's position.</returns>
        public static Vector3 GetPosition(int id)
        {
            return Get(id).transform.position;
        }
        /// <summary>
        /// Destroys a GameObject with a given id.
        /// </summary>
        /// <param name="id">The id of the GameObject to destroy.</param>
        public static void Destroy(int id)
        {
            Object.Destroy(Get(id));
        }
        /// <summary>
        /// Sets the name of a GameObject.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="name">The name of the GameObject.</param>
        public static void SetName(int id, string name)
        {
            Get(id).name = name;
        }
        /// <summary>
        /// Adds a LuaObjectComponent to a GameObject.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="type">The LuaObjectComponent type.
        /// This is the path to the Lua script to use using the rules of the active ScriptLoader.</param>
        public static void AddLuaObjectComponent(int id, string type)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return;
            }
            var luaComponent = _objects[id].AddComponent<LuaObjectComponent>();
            luaComponent.Load(type);
        }
        /// <summary>
        /// Adds an action to a GameObject which is repeatedly invoked at a specified interval.
        /// <para>This adds a new Timer which is responsible for invoking the action.</para>
        /// </summary>
        /// <param name="id">The id of the GameObject to attach the timed action to.</param>
        /// <param name="interval">The time in seconds between each time the action is invoked.</param>
        /// <param name="action">The action to invoke.</param>
        public static void AddTimedAction(int id, float interval, DynValue action)
        {
            var go = Get(id);
            var timer = go.AddComponent<Timer>();
            timer.Interval = interval;
            timer.OnTick = () => LuaManager.Call(action);
        }
        /// <summary>
        /// Attaches a new sprite to a GameObject's SpriteRender, or a SpriteRenderer in its children.
        /// A new SpriteRenderer is created if none was found.
        /// <para></para>
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="textureKey">The texture's key in the StreamingAssetsDatabase.</param>
        public static void SetTexture(int id, string textureKey)
        {
            var go = Get(id);
            var sr = go.GetComponentInChildren<SpriteRenderer>();
            if (sr == null)
            {
                sr = go.AddComponent<SpriteRenderer>();
            }
            if (textureKey == null)
            {
                sr.sprite = null;
            }
            else
            {
                var texture = StreamingAssetsDatabase.GetTexture(textureKey);
                sr.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
        }
        /// <summary>
        /// Sets the colour of a GameObject's SpriteRenderer, or a SpriteRenderer in its children.
        /// A new SpriteRenderer is created if none was found.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="r">The red colour component. 0-255.</param>
        /// <param name="g">The green colour component. 0-255.</param>
        /// <param name="b">The blue colour component. 0-255.</param>
        /// <param name="a">The alpha colour component. 0-255.</param>
        public static void SetColor(int id, int r, int g, int b, int a = 255)
        {
            var go = Get(id);
            var sr = go.GetComponentInChildren<SpriteRenderer>();
            sr.color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
        /// <summary>
        /// Sets the sorting layer of a GameObject's SpriteRenderer, or a SpriteRenderer in its children.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="layer">The new sorting layer of the SpriteRenderer.</param>
        public static void SetSortingLayer(int id, string layer)
        {
            var go = Get(id);
            var sr = go.GetComponentInChildren<SpriteRenderer>();
            if (sr == null) sr = go.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = layer;
        }
        /// <summary>
        /// Sets the local scale of a GameObject's transform.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="x">The scale's x component.</param>
        /// <param name="y">The scale's y component.</param>
        /// <param name="z">The scale's z component.</param>
        public static void SetScale(int id, float x, float y, float z)
        {
            var go = Get(id);
            go.transform.localScale = new Vector3(x, y, z);
        }
        /// <summary>
        /// Sets a GameObject's position.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="x">The position's x component.</param>
        /// <param name="y">The position's y component.</param>
        public static void SetPosition(int id, float x, float y)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return;
            }
            _objects[id].transform.position = new Vector3(x, y, _objects[id].transform.position.z);
        }
        /// <summary>
        /// Sets a GameObject's position.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="x">The position's x component.</param>
        /// <param name="y">The position's y component.</param>
        /// <param name="z">The position's z component.</param>
        public static void SetPosition(int id, float x, float y, float z)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return;
            }
            _objects[id].transform.position = new Vector3(x, y, z);
        }
        /// <summary>
        /// Moves a GameObject's transform.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        /// <param name="x">The x component of the distance to move the GameObject.</param>
        /// <param name="y">The y component of the distance to move the GameObject.</param>
        /// <param name="z">The z component of the distance to move the GameObject.</param>
        public static void Translate(int id, float x, float y, float z = 0)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return;
            }
            _objects[id].transform.position += new Vector3(x, y, z);
        }
        /// <summary>
        /// Sets the parent of a GameObject's transform.
        /// </summary>
        /// <param name="childId">The id of the child GameObject.</param>
        /// <param name="parentId">The id of the parent GameObject.</param>
        public static void SetParent(int childId, int parentId)
        {
            Get(childId).transform.SetParent(Get(parentId).transform, false);
        }
        /// <summary>
        /// Disables a GameObject if it was enabled and vice versa.
        /// </summary>
        /// <param name="id">The id of the GameObject.</param>
        public static void ToggleEnabled(int id)
        {
            var go = Get(id);
            go.SetActive(!go.activeSelf);
        }
        /// <summary>
        /// Tells Unity not to destroy a GameObject when loading a new scene.
        /// </summary>
        /// <param name="id">The GameObject's id.</param>
        public static void DontDestroyOnLoad(int id)
        {
            GameObject.DontDestroyOnLoad(Get(id));
        }
        /// <summary>
        /// Creates a new GameObject with a LuaObjectComponent.
        /// </summary>
        /// <param name="type">The LuaObjectComponent type.
        /// This is the path to the Lua script to use using the rules of the active ScriptLoader.</param>
        /// <returns>The id of the newly created GameObject.</returns>
        public static int CreateLuaObject(string type)
        {
            var go = Object.Instantiate(new GameObject());
            go.name = type;
            int id = (int)AddObject(go).Number;
            AddLuaObjectComponent(id, type);
            return id;
        }
        /// <summary>
        /// Adds an externally created GameObject to be accessible through the ObjectBuilder class.
        /// </summary>
        /// <param name="gameObject">The GameObject to add.</param>
        /// <returns>The id created for the GameObject.</returns>
        public static int AddExternalGameObject(GameObject gameObject)
        {
            if (_objects.ContainsValue(gameObject))
            {
                return _ids[gameObject];
            }
            else
            {
                return (int)AddObject(gameObject).Number;
            }
        }
        /// <summary>
        /// Gets the id of a GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to get the id of.</param>
        /// <returns>The id of the GameObject.</returns>
        public static int GetId(GameObject gameObject)
        {
            if (!_ids.ContainsKey(gameObject))
            {
                //InGameDebug.Log("GameObject '" + gameObject.name + "' has no id.");
                return -1;
            }
            if (gameObject == null)
            {
                _ids.Remove(gameObject);
                return -1;
            }
            return _ids[gameObject];
        }
    }
}