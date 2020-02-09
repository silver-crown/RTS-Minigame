using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public abstract class ScriptableObjectBase : ScriptableObject
{
    protected JsonSerializerSettings GetJsonSerializerSettings()
    {
        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new StringEnumConverter());

        // cant remember all of the settings off the top of my head
        // TODO: match settings locally

        return settings;
    }

    /// <summary>
    /// Should be called at startup to retrieve the external settings
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadRemote()
    {
        //todo
        //yield GetRemoteFile
        string jsondata = "{}";

        LoadData(jsondata);

        yield break;
    }

    /// <summary>
    /// Should be called to send the information to the server
    /// TODO: not sure if coroutine is a good idea, since this wont work in editor mode
    /// </summary>
    /// <returns></returns>
    public IEnumerator SaveRemote()
    {
        var jsondata = GetSaveData();

        //todo
        //yield SendRemoteFile jsondata

        yield break;
    }

    /// <summary>
    /// Load data into your object after receiving the file from the server
    /// </summary>
    /// <param name="data"></param>
    public abstract void LoadData(string data);

    /// <summary>
    /// Serialises the Scriptable Objects data and sends it to the server
    /// </summary>
    /// <returns></returns>
    public abstract string GetSaveData();
}
