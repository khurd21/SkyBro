using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Wind
{
    /// <summary>
    /// Wind direction in degrees.
    /// </summary>
    [JsonPropertyName("degrees")]
    public required int Degrees { get; init; }

    /// <summary>
    /// Wind speed in kilometers per hour.
    /// </summary>
    [JsonPropertyName("speed_kph")]
    public required int SpeedKph { get; init; }

    /// <summary>
    /// Wind speed in knots.
    /// </summary>
    [JsonPropertyName("speed_kts")]
    public required int SpeedKts { get; init; }

    /// <summary>
    /// Wind speed in miles per hour.
    /// </summary>
    [JsonPropertyName("speed_mph")]
    public required int SpeedMph { get; init; }

    /// <summary>
    /// Wind speed in meters per second.
    /// </summary>
    [JsonPropertyName("speed_mps")]
    public required int SpeedMps { get; init; }

    /// <summary>
    /// Wind gust in knots.
    /// </summary>
    [JsonPropertyName("gust_kts")]
    public int? GustKts { get; init; }

    /// <summary>
    /// Wind gust in kilometers per hour.
    /// </summary>
    [JsonPropertyName("gust_kph")]
    public int? GustKph { get; init; }

    /// <summary>
    /// Wind gust in miles per hour.
    /// </summary>
    [JsonPropertyName("gust_mph")]
    public int? GustMph { get; init; }

    /// <summary>
    /// Wind gust in meters per second.
    /// </summary>
    [JsonPropertyName("gust_mps")]
    public int? GustMps { get; init; }
}