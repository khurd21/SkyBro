namespace SkyBro.Metar.Models;

public class Temperature
{
    /// <summary>
    /// Temperature in celsius.
    /// </summary>
    public required int Celsius { get; init; }

    /// <summary>
    /// Temperature in fahrenheit.
    /// </summary>
    public required int Fahrenheit { get; init; }
}