using Tyd;
using UnityEngine;
using Yeeter;

public class TileType
{
    public string Name { get; set; }
    public bool Traversable { get; set; }
    public Sprite Sprite { get; set; }

    public static TileType FromTydTable(TydTable table)
    {
        var tileType = new TileType();
        tileType.Name = table.Name;
        foreach (var node in table.Nodes)
        {
            if (node.Name == "traversable")
            {
                tileType.Traversable = bool.Parse((node as TydString).Value);
            }
        }
        var texture = StreamingAssetsDatabase.GetTexture("Tiles." + tileType.Name);
        tileType.Sprite = Sprite.Create(
            texture, new Rect(0, 0, 16, 16), new Vector2(0, 0), 16
        );
        return tileType;
    }
}
