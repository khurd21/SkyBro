using System.Net;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Moq;
using Moq.Protected;
using WeatherObservations.Data.DynamoDB;
using WeatherObservations.Dependencies.Logger;
using WeatherObservations.Dependencies.WeatherObservations;
using Xunit;

namespace WeatherObservations.Tests.Dependencies.WeatherObservations;

public class SkyConditionObservationsTests
{
    private Mock<ILogger> MockLogger { get; set; }

    private Mock<IDynamoDBContext> MockDynamoDBContext { get; set; }

    private Mock<HttpMessageHandler> MockHttpMessageHandler { get; set; }

    private HttpClient HttpClient { get; set; }

    private SkyConditionObservations SkyConditionObservations { get; set; }

    private const int ExpectedNumberOfWeatherData = 22;

    public SkyConditionObservationsTests()
    {
        this.MockLogger = new();
        this.MockDynamoDBContext = new();
        this.MockHttpMessageHandler = new();
        this.HttpClient = new(this.MockHttpMessageHandler.Object);

        this.SkyConditionObservations = new(
            this.MockLogger.Object,
            this.MockDynamoDBContext.Object,
            this.HttpClient);
    }

    [Theory]
    [InlineData("KSEA", "WA")]
    public void TestGetSkyConditionsAsync_WithNoCache(string stationId, string state)
    {
        HttpResponseMessage response = new()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(File.ReadAllText("MockWebRequest.txt")),
        };

        this.MockDynamoDBContext.Setup(context => context
                .QueryAsync<WeatherData>(
                    It.IsAny<string>(),
                    It.IsAny<DynamoDBOperationConfig>())
                .GetRemainingAsync(It.Ref<CancellationToken>.IsAny))
                .ReturnsAsync(new List<WeatherData>())
                .Verifiable();
        
        this.MockDynamoDBContext.Setup(context => context
            .SaveAsync(It.IsAny<WeatherData>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        this.MockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response)
            .Verifiable();

        this.MockLogger.Setup(logger => logger.Log($"No records found for {stationId} in {state}. Making web request."))
            .Verifiable();

        var result = this.SkyConditionObservations.GetSkyConditionsAsync(stationId, state).Result;

        Assert.NotNull(result);
        Assert.Equal(ExpectedNumberOfWeatherData, result.Count);
        Mock.Verify();
    }

    [Theory]
    [InlineData("KSEA", "WA")]
    public void TestGetSkyConditionsAsync_WithUpToDateCache(string stationId, string state)
    {
        var weatherData = WeatherDataTestCaseData.GetUpToDateWeatherData();
        this.MockDynamoDBContext.Setup(context => context
                .QueryAsync<WeatherData>(
                    It.IsAny<string>(),
                    It.IsAny<DynamoDBOperationConfig>())
                .GetRemainingAsync(It.Ref<CancellationToken>.IsAny))
                .ReturnsAsync(weatherData)
                .Verifiable();

        this.MockLogger.Setup(logger => logger.Log($"Found {weatherData.Count} records for {stationId} in {state}."))
            .Verifiable();

        var result = this.SkyConditionObservations.GetSkyConditionsAsync(stationId, state).Result;

        Assert.NotNull(result);
        Assert.Equal(weatherData.Count, result.Count);
        Mock.Verify();
    }

    [Theory]
    [InlineData("KSEA", "WA")]
    public void TestGetSkyConditionsAsync_WithOutdatedCache(string stationId, string state)
    {
        var weatherData = WeatherDataTestCaseData.GetOutdatedWeatherData();
        this.MockDynamoDBContext.Setup(context => context
                .QueryAsync<WeatherData>(
                    It.IsAny<string>(),
                    It.IsAny<DynamoDBOperationConfig>())
                .GetRemainingAsync(It.Ref<CancellationToken>.IsAny))
                .ReturnsAsync(weatherData)
                .Verifiable();
        
        this.MockLogger.Setup(logger => logger.Log($"Found {weatherData.Count} records for {stationId} in {state}."))
            .Verifiable();
        
        this.MockDynamoDBContext.Setup(context => context
            .DeleteAsync(It.IsAny<WeatherData>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        this.MockDynamoDBContext.Setup(context => context
            .SaveAsync(It.IsAny<WeatherData>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        this.MockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(File.ReadAllText("MockWebRequest.txt")),
            })
            .Verifiable();

        var result = this.SkyConditionObservations.GetSkyConditionsAsync(stationId, state).Result;

        Assert.NotNull(result);
        Assert.Equal(ExpectedNumberOfWeatherData, result.Count);
        Mock.Verify();
    }
}