using System.Text.Json.Serialization;

namespace SkyBro.Metar.Models;

public class MetarResponse
{
    [JsonPropertyName("data")]
    public required IEnumerable<MetarStationData> Data;

}