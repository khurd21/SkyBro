using Xunit;

namespace SkyBro.Authentication.Tests;

public class APIKeyAuthenticatorTests
{
    [Fact]
    public void AttachToClient_AddsHeader_WhenHeaderIsNotPresent()
    {
        // Arrange
        const string key = "test-key";
        const string headerName = "X-API-Key";
        var authenticator = new APIKeyAuthenticator(key, headerName);
        var client = new HttpClient();

        // Act
        authenticator.AttachToClient(client);

        // Assert
        Assert.True(client.DefaultRequestHeaders.Contains(headerName));
        Assert.Equal(key, client.DefaultRequestHeaders.GetValues(headerName).Single());
    }

    [Fact]
    public void AttachToClient_DoesNotAddHeader_WhenHeaderIsAlreadyPresent()
    {
        // Arrange
        const string key = "test-key";
        const string headerName = "X-API-Key";
        var authenticator = new APIKeyAuthenticator(key, headerName);
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add(headerName, "existing-value");

        // Act
        authenticator.AttachToClient(client);

        // Assert
        Assert.True(client.DefaultRequestHeaders.Contains(headerName));
        Assert.Equal("existing-value", client.DefaultRequestHeaders.GetValues(headerName).Single());
    }
}