using System.Text.Json.Serialization;

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
    [JsonPropertyName("geometry")]
    public required Geometry Geometry { get; init; }

    /// <summary>
    /// Station location.
    /// </summary>
    [JsonPropertyName("location")]
    public required string Location { get; init; }

    /// <summary>
    /// Station name.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Type - "Airport", "Heliport", "Seaplane Base", etc.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}