using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Humidity
{
    /// <summary>
    /// Humidity percentage.
    /// </summary>
    [JsonPropertyName("percent")]
    public required int Percent { get; init; }
}