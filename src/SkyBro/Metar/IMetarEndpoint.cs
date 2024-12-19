using SkyBro.Metar.Models;
using SkyBro.Services;

namespace SkyBro.Metar;



public interface IMetarEndpoint
{
    Task<APIResponse<IEnumerable<Models.Metar>>> GetMetarAsync(IEnumerable<string> icaoCodes);

    Task<APIResponse<MetarPosition>> GetNearestMetarAsync(string icaoCode);

    Task<APIResponse<IEnumerable<MetarPosition>>> GetMetarWithinRadius(string icaoCode, int radiusMiles);

    Task<APIResponse<MetarPosition>> GetNearestMetarAsync(GeolocationCoordinate coordinate);

    Task<APIResponse<IEnumerable<MetarPosition>>> GetNearestMetarAsync(GeolocationCoordinate coordinate, int radiusMiles);
}