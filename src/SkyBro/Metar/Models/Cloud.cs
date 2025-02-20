using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Cloud
{
    /// <summary>
    /// Base feet above ground level AGL.
    /// </summary>
    [JsonPropertyName("base_feet_agl")]
    public int? BaseFeetAGL { get; init; }

    /// <summary>
    /// Base meters above ground level AGL.
    /// </summary>
    [JsonPropertyName("base_meters_agl")]
    public int? BaseMetersAGL { get; init; }

    /// <summary>
    /// Cloud abbreviation code
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }

    /// <summary>
    /// Cloud text description.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }

    /// <summary>
    /// Feet above ground level AGL.
    /// </summary>
    [JsonPropertyName("feet")]
    public int? Feet { get; init; }

    /// <summary>
    /// Meters above ground level AGL.
    /// </summary>
    [JsonPropertyName("meters")]
    public int? Meters { get; init; }
}