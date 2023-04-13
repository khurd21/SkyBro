using Ninject.Activation;
using WeatherObservations.Dependencies.Http;
using Xunit;

namespace WeatherObservations.Tests.Dependencies.Http;


internal class TestableHttpClientProvider : HttpClientProvider
{
    public TestableHttpClientProvider() : base()
    {
    }

    public new HttpClient CreateInstance(IContext context)
    {
        return base.CreateInstance(context);
    }
}

public class HttpClientProviderTests
{
    private TestableHttpClientProvider ClientProvider { get; set; }

    public HttpClientProviderTests()
    {
        this.ClientProvider = new();
    }

    [Fact]
    public void TestCreateInstance()
    {
        var client = this.ClientProvider.CreateInstance(null!);

        Assert.NotNull(client);
        Assert.IsType<HttpClient>(client);
    }
}