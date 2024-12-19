namespace SkyBro.Metar.Models;

public class Visibility
{
    /// <summary>
    /// Visibility in miles (String to support values like "1/2 mile").
    /// </summary>
    public required string Miles { get; init; }

    /// <summary>
    /// Visibility in miles.
    /// </summary>
    public required float MilesFloat { get; init; }

    /// <summary>
    /// Visibility in meters (String to support values like "> 9000").
    /// </summary>
    public required string Meters { get; init; }

    /// <summary>
    /// Visibility in meters.
    /// </summary>
    public required float MetersFloat { get; init; }
}