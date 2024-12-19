using System.Text.Json.Serialization;

using Newtonsoft.Json;

namespace SkyBro.Metar.Models;

public class Snow
{
    /// <summary>
    /// Snowfall in inches.
    /// </summary>
    [JsonPropertyName("inches")]
    public required decimal Inches { get; init; }

    /// <summary>
    /// Snowfall in millimeters.
    /// </summary>
    [JsonPropertyName("millimeters")]
    public required decimal Millimeters { get; init; }
}