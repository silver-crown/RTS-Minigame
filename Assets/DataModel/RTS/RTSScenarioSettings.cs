using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace RTS.DataModel
{
    public enum CIStrategy
    {
        ResourceExpansion,
        HuntAndDestroy
    }

    // TODO: add your settings for the scenario here - Do not use UnityTypes, such as Quaternion and Vector3. Make you own if needed
    [System.Serializable]
    public class ScenarioDefinition
    {
        public string Name;
        public CIStrategy Strategy;
    }

    [System.Serializable]
    public class Vector3Data
    {
        public float X;
        public float Y;
        public float Z;
    }

    public class RTSScenarioSettings : ScenarioSettings
    {
        [SerializeField]
        public string Name;

        [SerializeField]
        public CIStrategy Strategy;

        public override void ExecuteScenarioSetup()
        {
            
        }

        public override string GetSaveData()
        {
            return "";
        }

        public override void LoadData(string data)
        {
            
        }
    }
}
