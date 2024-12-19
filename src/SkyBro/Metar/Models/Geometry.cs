using Alexa.NET.Request.Type;

namespace SkyBro.Metar.Models;

public class Geometry
{
    /// <summary>
    /// GeoJSON array of coordinates [longitude, latitude].
    /// </summary>
    public required IEnumerable<GeolocationCoordinate> Coordinates;

    /// <summary>
    /// GeoJSON object type: "POINT"
    /// </summary>
    public required string Type { get; init; }
}