using SkyBro.Metar.Models;
using SkyBro.Services;

namespace SkyBro.Metar;

public interface IMetarEndpoint
{
    Task<APIResponse<IEnumerable<MetarResponse>>> GetMetarAsync(IEnumerable<string> icaoCodes);

    Task<APIResponse<MetarResponse>> GetNearestMetarAsync(string icaoCode);

    Task<APIResponse<IEnumerable<MetarResponse>>> GetMetarWithinRadius(string icaoCode, int radiusMiles);

    Task<APIResponse<MetarResponse>> GetNearestMetarAsync(GeolocationCoordinate coordinate);

    Task<APIResponse<IEnumerable<MetarResponse>>> GetNearestMetarAsync(GeolocationCoordinate coordinate, int radiusMiles);
}