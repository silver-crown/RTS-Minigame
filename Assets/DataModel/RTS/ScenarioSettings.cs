using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace RTS.DataModel
{
    // TODO: add your settings for the scenario here - Do not use UnityTypes, such as Quaternion and Vector3. Make you own if needed
    [System.Serializable]
    public class ScenarioDefinition
    {
        public string Name;
    }

    [System.Serializable]
    public class Vector3Data
    {
        public float X;
        public float Y;
        public float Z;
    }

    public class ScenarioSettings : ScriptableObjectBase
    {
        [SerializeField]
        public string Name;

        public override string GetSaveData()
        {
            throw new NotImplementedException();
        }

        public override void LoadData(string data)
        {
            throw new NotImplementedException();
        }
    }
}
