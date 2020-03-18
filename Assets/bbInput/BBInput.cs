using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A very basic input wrapper that allows for event-driven input
/// with the ability to prioritise listeners and which gives listeners the ability to eat input.
/// </summary>
public class BBInput : MonoBehaviour
{
    private static List<KeyCode> _eaten = new List<KeyCode>();
    private static List<KeyCode> _keys = new List<KeyCode>();

    private static SortedDictionary<int, Dictionary<KeyCode, Action>> _onKeyDown =
        new SortedDictionary<int, Dictionary<KeyCode, Action>>();
    private static SortedDictionary<int, Dictionary<KeyCode, Action>> _onKeyUp =
        new SortedDictionary<int, Dictionary<KeyCode, Action>>();
    private static SortedDictionary<int, Dictionary<KeyCode, Action>> _onKey =
        new SortedDictionary<int, Dictionary<KeyCode, Action>>();

    public static Action OnEatenKeysReset;

    private void Update()
    {
        _eaten = new List<KeyCode>();
        OnEatenKeysReset?.Invoke();

        void Handle(SortedDictionary<int, Dictionary<KeyCode, Action>> keyEvents, Func<KeyCode, bool> isTriggered)
        {
            foreach (var order in keyEvents.Keys)
            {
                foreach (var keyCode in keyEvents[order].Keys)
                {
                    if (!_eaten.Contains(keyCode) && isTriggered(keyCode))
                    {
                        keyEvents[order][keyCode]?.Invoke();
                    }
                }
            }
        }
        Handle(_onKeyDown, Input.GetKeyDown);
        Handle(_onKeyUp, Input.GetKeyUp);
        Handle(_onKey, Input.GetKey);

    }

    private static void Add(
        SortedDictionary<int, Dictionary<KeyCode, Action>> keyEvents,
        KeyCode keyCode,
        Action action,
        int order)
    {
        if (!keyEvents.ContainsKey(order))
        {
            keyEvents[order] = new Dictionary<KeyCode, Action>();
        }
        if (!keyEvents[order].ContainsKey(keyCode))
        {
            keyEvents[order].Add(keyCode, null);
        }
        keyEvents[order][keyCode] += action;

        if (!_keys.Contains(keyCode))
        {
            _keys.Add(keyCode);
        }
    }

    public static void AddOnKeyDown(KeyCode keyCode, Action action, int order = 0)
    {
        Add(_onKeyDown, keyCode, action, order);
    }

    public static void AddOnKeyUp(KeyCode keyCode, Action action, int order = 0)
    {
        Add(_onKeyUp, keyCode, action, order);
    }

    public static void AddOnKey(KeyCode keyCode, Action action, int order = 0)
    {
        Add(_onKey, keyCode, action, order);
    }

    public static void EatAll()
    {
        _eaten.AddRange(_keys);
    }

    public static void UnEat(KeyCode keyCode)
    {
        _eaten.Remove(keyCode);
    }
}