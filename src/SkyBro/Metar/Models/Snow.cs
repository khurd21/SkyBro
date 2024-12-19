namespace SkyBro.Metar.Models;

public class Snow
{
    /// <summary>
    /// Snowfall in inches.
    /// </summary>
    public required int Inches { get; init; }

    /// <summary>
    /// Snowfall in millimeters.
    /// </summary>
    public required int Millimeters { get; init; }
}