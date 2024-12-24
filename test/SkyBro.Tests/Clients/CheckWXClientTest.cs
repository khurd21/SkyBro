using Moq;

using SkyBro.Authentication;
using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests;
public class CheckWXClientTests
{
    private Mock<IAuthenticator> MockAuthenticator { get; init; }
    private HttpClient HttpClient { get; init; }

    public CheckWXClientTests()
    {
        MockAuthenticator = new Mock<IAuthenticator>();
        HttpClient = new HttpClient();
    }

    [Fact]
    public void Constructor_InitializesAuthenticatorAndClient()
    {
        // Act
        var client = new CheckWXClient(HttpClient, MockAuthenticator.Object);

        // Assert
        MockAuthenticator.Verify(a => a.AttachToClient(HttpClient), Times.Once);
        Assert.NotNull(client);
    }

    [Fact]
    public async Task GetMetarAsync_ThrowsNotImplementedException()
    {
        // Arrange
        var client = new CheckWXClient(HttpClient, MockAuthenticator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() =>
            client.GetMetarAsync(new[] { "KJFK", "KLAX" }));
    }

    [Fact]
    public async Task GetNearestMetarAsync_ByIcaoCode_ThrowsNotImplementedException()
    {
        // Arrange
        var client = new CheckWXClient(HttpClient, MockAuthenticator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() =>
            client.GetNearestMetarAsync("KJFK"));
    }

    [Fact]
    public async Task GetMetarWithinRadius_ThrowsNotImplementedException()
    {
        // Arrange
        var client = new CheckWXClient(HttpClient, MockAuthenticator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() =>
            client.GetMetarWithinRadius("KJFK", 50));
    }

    [Fact]
    public async Task GetNearestMetarAsync_ByCoordinate_ThrowsNotImplementedException()
    {
        // Arrange
        var client = new CheckWXClient(HttpClient, MockAuthenticator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() =>
            client.GetNearestMetarAsync(new GeolocationCoordinate { Latitude = 40.0M, Longitude = -74.0M }));
    }

    [Fact]
    public async Task GetNearestMetarAsync_ByCoordinateAndRadius_ThrowsNotImplementedException()
    {
        // Arrange
        var client = new CheckWXClient(HttpClient, MockAuthenticator.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotImplementedException>(() =>
            client.GetNearestMetarAsync(new GeolocationCoordinate { Latitude = 40.0M, Longitude = -74.0M }, 50));
    }
}