using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HGC;

public static class CollectionExtension
{
    public static bool IsNullOrEmpty(this ICollection collection)
    {
        bool isValid = collection != null && collection.Count != 0;
        return !isValid;
    }

    /// <summary>
    /// IsNullOrEmpty alternative for generic enumerables
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static bool IsEnumerableNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        if (collection == null)
            return true;

        foreach (var item in collection)
            return false;

        return true;
    }

    public static void AddOrUpdate<T>(this IList<T> list, T value)
    {
        int index = list.IndexOf(value);
        if (index != -1)
        {
            list[index] = value;
        }
        else
        {
            list.Add(value);
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : class
    {
        if (key == null)
        {
            Debug.LogError("Key was null!");
            return;
        }

        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : class
        where TValue : class, new()
    {
        if (key == null)
        {
            Debug.LogError("Key was null!");
            return default(TValue);
        }

        if (dictionary.ContainsKey(key))
        {
            return dictionary[key];
        }
        else
        {
            var value = new TValue();
            dictionary.Add(key, value);
            return value;
        }
    }

    public static bool ExistsAtIndex<T>(this List<T> list, int index)
    {
        return (index >= 0 && list.Count != 0 && index < list.Count && list[index] != null);
    }

    public static bool TryGetItemAtIndex<T>(this List<T> list, int index, out T item) where T : class
    {
        bool exists = ExistsAtIndex<T>(list, index);

        if (exists)
            item = list[index];
        else
            item = null;

        return exists;
    }
}