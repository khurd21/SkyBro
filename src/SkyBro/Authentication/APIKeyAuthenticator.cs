
namespace SkyBro.Authentication;

public class APIKeyAuthenticator : IAuthenticator
{
    private string Key { get; init; }

    private string HeaderName { get; init; }

    public APIKeyAuthenticator(string key, string headerName)
    {
        Key = key;
        HeaderName = headerName;
    }

    public void AttachToClient(HttpClient client)
    {
        if (!client.DefaultRequestHeaders.Contains(HeaderName))
        {
            client.DefaultRequestHeaders.Add(HeaderName, Key);
        }
    }
}