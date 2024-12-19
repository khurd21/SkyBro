namespace SkyBro.Metar.Models;

public class Total
{
    /// <summary>
    /// 24 hour total rainfall in inches.
    /// </summary>
    public int? Inches { get; init; }

    /// <summary>
    /// 24 hour total rainfall in millimeters.
    /// </summary>
    public int? Millimeters { get; init; }
}

public class Rain
{
    /// <summary>
    /// Rainfall in inches.
    /// </summary>
    public required int Inches { get; init; }

    /// <summary>
    /// Rainfall in millimeters.
    /// </summary>
    public required int Millimeters { get; init; }

    public Total? Total { get; init; }
}