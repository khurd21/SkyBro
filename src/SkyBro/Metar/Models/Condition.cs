using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Condition
{
    /// <summary>
    /// Condition abbreviation code.
    /// </summary>
    [JsonPropertyName("code")]
    public required string Code { get; init; }

    /// <summary>
    /// Condition text description.
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}