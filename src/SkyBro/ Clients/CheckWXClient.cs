using System.Net;
using System.Text;
using System.Text.Json;

using Amazon.Runtime.SharedInterfaces;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;

using SkyBro.Authentication;
using SkyBro.Metar;
using SkyBro.Metar.Models;
using SkyBro.Services;

namespace SkyBro;

public class CheckWXClient : IMetarEndpoint
{
    private IAuthenticator Authenticator { get; init; }

    private HttpClient Client { get; init; }

    private static string BaseUrl { get; } = "https://api.checkwx.com";

    public CheckWXClient(HttpClient client, IAuthenticator authenticator)
    {
        Authenticator = authenticator;
        Client = client;
        Authenticator.AttachToClient(Client);
    }

    public async Task<APIResponse<IEnumerable<MetarStationData>>> GetMetarAsync(IEnumerable<string> icaoCodes)
    {
        if (icaoCodes.Count() == 0)
        {
            return new APIResponse<IEnumerable<MetarStationData>> { Success = false, Message = "No ICAO codes provided." };
        }
        if (icaoCodes.Count() > 20)
        {
            return new APIResponse<IEnumerable<MetarStationData>> { Success = false, Message = "Too many ICAO codes provided. Maximum is 20." };
        }
        var codes = string.Join(",", icaoCodes);
        string uri = $"{BaseUrl}/metar/{codes}/decoded";
        var response = await Client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            return new APIResponse<IEnumerable<MetarStationData>> { Message = $"Error fetching data from {uri}. Received status code: {response.StatusCode}.", Success = false };
        }
        try
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MetarResponse>(responseContent);
            return new APIResponse<IEnumerable<MetarStationData>> { Success = true, Data = result!.Data, Message = string.Empty };
        }
        catch (Exception)
        {
            return new APIResponse<IEnumerable<MetarStationData>> { Success = false, Message = $"Error trying to deserialize response: {await response.Content.ReadAsStringAsync()}" };
        }
    }

    public async Task<APIResponse<MetarStationData>> GetNearestMetarAsync(string icaoCode)
    {
        string uri = $"{BaseUrl}/metar/{icaoCode}/nearest/decoded";
        var response = await Client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            return new APIResponse<MetarStationData> { Message = $"Error fetching data from {uri}. Received status code: {response.StatusCode}.", Success = false };
        }
        try
        {
            var result = JsonSerializer.Deserialize<MetarResponse>(await response.Content.ReadAsStringAsync());
            return new APIResponse<MetarStationData> { Success = true, Data = result!.Data.First(), Message = string.Empty };
        }
        catch (Exception)
        {
            return new APIResponse<MetarStationData> { Success = false, Message = $"Error trying to deserialize response: {await response.Content.ReadAsStringAsync()}" };
        }
    }

    public async Task<APIResponse<IEnumerable<MetarStationData>>> GetMetarWithinRadiusAsync(string icaoCode, int radiusMiles)
    {
        if (radiusMiles < 1 || radiusMiles > 250)
        {
            return new APIResponse<IEnumerable<MetarStationData>> { Success = false, Message = "Radius must be between 1 and 250 miles." };
        }
        string uri = $"{BaseUrl}/metar/{icaoCode}/radius/{radiusMiles}/decoded";
        var response = await Client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            return new APIResponse<IEnumerable<MetarStationData>>
            {
                Message = $"Error fetching data from {uri}. Received status code: {response.StatusCode}.",
                Success = false
            };
        }
        try
        {
            var result = JsonSerializer.Deserialize<MetarResponse>(await response.Content.ReadAsStringAsync());
            return new APIResponse<IEnumerable<MetarStationData>> { Success = true, Data = result!.Data, Message = string.Empty };
        }
        catch (Exception)
        {
            return new APIResponse<IEnumerable<MetarStationData>>
            {
                Success = false,
                Message = $"Error trying to deserialize response: {await response.Content.ReadAsStringAsync()}"
            };
        }
    }

    public async Task<APIResponse<MetarStationData>> GetNearestMetarAsync(GeolocationCoordinate coordinate)
    {
        var uri = $"{BaseUrl}/metar/lat/{coordinate.Latitude}/lon/{coordinate.Longitude}/decoded";
        var response = await Client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            return new APIResponse<MetarStationData>
            {
                Message = $"Error fetching data from {uri}. Received status code: {response.StatusCode}.",
                Success = false
            };
        }
        try
        {
            var result = JsonSerializer.Deserialize<MetarResponse>(await response.Content.ReadAsStringAsync());
            return new APIResponse<MetarStationData> { Success = true, Data = result!.Data.First(), Message = string.Empty };
        }
        catch (Exception)
        {
            return new APIResponse<MetarStationData>
            {
                Success = false,
                Message = $"Error trying to deserialize response: {await response.Content.ReadAsStringAsync()}"
            };
        }
    }

    public async Task<APIResponse<IEnumerable<MetarStationData>>> GetNearestMetarAsync(GeolocationCoordinate coordinate, int radiusMiles)
    {
        if (radiusMiles < 1 || radiusMiles > 250)
        {
            return new APIResponse<IEnumerable<MetarStationData>> { Success = false, Message = "Radius must be between 1 and 250 miles." };
        }
        var uri = $"{BaseUrl}/metar/lat/{coordinate.Latitude}/lon/{coordinate.Longitude}/radius/{radiusMiles}/decoded";
        var response = await Client.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            return new APIResponse<IEnumerable<MetarStationData>>
            {
                Message = $"Error fetching data from {uri}. Received status code: {response.StatusCode}.",
                Success = false
            };
        }
        try
        {
            var result = JsonSerializer.Deserialize<MetarResponse>(await response.Content.ReadAsStringAsync());
            return new APIResponse<IEnumerable<MetarStationData>> { Success = true, Data = result!.Data, Message = string.Empty };
        }
        catch (Exception)
        {
            return new APIResponse<IEnumerable<MetarStationData>>
            {
                Success = false,
                Message = $"Error trying to deserialize response: {await response.Content.ReadAsStringAsync()}"
            };
        }
    }
}