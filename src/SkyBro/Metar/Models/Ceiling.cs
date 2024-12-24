using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Ceiling
{
    /// <summary>
    /// Ceiling feet above ground level AGL.
    /// </summary>
    [JsonPropertyName("feet")]
    public required decimal Feet { get; init; }

    /// <summary>
    /// Ceiling meters above ground level AGL.
    /// </summary>
    [JsonPropertyName("meters")]
    public required decimal Meters { get; init; }
}