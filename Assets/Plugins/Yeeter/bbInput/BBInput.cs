using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using Tyd;
using UnityEngine;
using Yeeter;

/// <summary>
/// A very basic input wrapper that allows for event-driven input
/// with the ability to prioritise listeners and which gives listeners the ability to eat input.
/// Usage: Call LoadProfiles(), then Initialize().
/// </summary
[MoonSharpUserData]
public class BBInput
{
    private static BBInputProfile _previousProfile = null;
    public static List<BBInputProfile> Profiles { get; private set; } = new List<BBInputProfile>();

    /// <summary>
    /// Loads all input profiles from the defs.
    /// </summary>
    public static void LoadProfiles()
    {
        InGameDebug.Log("-----BBInput: Loading input profiles...");
        var profilesNode = StreamingAssetsDatabase.GetDef("Input.Profiles") as TydTable;
        foreach (var profileNode in profilesNode.Nodes)
        {
            Profiles.Add(BBInputProfile.FromTydTable(profileNode as TydTable));
        }
        InGameDebug.Log("-----Input profiles loaded.");
    }
    /// <summary>
    /// Initialises the input system. Call after LoadProfiles.
    /// </summary>
    public static void Initialize()
    {
        InGameDebug.Log("BBInput initialized.");
        var go = GameObject.Instantiate(new GameObject());
        go.AddComponent<BBInputUpdater>();
        go.name = "BBInputUpdater";
        GameObject.DontDestroyOnLoad(go);
    }
    public static void SetActiveProfile(string profileName)
    {
        foreach (var profile in Profiles)
        {
            if (profile.AlwaysEnabled) continue;
            if (profile.Enabled && profile.Name != profileName) _previousProfile = profile;
            if (profileName == profile.Name)
            {
                profile.Enabled = true;
            }
            else
            {
                profile.Enabled = false;
            }
        }
    }
    public static void ActivatePreviousProfile()
    {
        if (_previousProfile == null) return;
        _previousProfile.Enabled = true;
        foreach (var profile in Profiles)
        {
            if (profile.AlwaysEnabled) continue;
            if (profile.Enabled && profile != _previousProfile) _previousProfile = profile;
        }
    }
    /// <summary>
    /// Adds an action to be called when an axis is pressed.
    /// </summary>
    /// <param name="axisName">Te name of the axis.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="priority">The priority of the action.</param>
    [MoonSharpHidden]
    public static void AddOnAxisPressed(string axisName, Action action, int priority = 0)
    {
        foreach (var profile in Profiles)
        {
            foreach (var axis in profile.Axes)
            {
                if (axis.Name == axisName)
                {
                    axis.AddOnPressed(action, priority);
                    return;
                }
            }
        }
        InGameDebug.Log("No axis with name '" + axisName + "'.");
    }
    /// <summary>
    /// Adds an action to be called when an axis is pressed.
    /// </summary>
    /// <param name="axisName">Te name of the axis.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="priority">The priority of the action.</param>
    public static void AddOnAxisPressed(string axisName, DynValue action, int priority = 0)
    {
        foreach (var profile in Profiles)
        {
            foreach (var axis in profile.Axes)
            {
                if (axis.Name == axisName)
                {
                    axis.AddOnPressed(() => LuaManager.Call(action), priority);
                    return;
                }
            }
        }
        InGameDebug.Log("No axis with name '" + axisName + "'.");
    }
    /// <summary>
    /// Adds an action to be called when an axis is held.
    /// </summary>
    /// <param name="axisName">Te name of the axis.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="priority">The priority of the action.</param>
    [MoonSharpHidden]
    public static void AddOnAxisHeld(string axisName, Action action, int priority = 0)
    {
        foreach (var profile in Profiles)
        {
            foreach (var axis in profile.Axes)
            {
                if (axis.Name == axisName)
                {
                    axis.AddOnHeld(action, priority);
                    return;
                }
            }
        }
        InGameDebug.Log("No axis with name '" + axisName + "'.");
    }
    /// <summary>
    /// Adds an action to be called when an axis is held.
    /// </summary>
    /// <param name="axisName">Te name of the axis.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="priority">The priority of the action.</param>
    public static void AddOnAxisHeld(string axisName, DynValue action, int priority = 0)
    {
        foreach (var profile in Profiles)
        {
            foreach (var axis in profile.Axes)
            {
                if (axis.Name == axisName)
                {
                    axis.AddOnHeld(() => LuaManager.Call(action), priority);
                    return;
                }
            }
        }
        InGameDebug.Log("No axis with name '" + axisName + "'.");
    }
    /// <summary>
    /// Adds an action to be called when an axis is released.
    /// </summary>
    /// <param name="axisName">Te name of the axis.</param>
    /// <param name="action">The action to perform.</param>
    /// <param name="priority">The priority of the action.</param>
    [MoonSharpHidden]
    public static void AddOnAxisReleased(string axisName, Action action, int priority = 0)
    {
        foreach (var profile in Profiles)
        {
            foreach (var axis in profile.Axes)
            {
                if (axis.Name == axisName)
                {
                    axis.AddOnReleased(action, priority);
                    return;
                }
            }
        }
        InGameDebug.Log("No axis with name '" + axisName + "'.");
    }/// <summary>
     /// Adds an action to be called when an axis is released.
     /// </summary>
     /// <param name="axisName">Te name of the axis.</param>
     /// <param name="action">The action to perform.</param>
     /// <param name="priority">The priority of the action.</param>
    public static void AddOnAxisReleased(string axisName, DynValue action, int priority = 0)
    {
        foreach (var profile in Profiles)
        {
            foreach (var axis in profile.Axes)
            {
                if (axis.Name == axisName)
                {
                    axis.AddOnReleased(() => LuaManager.Call(action), priority);
                    return;
                }
            }
        }
        InGameDebug.Log("No axis with name '" + axisName + "'.");
    }
}