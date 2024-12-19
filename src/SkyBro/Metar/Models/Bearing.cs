using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Bearing
{
    /// <summary>
    /// Bearing from base location (0-360).
    /// </summary>
    [JsonPropertyName("from")]
    public required int From { get; init; }

    /// <summary>
    /// Bearing to base location (0-360).
    /// </summary>
    [JsonPropertyName("to")]
    public required int To { get; init; }

}