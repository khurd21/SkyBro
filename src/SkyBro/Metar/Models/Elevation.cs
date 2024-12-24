using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Elevation
{
    /// <summary>
    /// Elevation of weather recording instrument in feet.
    /// </summary>
    [JsonPropertyName("feet")]
    public required decimal Feet { get; init; }

    /// <summary>
    /// Elevation of weather recording instrument in meters.
    /// </summary>
    [JsonPropertyName("meters")]
    public required decimal Meters { get; init; }
}