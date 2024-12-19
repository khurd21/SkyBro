using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class GeolocationCoordinate
{
    [JsonPropertyName("latitude")]
    public required decimal Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public required decimal Longitude { get; init; }
}