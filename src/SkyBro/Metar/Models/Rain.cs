using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Rain
{
    /// <summary>
    /// Rainfall in inches.
    /// </summary>
    [JsonPropertyName("inches")]
    public required decimal Inches { get; init; }

    /// <summary>
    /// Rainfall in millimeters.
    /// </summary>
    [JsonPropertyName("millimeters")]
    public required decimal Millimeters { get; init; }
}