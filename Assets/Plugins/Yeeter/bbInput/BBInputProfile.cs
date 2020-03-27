using System.Collections.Generic;
using Tyd;
using Yeeter;

/// <summary>
/// Contains a set of input axes which work together.
/// Used to exclude certain actions from being performed depending on context.
/// </summary>
public class BBInputProfile
{
    /// <summary>
    /// The name of the profile when referenced in code.
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Whether the profile is enabled.
    /// If it is not enabled the axes cannot be 
    /// </summary>
    public bool Enabled { get; set; }
    /// <summary>
    /// Whether the profile is always enabled.
    /// A profile with this property set to true can never be disabled under any circumstances.
    /// </summary>
    public bool AlwaysEnabled { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public List<BBInputAxis> Axes { get; private set; }

    public static BBInputProfile FromTydTable(TydTable table)
    {
        var profile = new BBInputProfile();
        profile.Axes = new List<BBInputAxis>();
        profile.Name = table.Name;
        profile.Enabled = false;
        InGameDebug.Log("Loading new BBInputProfile...");
        foreach (var node in table.Nodes)
        {
            if (node.Name.ToLowerInvariant() == "enabled")
            {
                profile.Enabled = bool.Parse((node as TydString).Value);
            }
            else if (node.Name.ToLowerInvariant() == "alwaysenabled")
            {
                profile.AlwaysEnabled = bool.Parse((node as TydString).Value);
            }
            else if (node.Name.ToLowerInvariant() == "name")
            {
                profile.Name = (node as TydString).Value;
            }
            else if (node.Name.ToLowerInvariant() == "axes")
            {
                foreach (var axisNodes in (node as TydCollection).Nodes)
                {
                    profile.Axes.Add(BBInputAxis.FromTydTable(axisNodes as TydTable));
                }
            }
        }
        profile.Axes.Sort((x, y) => y.Priority - x.Priority);
        InGameDebug.Log("Loaded BBInputProfile: " + profile.Name);
        return profile;
    }
}
