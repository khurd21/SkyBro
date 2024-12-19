namespace SkyBro.Metar.Models;

public class Wind
{
    /// <summary>
    /// Wind direction in degrees.
    /// </summary>
    public required int Degrees { get; init; }

    /// <summary>
    /// Wind speed in kilometers per hour.
    /// </summary>
    public required int SpeedKph { get; init; }

    /// <summary>
    /// Wind speed in knots.
    /// </summary>
    public required int SpeedKts { get; init; }

    /// <summary>
    /// Wind speed in miles per hour.
    /// </summary>
    public required int SpeedMph { get; init; }

    /// <summary>
    /// Wind speed in meters per second.
    /// </summary>
    public required int SpeedMps { get; init; }

    /// <summary>
    /// Wind gust in knots.
    /// </summary>
    public required int GustKts { get; init; }

    /// <summary>
    /// Wind gust in kilometers per hour.
    /// </summary>
    public required int GustKph { get; init; }

    /// <summary>
    /// Wind gust in miles per hour.
    /// </summary>
    public required int GustMph { get; init; }

    /// <summary>
    /// Wind gust in meters per second.
    /// </summary>
    public required int GustMps { get; init; }
}