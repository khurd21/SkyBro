using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class MetarStationData
{
    [JsonPropertyName("barometer")]
    public Barometer? Barometer { get; init; }

    [JsonPropertyName("ceiling")]
    public Ceiling? Ceiling { get; init; }

    [JsonPropertyName("clouds")]
    public IEnumerable<Cloud>? Clouds { get; init; }

    [JsonPropertyName("conditions")]
    public IEnumerable<Condition>? Conditions { get; init; }

    [JsonPropertyName("dewpoint")]
    public Dewpoint? Dewpoint { get; init; }

    [JsonPropertyName("elevation")]
    public Elevation? Elevation { get; init; }

    /// <summary>
    /// VFR, MVFR, IFR, or LIFR.
    /// </summary>
    [JsonPropertyName("flight_category")]
    public string? FlightCategory { get; init; }

    [JsonPropertyName("humidity")]
    public Humidity? Humidity { get; init; }

    /// <summary>
    /// ICAO airport code or station indicator.
    /// </summary>
    [JsonPropertyName("icao")]
    public required string Icao { get; init; }

    /// <summary>
    /// METAR observed UTC timestamp in ISO format.
    /// </summary>
    [JsonPropertyName("observed")]
    public required string Observed { get; init; }

    [JsonPropertyName("snow")]
    public Snow? Snow { get; init; }

    [JsonPropertyName("station")]
    public required Station Station { get; init; }

    [JsonPropertyName("temperature")]
    public Temperature? Temperature { get; init; }

    [JsonPropertyName("rain")]
    public Rain? Rain { get; init; }

    /// <summary>
    /// Raw METAR text string.
    /// </summary>
    [JsonPropertyName("raw_text")]
    public required string RawText { get; init; }

    [JsonPropertyName("visibility")]
    public Visibility? Visibility { get; init; }

    [JsonPropertyName("wind")]
    public Wind? Wind { get; init; }
}