using SkyBro.Metar.Models;
using SkyBro.Services;

namespace SkyBro.Metar;

public interface IMetarEndpoint
{
    Task<APIResponse<IEnumerable<MetarStationData>>> GetMetarAsync(IEnumerable<string> icaoCodes);

    Task<APIResponse<MetarStationData>> GetNearestMetarAsync(string icaoCode);

    Task<APIResponse<IEnumerable<MetarStationData>>> GetMetarWithinRadiusAsync(string icaoCode, int radiusMiles);

    Task<APIResponse<MetarStationData>> GetNearestMetarAsync(GeolocationCoordinate coordinate);

    Task<APIResponse<IEnumerable<MetarStationData>>> GetNearestMetarAsync(GeolocationCoordinate coordinate, int radiusMiles);
}