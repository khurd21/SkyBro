using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Visibility
{
    /// <summary>
    /// Visibility in miles (String to support values like "1/2 mile").
    /// </summary>
    [JsonPropertyName("miles_text")]
    public required string Miles { get; init; }

    /// <summary>
    /// Visibility in miles.
    /// </summary>
    [JsonPropertyName("miles")]
    public required float MilesFloat { get; init; }

    /// <summary>
    /// Visibility in meters (String to support values like "> 9000").
    /// </summary>
    [JsonPropertyName("meters_text")]
    public required string Meters { get; init; }

    /// <summary>
    /// Visibility in meters.
    /// </summary>
    [JsonPropertyName("meters")]
    public required float MetersFloat { get; init; }
}