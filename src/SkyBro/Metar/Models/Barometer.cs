using System.Text.Json;
using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Barometer
{
    /// <summary>
    /// Barometer in inches of mercury.
    /// </summary>
    [JsonPropertyName("hg")]
    public required decimal Hg { get; init; }

    /// <summary>
    /// Barometer in hectopascals.
    /// </summary>
    [JsonPropertyName("hpa")]
    public required decimal Hpa { get; init; }

    /// <summary>
    /// Barometer in kilopascals.
    /// </summary>
    [JsonPropertyName("kpa")]
    public required decimal Kpa { get; init; }

    /// <summary>
    /// Barometer in millibars.
    /// </summary>
    [JsonPropertyName("mb")]
    public required decimal Mb { get; init; }
}