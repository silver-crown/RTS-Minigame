using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace RTS.DataModel
{
    // the data that is saved and stored remotely
    [System.Serializable]
    public class AllScenarioDefinitions
    {
        public int Version;
        public List<ScenarioDefinition> Definitions;
    }

    public class AllScenarios : ScriptableObjectBase
    {
        [SerializeField]
        public int Version;

        // the data that is rendered in the inspector and used by the game
        [SerializeField]
        private List<ScenarioDefinition> Definitions;

        public override string GetSaveData()
        {
            var data = new AllScenarioDefinitions();
            data.Version = Version;
            data.Definitions = Definitions;

            return JsonConvert.SerializeObject(data);
        }

        public override void LoadData(string data)
        {
            var remoteData = JsonConvert.DeserializeObject<AllScenarioDefinitions>(data, GetJsonSerializerSettings());

            // validate for data errors

            if(remoteData == null)
            {
                Debug.LogError("Data was null");
                return;
            }

            if(remoteData.Version <= Version)
            {
                Debug.LogError("Current Version is uptodate: " + Version + " (remote: " + remoteData.Version + ")");
            }

            // check using extension method
            if(remoteData.Definitions.IsNullOrEmpty())
            {
                Debug.LogError("There were no scenarios");
            }

            Version = remoteData.Version;
            Definitions = remoteData.Definitions;
        }
    }
}
