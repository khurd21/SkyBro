using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Temperature
{
    /// <summary>
    /// Temperature in celsius.
    /// </summary>
    [JsonPropertyName("celsius")]
    public required int Celsius { get; init; }

    /// <summary>
    /// Temperature in fahrenheit.
    /// </summary>
    [JsonPropertyName("fahrenheit")]
    public required int Fahrenheit { get; init; }
}