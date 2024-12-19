namespace SkyBro.Metar.Models;

public class Station
{
    /// <summary>
    /// GeoJSON object with two properties:
    /// <list type="bullet">
    /// <item>
    /// <description>GeoJSON array of coordinates [longitude, latitude]</description>
    /// </item>
    /// <item>
    /// <description>GeoJSON object type: "POINT".</description>
    /// </item>
    /// </list>
    /// </summary>
    public required Geometry Geometry { get; init; }

    /// <summary>
    /// Station location.
    /// </summary>
    public required string Location { get; init; }

    /// <summary>
    /// Station name.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Type - "Airport", "Heliport", "Seaplane Base", etc.
    /// </summary>
    public string? Type { get; init; }
}