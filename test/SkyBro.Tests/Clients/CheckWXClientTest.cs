using System.Net;
using System.Text.Json;

using Moq;

using RichardSzalay.MockHttp;

using SkyBro.Authentication;
using SkyBro.Metar.Models;

using Xunit;

namespace SkyBro.Tests;
public class CheckWXClientTests
{
    private Mock<IAuthenticator> MockAuthenticator { get; init; }

    private MockHttpMessageHandler MockHttp { get; init; }

    private HttpClient HttpClientInstance { get; init; }

    private CheckWXClient ClientUnderTest { get; init; }

    public CheckWXClientTests()
    {
        MockAuthenticator = new Mock<IAuthenticator>();
        MockHttp = new MockHttpMessageHandler();
        HttpClientInstance = MockHttp.ToHttpClient();
        ClientUnderTest = new(HttpClientInstance, MockAuthenticator.Object);
        MockAuthenticator.Verify(a => a.AttachToClient(HttpClientInstance), Times.Once);
    }

    [Fact]
    public async Task GetMetarAsync_WhenNoIcaoCodesProvided_ReturnsFailureResponse()
    {
        // Act
        var response = await ClientUnderTest.GetMetarAsync(new string[] { });

        // Assert
        Assert.False(response.Success);
        Assert.Equal("No ICAO codes provided.", response.Message);
        Assert.Null(response.Data);
    }

    [Fact]
    public async Task GetMetarAsync_WhenTooManyIcaoCodesProvided_ReturnsFailureResponse()
    {
        // Act
        var response = await ClientUnderTest.GetMetarAsync(new[]
        {
            "KJFK", "KLAX", "KORD", "KATL", "KDFW", "KDEN",
            "KSFO", "KSEA", "KMIA", "KMCO", "KPHX", "KDTW",
            "KCLT", "KPHL", "KIAH", "KSLC", "KSTL", "KTPA",
            "KMSP", "KPDX", "KDAL"
        });

        // Assert
        Assert.False(response.Success);
        Assert.Equal("Too many ICAO codes provided. Maximum is 20.", response.Message);
        Assert.Null(response.Data);
    }

