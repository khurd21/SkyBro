using System.Text.Json.Serialization;

using Amazon.Runtime.Internal.Auth;

namespace SkyBro.Metar.Models;

public class MetarResponse
{
    [JsonPropertyName("data")]
    public required IEnumerable<MetarStationData> Data { get; init; }

}