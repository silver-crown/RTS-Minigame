using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

namespace HGC
{
    [System.Flags]
    public enum ExcludeType
    {
        None = 0,
        Interface = 1,
        Generic = 2,
        Abstract = 4,

        Protocol = Interface | Abstract
    }

    public static class TypeUtil
    {
        public static string GetUnqualifiedTypeName<Type>() where Type : class
        {
            string qualifiedType = (typeof(Type)).ToString();

            if (qualifiedType.Contains("."))
                return qualifiedType.Remove(0, qualifiedType.LastIndexOf(".") + 1);
            else
                return qualifiedType;
        }

        public static string GetUnqualifiedTypeName(System.Type type)
        {
            string qualifiedType = type.ToString();

            if (qualifiedType.Contains("."))
                return qualifiedType.Remove(0, qualifiedType.LastIndexOf(".") + 1);
            else
                return qualifiedType;
        }

        public static string GetUnqualifiedTypeName(string qualifiedType)
        {
            if (qualifiedType.Contains("."))
                return qualifiedType.Remove(0, qualifiedType.LastIndexOf(".") + 1);
            else
                return qualifiedType;
        }       
    }
}
