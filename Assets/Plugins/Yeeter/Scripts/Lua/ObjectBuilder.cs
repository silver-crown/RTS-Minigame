using MoonSharp.Interpreter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Yeeter
{
    [Preserve, MoonSharpUserData]
    public class ObjectBuilder
    {
        private static int _nextId = 0;
        private static Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
        private static Dictionary<GameObject, int> _ids = new Dictionary<GameObject, int>();

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

        public static DynValue Instantiate(string path)
        {
            if (!path.StartsWith("Prefabs/"))
            {
                path = "Prefabs/" + path;
            }
            var go = Object.Instantiate(Resources.Load<GameObject>(path));
            return AddObject(go);
        }
        public static DynValue InstantiateUIElement()
        {
            var go = Object.Instantiate(new GameObject(), Object.FindObjectOfType<Canvas>().transform);
            go.AddComponent<RectTransform>();
            return AddObject(go);
        }
        public static DynValue InstantiateUIElement(string path)
        {
            var go = Object.Instantiate(
                Resources.Load<GameObject>("Prefabs/UI/" + path),
                Object.FindObjectOfType<Canvas>().transform);
            return AddObject(go);
        }
        public static DynValue Create(int x = 0, int y = 0)
        {
            var go = Object.Instantiate(new GameObject(), new Vector3(x, y), Quaternion.identity, null);
            return AddObject(go);
        }
        public static void Destroy(int id)
        {
            Object.Destroy(Get(id));
        }
        public static void SetName(int id, string name)
        {
            Get(id).name = name;
        }

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
        public static void AddLuaObjectComponent(string id, string type)
        {
            var go = GameObject.Find(id);
            if (go == null)
            {
                InGameDebug.Log("Couldn't find GameObject with name '" + id + "'.");
                return;
            }
            var luaComponent = GameObject.Find(id).AddComponent<LuaObjectComponent>();
            luaComponent.Load(type);
        }
        public static int CreateLuaObject(string type)
        {
            var go = Object.Instantiate(new GameObject());
            go.name = type;
            int id = (int)AddObject(go).Number;
            AddLuaObjectComponent(id, type);
            return id;
        }

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
        public static void SetColor(int id, int r, int g, int b, int a = 255)
        {
            var go = Get(id);
            var sr = go.GetComponentInChildren<SpriteRenderer>();
            sr.color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
        public static void SetSortingLayer(int id, string layer)
        {
            var go = Get(id);
            var sr = go.GetComponentInChildren<SpriteRenderer>();
            if (sr == null) sr = go.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = layer;
        }

        public static void SetScale(int id, float x, float y, float z)
        {
            var go = Get(id);
            go.transform.localScale = new Vector3(x, y, z);
        }
        public static void SetPosition(int id, float x, float y)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return;
            }
            _objects[id].transform.position = new Vector2(x, y);
        }
        public static Vector3 GetPosition(int id)
        {
            return Get(id).transform.position;
        }
        public static void SetPosition(string id, float x, float y)
        {
            var go = GameObject.Find(id);
            if (go == null)
            {
                InGameDebug.Log("Couldn't find GameObject with name '" + id + "'.");
                return;
            }
            go.transform.position = new Vector2(x, y);
        }
        public static void Translate(int id, float x, float y, float z = 0)
        {
            if (!_objects.ContainsKey(id))
            {
                InGameDebug.Log("No GameObject with id " + id + ".");
                return;
            }
            _objects[id].transform.position += new Vector3(x, y, z);
        }

        public static void SetParent(int childId, int parentId)
        {
            Get(childId).transform.SetParent(Get(parentId).transform, false);
        }

        public static void ToggleEnabled(int id)
        {
            var go = Get(id);
            go.SetActive(!go.activeInHierarchy);
        }

        public static void DontDestroyOnLoad(int id)
        {
            GameObject.DontDestroyOnLoad(Get(id));
        }

        public static List<int> GetChildIds(int id)
        {
            var result = new List<int>();
            foreach (var child in Get(id).transform)
            {
                var go = child as GameObject;
                if (go == null) continue;
                result.Add(_ids[go]);
            }
            return result;
        }

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