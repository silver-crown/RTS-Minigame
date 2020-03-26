using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeeter;

public class CustomLuaManager : LuaManager
{
    public override Script CreateScript()
    {
        var script = base.CreateScript();
        InGameDebug.Log("OH YEAS");
        UserData.RegisterAssembly(typeof(CustomLuaManager).Assembly);
        script.Globals["FloorBuilder"] = new FloorBuilder();
        return script;
    }
}
