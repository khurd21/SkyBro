using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Dewpoint
{
    /// <summary>
    /// Dewpoint in celsius.
    /// </summary>
    [JsonPropertyName("celsius")]
    public required int Celsius { get; init; }

    /// <summary>
    /// Dewpoint in fahrenheit.
    /// </summary>
    [JsonPropertyName("fahrenheit")]
    public required int Fahrenheit { get; init; }
}