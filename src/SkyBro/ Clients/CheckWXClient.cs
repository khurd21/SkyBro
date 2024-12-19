
using SkyBro.Authentication;
using SkyBro.Metar;
using SkyBro.Metar.Models;
using SkyBro.Services;

namespace SkyBro;

public class CheckWXClient : IMetarEndpoint
{
    private IAuthenticator Authenticator { get; init; }

    private HttpClient Client { get; init; }

    public CheckWXClient(HttpClient client, IAuthenticator authenticator)
    {
        Authenticator = authenticator;
        Client = client;
        Authenticator.AttachToClient(Client);
    }

    public Task<APIResponse<IEnumerable<Metar.Models.Metar>>> GetMetarAsync(IEnumerable<string> icaoCodes)
    {
        throw new NotImplementedException();
    }

    public Task<APIResponse<MetarPosition>> GetNearestMetarAsync(string icaoCode)
    {
        throw new NotImplementedException();
    }

    public Task<APIResponse<IEnumerable<MetarPosition>>> GetMetarWithinRadius(string icaoCode, int radiusMiles)
    {
        throw new NotImplementedException();
    }

    public Task<APIResponse<MetarPosition>> GetNearestMetarAsync(GeolocationCoordinate coordinate)
    {
        throw new NotImplementedException();
    }

    public Task<APIResponse<IEnumerable<MetarPosition>>> GetNearestMetarAsync(GeolocationCoordinate coordinate, int radiusMiles)
    {
        throw new NotImplementedException();
    }
}