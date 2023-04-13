using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Moq;
using WeatherObservations.Data.DynamoDB;
using WeatherObservations.Dependencies.Logger;
using WeatherObservations.Dependencies.WeatherObservations;
using WeatherObservations.IntentHandlers.WeatherObservations;
using Xunit;

namespace WeatherObservations.Tests.IntentHandlers.WeatherObservations;

public class WeatherObservationsIntentHandlerTests
{
    private Mock<ILogger> LoggerMock { get; set; }

    private Mock<ISkyConditionObservations> Observations { get; set; }

    private WeatherObservationsIntentHandler IntentHandler { get; set; }

    private const string AirportSlot = "airport";

    private const string DateSlot = "date";

    private const string WeatherObservationsIntent = "WeatherObservationsIntent";

    public WeatherObservationsIntentHandlerTests()
    {
        this.Observations = new();
        this.LoggerMock = new();

        this.LoggerMock.Setup(l => l.Log(It.IsAny<string>()));

        this.IntentHandler = new(this.LoggerMock.Object, this.Observations.Object);
    }

    [Fact]
    public void TestIntentName()
    {
        Assert.Equal("WeatherObservationsIntent", this.IntentHandler.IntentName);
    }

    [Theory]
    [InlineData("KSHN", "WA", "2021-01-01")]
    [InlineData("KSHN", "WA", null)]
    public void TestHandleIntentAsync_ValidStation(string icao, string state, string dateString)
    {
        SkillRequest request = new SkillRequestBuilder()
            .WithIntent(WeatherObservationsIntent)
            .WithSlotResolutionId(AirportSlot, $"{icao}:{state}")
            .WithSlotValue(DateSlot, dateString)
            .Build();

        // Can be a string or null value 
        this.Observations.Setup(o => o.GetSkyConditionsAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Dictionary<DateTime, WeatherData>()
            {
                {DateTime.Now, new WeatherData()}
            })
            .Verifiable();

        var response = this.IntentHandler.HandleIntentAsync((IntentRequest)request.Request);

        Assert.NotNull(response);
        Assert.NotNull(response.Result);
        Assert.NotNull(response.Result.Response);
        Assert.NotNull(response.Result.Response.OutputSpeech);
        Mock.Verify();
        if (dateString == null)
        {
            Assert.Contains("skybro reporting", (response.Result.Response.OutputSpeech as PlainTextOutputSpeech)!.Text.ToLower());
            this.LoggerMock.Verify(l => l.Log(It.Is<string>(s => s.Contains("Error parsing slot values"))), Times.Once);
        }
    }
}