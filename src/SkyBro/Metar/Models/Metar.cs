namespace SkyBro.Metar.Models;

public class Metar
{
    public Barometer? Barometer { get; init; }

    public Ceiling? Ceiling { get; init; }

    public IEnumerable<Cloud>? Clouds { get; init; }

    public IEnumerable<Condition>? Conditions { get; init; }

    public Dewpoint? Dewpoint { get; init; }

    public Elevation? Elevation { get; init; }

    /// <summary>
    /// VFR, MVFR, IFR, or LIFR.
    /// </summary>
    public string? FlightCategory { get; init; }

    public Humidity? Humidity { get; init; }

    /// <summary>
    /// ICAO airport code or station indicator.
    /// </summary>
    public required string Icao { get; init; }

    /// <summary>
    /// METAR observed UTC timestamp in ISO format.
    /// </summary>
    public required string Observed { get; init; }

    public Snow? Snow { get; init; }

    public required Station Station { get; init; }

    public Temperature? Temperature { get; init; }

    public Rain? Rain { get; init; }

    /// <summary>
    /// Raw METAR text string.
    /// </summary>
    public required string RawText { get; init; }

    public Visibility? Visibility { get; init; }

    public Wind? Wind { get; init; }
}