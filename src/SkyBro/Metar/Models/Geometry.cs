using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class Geometry
{
    /// <summary>
    /// GeoJSON array of coordinates [longitude, latitude].
    /// </summary>
    [JsonPropertyName("coordinates")]
    [JsonConverter(typeof(GeolocationCoordinateConverter))]
    public required GeolocationCoordinate Coordinates { get; init; }

    /// <summary>
    /// GeoJSON object type: "POINT"
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }
}