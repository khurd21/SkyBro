using Ninject.Activation;

namespace WeatherObservations.Dependencies.Http;

public class HttpClientProvider : Provider<HttpClient>
{
    private HttpClient Client { get; init; }

    public HttpClientProvider()
    {
        this.Client = new();

        this.Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows " +
        "NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
        "Chrome/86.0.4240.198 Edg/86.0.622.69");
        this.Client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        this.Client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
        this.Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        this.Client.DefaultRequestHeaders.Add("DNT", "1");
        this.Client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        this.Client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
    }

    protected override HttpClient CreateInstance(IContext context)
    {
        return this.Client;
    }
}