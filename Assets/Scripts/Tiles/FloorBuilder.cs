using MoonSharp.Interpreter;
using System.Collections.Generic;
using Tyd;
using UnityEngine;
using UnityEngine.Scripting;
using Yeeter;

[Preserve, MoonSharpUserData]
public class FloorBuilder
{
    private static Camera _mainCamera;
    private static Dictionary<string, TileType> _tileTypes;

    public static void LoadTileTypes()
    {
        _tileTypes = new Dictionary<string, TileType>();
        var tileNodes = StreamingAssetsDatabase.GetDef("TileTypes") as TydTable;
        foreach (var node in tileNodes.Nodes)
        {
            var tileType = TileType.FromTydTable(node as TydTable);
            _tileTypes.Add(node.Name, tileType);
        }
    }

    public static void SetTileTypeToPlaceOnMouseClick(string typeName)
    {
        var type = GetTileTypeWithName(typeName);
    }

    public static TileType GetTileTypeWithName(string name)
    {
        return _tileTypes[name];
    }

    public static void PlaceTile(string typeName, int x, int y)
    {
        Floor.CurrentFloor.PlaceTile(_tileTypes[typeName], x, y);
    }

    public void PositionCameraAtEntry()
    {
        if (Floor.CurrentFloor.Entry == null)
        {
            InGameDebug.Log(
                "<color=red>" + GetType().Name + ".PositionCameraAtEntry(): Current floor has no entry.</color>");
        }
        if (_mainCamera == null) _mainCamera = Camera.main;
        _mainCamera.transform.position =
            Floor.CurrentFloor.Entry.transform.position + Vector3.forward * _mainCamera.transform.position.z;
    }

    public void ClearFloor()
    {
        Floor.CurrentFloor.Clear();
    }
}
