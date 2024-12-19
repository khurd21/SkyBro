namespace SkyBro.Metar.Models;

public class MetarPosition : Metar
{
    /// <summary>
    /// Base location ICAO. Only included for ICAO requests.
    /// </summary>
    public string? Base { get; init; }

    public required Bearing Bearing { get; init; }

    /// <summary>
    /// Base location latitude decimals. Only included for Lat/Lon requests.
    /// </summary>
    public float? Latitude { get; init; }

    /// <summary>
    /// Base location longitude decimals. Only included for Lat/Lon requests.
    /// </summary>
    public float? Longitutde { get; init; }

    /// <summary>
    /// Distance from base location in miles.
    /// </summary>
    public required float Miles { get; init; }

    /// <summary>
    /// Distance from base location in meters
    /// </summary>
    public required float Meters { get; init; }
}
