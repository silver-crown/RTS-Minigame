using MoonSharp.Interpreter;
using UnityEngine;
using UnityEngine.Scripting;

namespace Yeeter
{
    [Preserve, MoonSharpUserData, MoonSharpAlias("Input")]
    public class LuaInput
    {
        public static float MouseX { get => Input.mousePosition.x; }
        public static float MouseY { get => Input.mousePosition.y; }
        public static float MouseWorldX { get => Camera.main.ScreenToWorldPoint(Input.mousePosition).x; }
        public static float MouseWorldY { get => Camera.main.ScreenToWorldPoint(Input.mousePosition).y; }
        public static float DeltaX { get => Input.GetAxis("Mouse X"); }
        public static float DeltaY { get => Input.GetAxis("Mouse Y"); }
    }
}