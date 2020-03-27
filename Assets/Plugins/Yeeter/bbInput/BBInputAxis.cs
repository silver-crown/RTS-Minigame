using System;
using System.Collections.Generic;
using Tyd;
using UnityEngine;
using Yeeter;

/// <summary>
/// Represents an axis that can be bound in the BBInput system.
/// Analogous to Unity' Input axes.
/// </summary>
public class BBInputAxis
{
    /// <summary>
    /// Triggered when a button in the axis is pressed.
    /// </summary>
    private SortedList<int, Action> _onPressed = new SortedList<int, Action>();
    /// <summary>
    /// Triggered when a button in the axis is held.
    /// </summary>
    private SortedList<int, Action> _onHeld = new SortedList<int, Action>();
    /// <summary>
    /// Triggered when a button in the axis is released.
    /// </summary>
    private SortedList<int, Action> _onReleased = new SortedList<int, Action>();

    private bool _isHeld;
    private bool _wasPressedThisFrame;
    private bool _wasReleasedThisFrame;

    /// <summary>
    /// The name used to refer to the key in scripts.
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// The name of the key as it appears in the game.
    /// Example: An axis controls left-right movement.
    /// The label should be something like "Horizontal", or "Horizontal movement", or "Move horizontally", etc. 
    /// </summary>
    public string Label { get; private set; }
    /// <summary>
    /// The name of the positive axis direction as it appears in the game.
    /// Example: An axis controls left-right movement.
    /// The positive label should be something like "Move right" or "Right", etc. 
    /// </summary>
    public string PositiveLabel { get; private set; }
    /// <summary>
    /// The name of the positive axis direction as it appears in the game.
    /// Example: An axis controls left-right movement.
    /// The positive label should be something like "Move left" or "left", etc. 
    /// </summary>
    public string NegativeLabel { get; private set; }
    /// <summary>
    /// The description of the key.
    /// </summary>
    public string Description { get; private set; }
    /// <summary>
    /// The priority of the axis. The higher the number, the sooner it gets checked by the input system.
    /// This allows you to have two axes bound to the same key, assign one to a higher priority, and have the
    /// axis with the higher priority use up and "eat" the key (i.e. make it so no one else can use it for the
    /// duration of the current frame).
    /// </summary>
    public int Priority { get; private set; }
    /// <summary>
    /// Whether or not this axis is enabled. An axis is only checked if enabled.
    /// </summary>
    public bool Enabled { get; private set; }
    /// <summary>
    /// Whether the axis was pressed this frame.
    /// </summary>
    public bool WasPressedThisFrame
    {
        get => _wasPressedThisFrame;
        set
        {
            _wasPressedThisFrame = value;
            if (_wasPressedThisFrame)
            {
                foreach (var priority in _onPressed.Keys)
                {
                    _onPressed[priority]?.Invoke();
                }
            }
        }
    }
    /// <summary>
    /// Whether the axis is currently held.
    /// </summary>
    public bool IsHeld
    {
        get => _isHeld;
        set
        {
            _isHeld = value;
            if (_isHeld)
            {
                foreach (var priority in _onHeld.Keys)
                {
                    _onHeld[priority]?.Invoke();
                }
            }
        }
    }
    /// <summary>
    /// Whether the axis was released this frame.
    /// </summary>
    public bool WasReleasedThisFrame
    {
        get => _wasReleasedThisFrame;
        set
        {
            _wasReleasedThisFrame = value;
            if (_wasReleasedThisFrame)
            {
                foreach (var priority in _onReleased.Keys)
                {
                    _onReleased[priority]?.Invoke();
                }
            }
        }
    }
    /// <summary>
    /// The Unity key codes bound to the positive axis direction.
    /// This is a list of lists which each contain a combination of key codes
    /// which must all be pressed to trigger the key.
    /// Example as it appears in a .tyd file:
    /// positiveKeyCodes [[A]; [LeftShift; M]]
    /// The above would mean that the key is triggered by
    /// 1. Pressing A.
    /// and/or
    /// 2. Pressing left shift and M at the same time.
    /// </summary>
    public List<List<KeyCode>> PositiveKeyCodes { get; private set; }
    /// <summary>
    /// NOTE: Not implemented
    /// The Unity key codes bound to the negative axis direction.
    /// This is a list of lists which each contain a combination of key codes
    /// which must all be pressed to trigger the key.
    /// Example as it appears in a .tyd file:
    /// keyCodes [[D]; [RightShift; M]]
    /// The above would mean that the key is triggered by
    /// 1. Pressing D.
    /// and/or
    /// 2. Pressing right shift and M at the same time.
    /// </summary>
    public List<List<KeyCode>> NegativeKeyCodes { get; private set; }

    /// <summary>
    /// Adds an action to be called when the axis is pressed.
    /// </summary>
    /// <param name="action">The action to perform.</param>
    /// <param name="priority">The priority of the action.</param>
    public void AddOnPressed(Action action, int priority = 0)
    {
        if (!_onPressed.ContainsKey(-priority))
        {
            _onPressed.Add(-priority, action);
        }
        else
        {
            _onPressed[-priority] += action;
        }
    }
    /// <summary>
    /// Adds an action to be called when the axis is held.
    /// </summary>
    /// <param name="action">The action to perform.</param>
    /// <param name="priority">The priority of the action.</param>
    public void AddOnHeld(Action action, int priority = 0)
    {
        if (!_onHeld.ContainsKey(-priority))
        {
            _onHeld.Add(-priority, action);
        }
        else
        {
            _onHeld[-priority] += action;
        }
    }
    /// <summary>
    /// Adds an action to be called when the axis is released.
    /// </summary>
    /// <param name="action">The action to perform.</param>
    /// <param name="priority">The priority of the action.</param>
    public void AddOnReleased(Action action, int priority = 0)
    {
        if (!_onReleased.ContainsKey(-priority))
        {
            _onReleased.Add(-priority, action);
        }
        else
        {
            _onReleased[-priority] += action;
        }
    }

    /// <summary>
    /// Constructs a new BBInputAxis from A TydTable.
    /// </summary>
    /// <param name="table">The table to load the axis from.</param>
    /// <returns>The new BBInputAxis.</returns>
    public static BBInputAxis FromTydTable(TydTable table)
    {
        var axis = new BBInputAxis();
        InGameDebug.Log("Loading new BBInputAxis...");
        foreach (var node in table.Nodes)
        {
            switch (node.Name.ToLowerInvariant())
            {
                case "name":
                    axis.Name = (node as TydString).Value;
                    break;
                case "label":
                    axis.Label = (node as TydString).Value;
                    break;
                case "description":
                    axis.Label = (node as TydString).Value;
                    break;
                case "positivelabel":
                    axis.Label = (node as TydString).Value;
                    break;
                case "negativelabel":
                    axis.Label = (node as TydString).Value;
                    break;
                case "priority":
                    axis.Priority = int.Parse((node as TydString).Value);
                    break;
                case "keycodes":
                case "positivekeycodes":
                    axis.PositiveKeyCodes = new List<List<KeyCode>>();
                    foreach (var bindingNode in (node as TydList).Nodes)
                    {
                        var keyCodes = new List<KeyCode>();
                        foreach (var keyNode in (bindingNode as TydList).Nodes)
                        {
                            keyCodes.Add((KeyCode)Enum.Parse(typeof(KeyCode), (keyNode as TydString).Value));
                        }
                        axis.PositiveKeyCodes.Add(keyCodes);
                    }
                    break;
                case "negativekeycodes":
                    break;
            }
        }
        InGameDebug.Log("Loaded BBInputAxis: " + axis.Name);
        return axis;
    }
}