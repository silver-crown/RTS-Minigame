using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.Lua
{
    [MoonSharpUserData]
    public class ObjectBuilder
    {
        private static Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

        public static int Create()
        {
            int id = WorldInfo.NextId++;
            var go = GameObject.Instantiate(new GameObject());
            _objects.Add(id, go);
            go.name = go.name.Replace("(Clone)", "");
            InGameDebug.Log("Created GameObject '" + go.name + "' with id " + id + ".");
            return id;
        }

        public static int Create(string prefabName)
        {
            prefabName = prefabName.Replace(".", "/");
            int id = WorldInfo.NextId++;
            var go = Resources.Load<GameObject>("Prefabs/" + prefabName);
            GameObject.Instantiate(go);
            _objects.Add(id, go);
            go.name = go.name.Replace("(Clone)", "");
            InGameDebug.Log("Created GameObject '" + go.name + "' with id " + id + ".");
            return id;
        }

        public static void AddComponent(int id, string componentName, Table parameters = null)
        {
            var go = _objects[id];
            switch (componentName)
            {
                case "Miner":
                    go.AddComponent<Miner>();
                    break;
                case "Inventory":
                    var inventory = go.AddComponent<Inventory>();
                    inventory.Capacity = (int)parameters.Get("capacity").Number;
                    break;
            }
            InGameDebug.Log(go.name + " (id " + id + "): Added component " + componentName + ".");
            LogComponent(id, componentName, 1);
        }

        public static void LogComponent(int id, string componentName, int tabs = 0)
        {
            var go = _objects[id];
            string message = "";
            for (int i = 0; i < tabs; i++) message += "\t";
            switch (componentName)
            {
                case "Miner":
                    message += go.GetComponent<Miner>();
                    break;
                case "Inventory":
                    message += go.GetComponent<Inventory>();
                    break;
            }
            InGameDebug.Log(message);
        }

        public static void SetName(int id, string name)
        {
            string oldName = _objects[id].name;
            _objects[id].name = name;
            InGameDebug.Log("'" + oldName + "' renamed to '" + name + "'.");
        }
    }
}