    [Theory]
    [InlineData("KJFK", HttpStatusCode.InternalServerError)]
    [InlineData("KSHN", HttpStatusCode.Forbidden)]
    [InlineData("FooBar", HttpStatusCode.FailedDependency)]
    public async Task GetMetarAsync_WhenResponseIsNotSuccessful_ReturnsFailureResponse(string icaoCode, HttpStatusCode statusCode)
    {
        // Arrange
        MockHttp.When("*").Respond(statusCode);

        // Act
        var response = await ClientUnderTest.GetMetarAsync(new[] { icaoCode });

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Data);
        Assert.Equal($"Error fetching data from https://api.checkwx.com/metar/{icaoCode}/decoded. Received status code: {statusCode}.", response.Message);
    }

    [Theory]
    [InlineData("{ \"FooBar\": 42 }")]
    [InlineData("invalid json")]
    public async Task GetMetarAsync_WhenResponseIsSuccessfulButDeserializationFails_ReturnsFailureResponse(string payload)
    {
        // Arrange
        MockHttp.When("*").Respond("application/json", payload);

        // Act
        var response = await ClientUnderTest.GetMetarAsync(new[] { "KSHN" });

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Data);
        Assert.Equal($"Error trying to deserialize response: {payload}", response.Message);
    }

    [Theory]
    [InlineData(new[] { "KAVL", "KJFK" }, "GetMetarAsync-KAVL-KJFK.json")]
    [InlineData(new[] { "KSHN" }, "GetMetarAsync-KSHN.json")]
    [InlineData(new[] { "KRDU" }, "GetMetarAsync-KRDU.json")]
    public async Task GetMetarAsync_WhenResponseIsSuccessful_ReturnsSuccessResponse(IEnumerable<string> icaoCodes, string fileName)
    {
        // Arrange
        string path = $"Clients/TestData/{fileName}";
        var content = File.ReadAllText(path);
        var expectedResponse = JsonSerializer.Deserialize<MetarResponse>(content);
        MockHttp.When("*").Respond("application/json", content);

        // Act
        var actualResponse = await ClientUnderTest.GetMetarAsync(icaoCodes);

        // Assert
        Assert.True(actualResponse.Success);
        Assert.Equal(expected: expectedResponse?.Data.Count(), actual: actualResponse?.Data?.Count());
        Assert.Equal(expected: string.Empty, actual: actualResponse?.Message);
        for (int i = 0; i < actualResponse?.Data?.Count(); ++i)
        {
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).Station.Name, actual: actualResponse.Data.ElementAt(i).Station.Name);
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).FlightCategory, actual: actualResponse.Data.ElementAt(i).FlightCategory);
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).Temperature?.Fahrenheit, actual: actualResponse.Data.ElementAt(i).Temperature?.Fahrenheit);
        }
    }

    [Theory]
    [InlineData("KSHN", HttpStatusCode.Forbidden)]
    [InlineData("KJFK", HttpStatusCode.InternalServerError)]
    public async Task GetNearestMetarAsync_WhenResponseIsNotSuccessful_ReturnsErrorResponse(string icaoCode, HttpStatusCode statusCode)
    {
        // Arrange
        MockHttp.When("*").Respond(statusCode);

        // Act
        var actualResponse = await ClientUnderTest.GetNearestMetarAsync(icaoCode);

        // Assert
        Assert.False(actualResponse.Success);
        Assert.Null(actualResponse.Data);
        Assert.Equal(expected: $"Error fetching data from https://api.checkwx.com/metar/{icaoCode}/nearest/decoded. Received status code: {statusCode}.", actual: actualResponse.Message);
    }

    [Theory]
    [InlineData("invalid json")]
    [InlineData(" { \"FooBar\": 42 }")]
    [InlineData("[1, 2, 3]")]
    public async Task GetNearestMetarAsync_WhenResponseIsSuccessfulButDeserializationFails_ReturnsFailureResponse(string payload)
    {
        // Arrange
        MockHttp.When("*").Respond("application/json", payload);

        // Act
        var response = await ClientUnderTest.GetNearestMetarAsync("KJFK");

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Data);
        Assert.Equal($"Error trying to deserialize response: {payload}", response.Message);
    }

    [Theory]
    [InlineData("KRDU", "GetMetarAsync-KRDU.json")]
    [InlineData("KSHN", "GetMetarAsync-KSHN.json")]
    public async Task GetNearestMetarAsync_WhenResponseIsSuccessful_ReturnsValidObject(string icaoCode, string fileName)
    {
        // Arrange
        string path = $"Clients/TestData/{fileName}";
        var content = File.ReadAllText(path);
        var expectedResponse = JsonSerializer.Deserialize<MetarResponse>(content);
        MockHttp.When("*").Respond("application/json", content);

        // Act
        var actualResponse = await ClientUnderTest.GetNearestMetarAsync(icaoCode);

        // Assert
        Assert.True(actualResponse.Success);
        Assert.Equal(expected: string.Empty, actual: actualResponse?.Message);
        Assert.Equal(expected: expectedResponse?.Data.ElementAt(0).Station.Name, actual: actualResponse?.Data?.Station.Name);
        Assert.Equal(expected: expectedResponse?.Data.ElementAt(0).FlightCategory, actual: actualResponse?.Data?.FlightCategory);
        Assert.Equal(expected: expectedResponse?.Data.ElementAt(0).Temperature?.Fahrenheit, actual: actualResponse?.Data?.Temperature?.Fahrenheit);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(Int32.MaxValue)]
    [InlineData(Int32.MinValue)]
    [InlineData(251)]
    public async Task GetMetarWithinRadiusAsync_WhenRadiusIsNotInValidRange_ReturnsErrorResponse(int radius)
    {
        // Act
        var actualResponse = await ClientUnderTest.GetMetarWithinRadiusAsync("KSHN", radius);

        // Assert
        Assert.False(actualResponse.Success);
        Assert.Null(actualResponse.Data);
        Assert.Equal(expected: "Radius must be between 1 and 250 miles.", actual: actualResponse.Message);
    }

    [Theory]
    [InlineData("KSHN", 1, HttpStatusCode.InternalServerError)]
    [InlineData("KRDU", 25, HttpStatusCode.Forbidden)]
    public async Task GetMetarWithinRadiusAsync_WhenResponseIsNotSuccessful_ReturnsErrorResponse(string icaoCode, int radius, HttpStatusCode statusCode)
    {
        // Arrange
        MockHttp.When("*").Respond(statusCode);

        // Act
        var actualResponse = await ClientUnderTest.GetMetarWithinRadiusAsync(icaoCode, radius);

        // Assert
        Assert.Null(actualResponse.Data);
        Assert.False(actualResponse.Success);
        Assert.Equal(
            expected: $"Error fetching data from https://api.checkwx.com/metar/{icaoCode}/radius/{radius}/decoded. Received status code: {statusCode}.",
            actual: actualResponse.Message);
    }

    [Theory]
    [InlineData("invalid json")]
    [InlineData("[1,2,3,4]")]
    [InlineData("{ \"Foo\": \"Bar\" }")]
    public async Task GetMetarWithinRadiusAsync_WhenResponseIsSuccessfulButDeserializationFails_ReturnsErrorResponse(string payload)
    {
        // Arrange
        MockHttp.When("*").Respond("application/json", payload);

        // Act
        var actualResponse = await ClientUnderTest.GetMetarWithinRadiusAsync("KSHN", 1);

        // Assert
        Assert.Null(actualResponse.Data);
        Assert.False(actualResponse.Success);
        Assert.Equal(expected: $"Error trying to deserialize response: {payload}", actual: actualResponse.Message);
    }

    [Theory]
    [InlineData("KRDU", 2, "GetMetarAsync-KRDU.json")]
    [InlineData("KSHN", 15, "GetMetarAsync-KSHN.json")]
    [InlineData("KAVL", 25, "GetMetarAsync-KAVL-KJFK.json")]
    public async Task GetMetarWithinRadiusAsync_WhenResponseIsSuccessful_ReturnsValidResponse(string icaoCode, int radius, string fileName)
    {
        // Arrange
        string path = $"Clients/TestData/{fileName}";
        var content = File.ReadAllText(path);
        var expectedResponse = JsonSerializer.Deserialize<MetarResponse>(content);
        MockHttp.When("*").Respond("application/json", content);

        // Act
        var actualResponse = await ClientUnderTest.GetMetarWithinRadiusAsync(icaoCode, radius);

        // Assert
        Assert.NotNull(actualResponse.Data);
        Assert.True(actualResponse.Success);
        Assert.True(string.IsNullOrEmpty(actualResponse.Message));
        Assert.Equal(expected: expectedResponse?.Data.Count(), actual: actualResponse.Data.Count());
        for (int i = 0; i < actualResponse?.Data.Count(); ++i)
        {
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).Station.Name, actual: actualResponse?.Data.ElementAt(i).Station.Name);
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).FlightCategory, actual: actualResponse?.Data.ElementAt(i).FlightCategory);
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).Temperature?.Fahrenheit, actual: actualResponse?.Data.ElementAt(i).Temperature?.Fahrenheit);
        }
    }

    [Theory]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]

    public async Task GetNearestMetarAsyncCoordinates_WhenResponseIsNotSuccessful_ReturnsErrorResponse(HttpStatusCode statusCode)
    {
        // Arrange
        MockHttp.When("*").Respond(statusCode);

        // Act
        var actualResponse = await ClientUnderTest.GetNearestMetarAsync(new GeolocationCoordinate { Latitude = 0.0M, Longitude = 0.0M });

        // Assert
        Assert.False(actualResponse.Success);
        Assert.Null(actualResponse.Data);
        Assert.Equal(expected: $"Error fetching data from https://api.checkwx.com/metar/lat/0.0/lon/0.0/decoded. Received status code: {statusCode}.", actual: actualResponse?.Message);
    }

    [Theory]
    [InlineData("invalid json")]
    [InlineData(" { \"FooBar\": 42 }")]
    [InlineData("[1, 2, 3]")]
    public async Task GetNearestMetarAsyncCoordinates_WhenResponseIsSuccessfulButDeserializationFails_ReturnsFailureResponse(string payload)
    {
        // Arrange
        MockHttp.When("*").Respond("application/json", payload);

        // Act
        var response = await ClientUnderTest.GetNearestMetarAsync(new GeolocationCoordinate { Latitude = 0.0M, Longitude = 0.0M });

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Data);
        Assert.Equal($"Error trying to deserialize response: {payload}", response.Message);
    }

    [Theory]
    [InlineData("GetMetarAsync-KSHN.json")]
    [InlineData("GetMetarAsync-KRDU.json")]
    public async Task GetNearestMetarAsyncCoordinates_WhenResponseIsSuccessful_ReturnsSuccessResponse(string fileName)
    {
        // Arrange
        string path = $"Clients/TestData/{fileName}";
        var coordinate = new GeolocationCoordinate { Latitude = 0.0M, Longitude = 0.0M };
        var content = File.ReadAllText(path);
        var expectedResponse = JsonSerializer.Deserialize<MetarResponse>(content);
        MockHttp.When("*").Respond("application/json", content);

        // Act
        var actualResponse = await ClientUnderTest.GetNearestMetarAsync(coordinate);

        // Assert
        Assert.True(actualResponse.Success);
        Assert.True(string.IsNullOrEmpty(actualResponse.Message));
        Assert.Equal(expected: expectedResponse?.Data.ElementAt(0).Station.Name, actual: actualResponse?.Data?.Station.Name);
        Assert.Equal(expected: expectedResponse?.Data.ElementAt(0).FlightCategory, actual: actualResponse?.Data?.FlightCategory);
        Assert.Equal(expected: expectedResponse?.Data.ElementAt(0).Temperature?.Fahrenheit, actual: actualResponse?.Data?.Temperature?.Fahrenheit);
    }

    [Theory]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetNearestMetarAsyncRadiusFromCoordinate_WhenResponseIsNotSuccessful_ReturnsErrorResponse(HttpStatusCode statusCode)
    {
        // Arrange
        MockHttp.When("*").Respond(statusCode);

        // Act
        var actualResponse = await ClientUnderTest.GetNearestMetarAsync(new GeolocationCoordinate { Latitude = 0.0M, Longitude = 0.0M }, 1);

        // Assert
        Assert.False(actualResponse.Success);
        Assert.Null(actualResponse.Data);
        Assert.Equal(expected: $"Error fetching data from https://api.checkwx.com/metar/lat/0.0/lon/0.0/radius/1/decoded. Received status code: {statusCode}.", actual: actualResponse?.Message);
    }

    [Theory]
    [InlineData("invalid json")]
    [InlineData(" { \"FooBar\": 42 }")]
    [InlineData("[1, 2, 3]")]
    public async Task GetNearestMetarAsyncRadiusFromCoordinate_WhenResponseIsSuccessfulButDeserializationFails_ReturnsFailureResponse(string payload)
    {
        // Arrange
        MockHttp.When("*").Respond("application/json", payload);

        // Act
        var response = await ClientUnderTest.GetNearestMetarAsync(new GeolocationCoordinate { Latitude = 0.0M, Longitude = 0.0M }, 1);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Data);
        Assert.Equal($"Error trying to deserialize response: {payload}", response.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(Int32.MaxValue)]
    [InlineData(Int32.MinValue)]
    [InlineData(251)]
    public async Task GetNearestMetarAsyncRadiusFromCoordinate_WhenRadiusIsNotInValidRange_ReturnsErrorResponse(int radius)
    {
        // Act
        var actualResponse = await ClientUnderTest.GetNearestMetarAsync(new GeolocationCoordinate { Longitude = 0.0M, Latitude = 0.0M }, radius);

        // Assert
        Assert.False(actualResponse.Success);
        Assert.Null(actualResponse.Data);
        Assert.Equal(expected: "Radius must be between 1 and 250 miles.", actual: actualResponse.Message);
    }

    [Theory]
    [InlineData("GetMetarAsync-KSHN.json")]
    [InlineData("GetMetarAsync-KRDU.json")]
    public async Task GetNearestMetarAsyncRadiusFromCoordinate_WhenResponseIsSuccessful_ReturnsSuccessResponse(string fileName)
    {
        // Arrange
        string path = $"Clients/TestData/{fileName}";
        var coordinate = new GeolocationCoordinate { Latitude = 0.0M, Longitude = 0.0M };
        var content = File.ReadAllText(path);
        var expectedResponse = JsonSerializer.Deserialize<MetarResponse>(content);
        MockHttp.When("*").Respond("application/json", content);

        // Act
        var actualResponse = await ClientUnderTest.GetNearestMetarAsync(coordinate, 1);

        // Assert
        Assert.True(actualResponse.Success);
        Assert.True(string.IsNullOrEmpty(actualResponse.Message));
        Assert.Equal(expected: expectedResponse?.Data.Count(), actual: actualResponse?.Data?.Count());
        for (int i = 0; i < actualResponse?.Data?.Count(); ++i)
        {
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).Station.Name, actual: actualResponse?.Data.ElementAt(i).Station.Name);
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).FlightCategory, actual: actualResponse?.Data?.ElementAt(i).FlightCategory);
            Assert.Equal(expected: expectedResponse?.Data.ElementAt(i).Temperature?.Fahrenheit, actual: actualResponse?.Data?.ElementAt(i).Temperature?.Fahrenheit);
        }
    }
}